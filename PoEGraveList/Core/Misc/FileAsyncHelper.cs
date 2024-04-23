using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
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

        private FileStream _stream;
        private SemaphoreSlim _sync;
        public FileAsyncHelper(string path)
        {
            this._stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            this._sync = new SemaphoreSlim(1); 
        }

        public async Task Append(string content)
        {
            await _sync.WaitAsync();
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                await this.writeAt(buffer, 0, SeekOrigin.End);  
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                _sync.Release();
            }
        }


        public async Task Overwrite(string content)
        {
            await _sync.WaitAsync();
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                await this.writeAt(buffer, 0, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                _sync.Release();
            }
        }
        public async Task<T?> ReadAsJson<T>() where T : class
        {
            await _sync.WaitAsync();
            T? objOutput = null;
            try
            {
                _stream.Seek(0, SeekOrigin.End);
                long fileLength = _stream.Position;
                _stream.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[fileLength];
                int lengthRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                objOutput = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(buffer));
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                _sync.Release();
            }

            return objOutput;
        }

        private async Task writeAt(byte[] buffer, int offset, SeekOrigin origin)
        {
            _stream.Seek(offset, origin);
            await _stream.WriteAsync(buffer, 0, buffer.Length);
            await _stream.FlushAsync();
        }
    }
}
