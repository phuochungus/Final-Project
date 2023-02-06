using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class ItemWrapper : BaseViewModel
    {
        public Item item;
        public string Id
        {
            get { return item.Id; }
            set { item.Id = value; OnPropertyChanged(); }
        }
        public string DisplayName
        {
            get { return item.DisplayName; }
            set { item.DisplayName = value; OnPropertyChanged(); }
        }
        public int UnitId
        {
            get { return item.UnitId; }
            set { item.UnitId = value; OnPropertyChanged(); }
        }
        public int CategoryId
        {
            get { return item.CategoryId; }
            set { item.CategoryId = value; OnPropertyChanged(); }
        }
        public int Price
        {
            get { return item.Price; }
            set { item.Price = value; OnPropertyChanged(); }
        }
        public string ImageURL
        {
            get { return item.ImageURL; }
            set { item.ImageURL = value; OnPropertyChanged(); }
        }
    }
}
