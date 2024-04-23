using PoEGraveList.Commands;
using PoEGraveList.Core.Shop;
using PoEGraveList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PoEGraveList.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private string _typedUrl;
        private ICommand _loadCommand;
        private ShopItem[] _weightList;

        private ShopHistoryManager _historyManager;
        public ShopItem[] WeightList
        {
            get
            {
                return _weightList;
            }
            set
            {
                _weightList = value;
                OnPropertyChanged(nameof(WeightList));
            }
        }
        public string TypedUrl
        {
            get
            {
                return _typedUrl;
            }
            set
            {
                _typedUrl = value;
                OnPropertyChanged(nameof(TypedUrl));
            }
        }
        public ICommand LoadCommand
        {
            get
            {
                return _loadCommand;
            }
            set
            {
                _loadCommand = value;
                OnPropertyChanged(nameof(LoadCommand));
            }
        }

        public MainWindowViewModel() 
        {
            this._typedUrl = "https://www.craftofexile.com/?b=20&m=graveyard&ob=both&v=d&a=e&l=a&lg=16&bp=y&as=1&hb=0&req={%221970%22:{%22l%22:82,%22g%22:2},%221972%22:{%22l%22:82,%22g%22:3},%221974%22:{%22l%22:82,%22g%22:1},%221985%22:{%22l%22:73,%22g%22:1}}&bld={}&im={}&ggt=|&ccp={}&gvc={%22weight%22:{%221%22:{%22-300%22:2},%222%22:{%22-300%22:3},%225%22:{%22-300%22:4},%226%22:{%22-300%22:1},%228%22:{%22500%22:2},%2214%22:{%22-300%22:5},%2215%22:{%22500%22:3},%2234%22:{%22500%22:1},%2235%22:{%22500%22:4}},%22tiers%22:{%220%22:{%2250%22:1}},%22prefix%22:{%220%22:{%22300%22:2}},%22suffix%22:{%220%22:{%22300%22:3}},%22explicit%22:{%220%22:{%221%22:4}}}&af={%2229%22:%221%22,%2233%22:%221%22}";
            this._loadCommand = new BaseCommand((obj) => this.loadQuery());
            this._weightList = [];
            this._historyManager = new ShopHistoryManager();
        }


        private void loadQuery()
        {
            ShopItem[] shopItems = ShopItemCreator.FromLink(this.TypedUrl);
            foreach(ShopItem item in shopItems)
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

            this._historyManager.CreateFromShopItems(shopItems);
            this.WeightList = shopItems;
        }

       
    }
}
