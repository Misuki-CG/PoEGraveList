using Newtonsoft.Json;
using PoEGraveList.Core.Misc;
using PoEGraveList.Models;

namespace PoEGraveList.Core.Shop
{
    public class ShopHistoryManager
    {
        private static readonly string HistoryFilePath = "./queryHistory.json";

        private ShopHistory[] _fullHistory;
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
            _fileHelper.Overwrite(JsonConvert.SerializeObject(this._fullHistory));

            return history;
        }

        public ShopHistory[] GetOrderedHistory()
        {
            return this._fullHistory.OrderByDescending((item) => item.QueryDate).ToArray();
        }

        public void DeleteHistory(ShopHistory? selectedHistory)
        {
            if (selectedHistory == null) return;

            this._fullHistory = [.. this._fullHistory.Where(item => item.Id != selectedHistory.Id)];
            _fileHelper.Overwrite(JsonConvert.SerializeObject(this._fullHistory));
         
        }

        public void UpdateShopItem(ShopHistory selectedHistory, ShopItem selectedItem, bool isIncrement)
        {

            this._fullHistory.Single(h => h.Id == selectedHistory.Id)
                .ShopItems.Single(i => i.Attribute.Key == selectedItem.Attribute.Key)
                .CurrentAmount += isIncrement ? 1 : -1;


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
