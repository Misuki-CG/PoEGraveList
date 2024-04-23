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

        public ShopHistoryManager() 
        {
            this._fullHistory = [];
            this._fileHelper = new FileAsyncHelper(HistoryFilePath);


            Task.Run(async () => this._fullHistory = await this.retrieveSavedHistoric());
         
        }

        public ShopHistory CreateFromShopItems(ShopItem[] items)
        {
            DateTime nowDateTime = DateTime.Now;
            int lastId = _fullHistory.Count() > 0 ? _fullHistory.OrderByDescending((item) => item.Id).First().Id + 1 : 0;
            ShopHistory history = new ShopHistory()
            {
                Id = lastId,
                ShopItems = items,
                ShopName = $"Query ({nowDateTime.ToString("en-US")})",
                QueryDate = nowDateTime
            };

            this._fullHistory = [history, .. this._fullHistory];
            _ = _fileHelper.Overwrite(JsonConvert.SerializeObject(this._fullHistory));

            return history;
        }

        public ShopHistory[] GetOrderedHistory()
        {
            return this._fullHistory.OrderByDescending((item) => item.QueryDate).ToArray();
        }

        public void DeleteHistory(ShopHistory? selectedHistory)
        {
            if (selectedHistory == null) return;

            this._fullHistory = this._fullHistory.Where((item) => item.Id != selectedHistory.Id);
            _ = _fileHelper.Overwrite(JsonConvert.SerializeObject(this._fullHistory));
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
