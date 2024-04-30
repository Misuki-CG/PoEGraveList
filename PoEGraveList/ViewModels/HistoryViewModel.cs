using PoEGraveList.Commands;
using PoEGraveList.Core.Attribute;
using PoEGraveList.Core.Shop;
using PoEGraveList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PoEGraveList.ViewModels
{
    class HistoryViewModel : ViewModelBase
    {

        private ICommand _deleteCommand;
        private ICommand _loadQueryCommand;
        private ICommand _updateShopitem;
        private ShopHistory[] _history;
        private ShopHistoryManager _historyManager;
        private ShopHistory? _selectedHistory;
        private ShopItem? _selectedShopItem;
        private ShopItem[] _shopItems;
        private string _typedUrl;

        public ICommand DeleteCommand
        {
            get { return _deleteCommand; }
            set { _deleteCommand = value; OnPropertyChanged(nameof(DeleteCommand)); }
        }
        public ICommand LoadQueryCommand
        {
            get { return _loadQueryCommand; }
            set { _loadQueryCommand = value; OnPropertyChanged(nameof(LoadQueryCommand)); }
        }

        public ICommand UpdateShopItem
        {
            get { return _updateShopitem; }
            set { _updateShopitem = value; OnPropertyChanged(nameof(UpdateShopItem)); }
        }

        public ShopHistory[] History
        {
            get { return _history; }
            set { _history = value; OnPropertyChanged(nameof(History)); }  
        }

        public ShopHistory? SelectedHistory
        {
            get { return _selectedHistory; }
            set
            {
                _selectedHistory = value;
                OnPropertyChanged(nameof(SelectedHistory));

                this.ShopItems = value != null ? value.ShopItems : [];
            }
        
        }

        public ShopItem[] ShopItems
        {
            get { return _shopItems; }
            set { _shopItems = value; OnPropertyChanged(nameof(ShopItems)); }
        }
        public ShopItem? SelectedShopItem
        {
            get { return _selectedShopItem; }
            set { _selectedShopItem = value; OnPropertyChanged(nameof(SelectedShopItem)); }
        }

        public string TypedUrl
        {
            get{ return _typedUrl; }
            set { _typedUrl = value; OnPropertyChanged(nameof(TypedUrl)); }
        }
     
        public HistoryViewModel() 
        {
            this._historyManager = new ShopHistoryManager();
            System.Threading.Thread.Sleep(1000);
            this._deleteCommand = new BaseCommand((obj) => this.onDeleteHistory());
            this._loadQueryCommand = new BaseCommand((obj) => this.loadQuery());
            this._updateShopitem = new BaseCommand(this.updateShopItem);
            this._history = this._historyManager.GetOrderedHistory() ?? [];
            this._selectedHistory = _history.Length > 0 ? _history.First() : null;
            this._typedUrl = "https://www.craftofexile.com/?b=20&m=graveyard&ob=both&v=d&a=e&l=a&lg=16&bp=y&as=1&hb=0&req={%221970%22:{%22l%22:82,%22g%22:2},%221972%22:{%22l%22:82,%22g%22:3},%221974%22:{%22l%22:82,%22g%22:1},%221985%22:{%22l%22:73,%22g%22:1}}&bld={}&im={}&ggt=|&ccp={}&gvc={%22weight%22:{%221%22:{%22-300%22:2},%222%22:{%22-300%22:3},%225%22:{%22-300%22:4},%226%22:{%22-300%22:1},%228%22:{%22500%22:2},%2214%22:{%22-300%22:5},%2215%22:{%22500%22:3},%2234%22:{%22500%22:1},%2235%22:{%22500%22:4}},%22tiers%22:{%220%22:{%2250%22:1}},%22prefix%22:{%220%22:{%22300%22:2}},%22suffix%22:{%220%22:{%22300%22:3}},%22explicit%22:{%220%22:{%221%22:4}}}&af={%2229%22:%221%22,%2233%22:%221%22}";
            this._shopItems = this.SelectedHistory == null ? [] : this.SelectedHistory.ShopItems;
            this._selectedShopItem = this.ShopItems == null ? null : this.ShopItems.First();
        }

        public void AddHistory(ShopItem[] items)
        {
            ShopHistory history = this._historyManager.CreateFromShopItems(items);
            this.History = this._historyManager.GetOrderedHistory();
            this.SelectedHistory = history;
        }

        private void onDeleteHistory()
        {
            this._historyManager.DeleteHistory(this.SelectedHistory);
            this.History = this._historyManager.GetOrderedHistory();
            this.SelectedHistory = this.History.Length > 0 ? this.History[0] : null;
            
        }
        
        private void updateShopItem(object? obj)
        {
            if (obj == null || !(obj is string)) return;
            bool isIncrement = (obj as string)!.ToUpper() == "UP";

            ShopItem historyShopItem = this.SelectedHistory.ShopItems.Single(i => i.Attribute.Key == this.SelectedShopItem.Attribute.Key);
            historyShopItem.CurrentAmount += isIncrement ? 1 : -1;
            this.SelectedShopItem = historyShopItem;
            OnPropertyChanged(nameof(SelectedHistory));
            OnPropertyChanged(nameof(SelectedShopItem.CurrentAmount));
        }
        private void loadQuery()
        {
            ShopItem[] shopItems = ShopItemCreator.FromLink(this.TypedUrl);
            foreach (ShopItem item in shopItems)
            {
                switch (item.Attribute.Key)
                {
                    case "tiers":
                        item.Attribute.SetAttributeDescription($"+{item.Attribute.ValueUp} Modifier Tier Rating");
                        break;
                    case "prefix":
                        item.Attribute.SetAttributeDescription($"{item.Attribute.ValueUp}% increased chance for Prefix Modifiers");
                        break;
                    case "suffix":
                        item.Attribute.SetAttributeDescription($"{item.Attribute.ValueUp}% increased chance for Suffix Modifiers");
                        break;
                    case "explicit":
                        item.Attribute.SetAttributeDescription($"+{item.Attribute.ValueUp} Explicit modifiers");
                        break;
                    case "haunted":
                        item.Attribute.SetAttributeDescription($"Armor can roll Haunted modifiers, {item.Attribute.ValueUp}% increased chance of Haunted Modifiers");
                        break;
                    default:
                        item.Attribute.SetAttributeDescription(item.IsUpValue);
                        break;
                }
            }

            this.AddHistory(shopItems);
        }
    }
}
