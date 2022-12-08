using _4NH_HAO_Coffee_Shop.Utils;
using _4NH_HAO_Coffee_Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{
    public class Cart : BaseViewModel
    {
        private int total { get; set; } = 0;
        public int Total
        {
            get => total;
            set
            {
                if (total != value)
                {
                    total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }
        private FullyObservableCollection<Product> productList { get; set; }
        public FullyObservableCollection<Product> ProductList
        {
            get => productList;
            set
            {
                if (productList != value)
                {
                    productList = value;
                    OnPropertyChanged(nameof(ProductList));
                }
            }
        }
        private int id { get; set; }
        public int Id
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }


        public Cart()
        {
            ProductList = new FullyObservableCollection<Product>();
            ProductList.CollectionChanged += (s, a) =>
            {
                Console.WriteLine(a.Action);
            };
            ProductList.ItemPropertyChanged += (s, a) =>
            {   
                foreach(var product in (s as List<Product>))
                {
                    Total += (product.Value - product.PreValue) * product.Key.Price;
                    if (product.Value == 0) ProductList.Remove(product);
                }
                Console.WriteLine(1);
            };
        }
    }
}
