using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class HomeViewModel : BaseViewModel
    {

        private List<Item> _itemList;
        public List<Item> ItemList
        {
            get => _itemList;
            set
            {
                _itemList = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel()
        {
            ItemList = DataProvider.Ins.DB.Items.ToList();
        }
    }
}
