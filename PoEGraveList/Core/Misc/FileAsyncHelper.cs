using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using PoEGraveList.Core.Misc.Implements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoEGraveList.Core.Misc
{
    public class FileAsyncHelper
    {

        private SemaphoreSlim _semaphore;

        private string _path;
        private AsyncQueue<FileActionWrapper> _queue;
        private bool _isRunning;
        
        public FileAsyncHelper(string path)
        {
            this._semaphore = new SemaphoreSlim(1, 1);
            this._path = path;
            this._queue = new AsyncQueue<FileActionWrapper>();
            this._isRunning = false;
        }

        public void Append(string content)
        {
            this.addToQueue(content, FileWriteMode.Append);
        }

        public void Overwrite(string content)
        {
            this.addToQueue(content, FileWriteMode.Overwrite);
        }
        public async Task<T?> ReadAsJson<T>() where T : class
        {

            T? outputObj = null;
            try
            {
                
                while (this._isRunning)
                    await Task.Delay(50);

                string content = await readAllFile();
                outputObj = JsonConvert.DeserializeObject<T>(content);
                if (outputObj == null) throw new NullReferenceException("Content couldn't be deserialize");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

         

            return outputObj;
           
        }

        private void addToQueue(string content, FileWriteMode mode)
        {
            this._queue.Enqueue(new FileActionWrapper() { Data = content, Mode = mode });

            if (!this._isRunning)
            {
                this._isRunning = true;
                _ = this.onOperationQueued();
            }
        }

        private async Task onOperationQueued()
        {
            
            try
            {
                while(this._queue.Count > 0)
                {
                    FileActionWrapper action = this._queue.Dequeue();
                    switch (action.Mode)
                    {
                        case FileWriteMode.Overwrite:
                            byte[] buffer = Encoding.UTF8.GetBytes(action.Data);
                            await this.overwriteFile(buffer);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                this._isRunning = false;
            }
        }
        private async Task overwriteFile(byte[] buffer)
        {
            await this._semaphore.WaitAsync();
            using (FileStream _stream = new FileStream(this._path, FileMode.Open, FileAccess.Write))
            {

                _stream.SetLength(0);
                await _stream.WriteAsync(buffer, 0, buffer.Length);
                await _stream.FlushAsync();
            }
            this._semaphore.Release();

        }

        private async Task<string> readAllFile()
        {
            string output = String.Empty;
            try
            {
                await this._semaphore.WaitAsync();
                using (FileStream _stream = new FileStream(this._path, FileMode.Open, FileAccess.Read))
                {
                    _stream.Seek(0, SeekOrigin.End);
                    int length = (int)_stream.Position;
                    _stream.Seek(0, SeekOrigin.Begin);

                    byte[] buffer = new byte[length];

                    _stream.Read(buffer, 0, length);
                    output = Encoding.UTF8.GetString(buffer);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                this._semaphore.Release();
            }

            return output;
            

           
        }
        
        private class FileActionWrapper
        {
            public FileWriteMode Mode { get; set; }
            public string Data { get; init; } = "";
        }
    }

    public enum FileWriteMode
    {
        Append,
        Overwrite,
    }
}
