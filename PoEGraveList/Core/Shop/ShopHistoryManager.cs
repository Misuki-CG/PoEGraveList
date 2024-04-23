using Newtonsoft.Json;
using PoEGraveList.Core.Misc;
using PoEGraveList.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoEGraveList.Core.Shop
{
    public class ShopHistoryManager
    {
        private static readonly string HistoryFilePath = "./queryHistory.json";

        private IEnumerable<ShopHistory> _fullHistory;
        private FileAsyncHelper _fileHelper;

        public ShopHistory[] FullHistory
        {
            get
            {
                return this._fullHistory.ToArray();
            }
        }

        public ShopHistoryManager() 
        {
            this._fullHistory = [];
            this._fileHelper = new FileAsyncHelper(HistoryFilePath);


            Task.Run(async () => this._fullHistory = await this.retrieveSavedHistoric());
         
        }

        public async void CreateFromShopItems(ShopItem[] items)
        {
            DateTime nowDateTime = DateTime.Now;
            ShopHistory history = new ShopHistory()
            {
                Id = _fullHistory.Count(),
                ShopItems = items,
                ShopName = $"Query ({nowDateTime.ToString("en-US")})",
                QueryDate = nowDateTime
            };

            this._fullHistory = [history, .. this._fullHistory];
            await _fileHelper.Overwrite(JsonConvert.SerializeObject(this._fullHistory));
        }

        private async Task<ShopHistory[]> retrieveSavedHistoric()
        {
            ShopHistory[]? data = await _fileHelper.ReadAsJson<ShopHistory[]>();
            if (data == null) throw new NullReferenceException();

            data = data.OrderByDescending((item) => item.QueryDate).ToArray();

            return data;
        }

       


    }
}
