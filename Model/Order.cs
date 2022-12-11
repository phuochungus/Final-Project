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
    public class Order : BaseViewModel, ICloneable
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
        public void Clear()
        {
            Total = 0;
            productList.Clear();
            Id = 0;
        }
        public Order()
        {
            ProductList = new FullyObservableCollection<Product>();
            ProductList.CollectionChanged += (s, e) => handeProductListChanged(s, e);
            ProductList.ItemPropertyChanged += (s, e) => handeProductListChanged(s, e);
        }

        public Order(Order obj)
        {
            this.Total = obj.Total;
            this.Id = obj.Id;
        }
        private void handeProductListChanged(object sender, EventArgs e)
        {
            foreach (var product in (sender as FullyObservableCollection<Product>).ToList())
            {
                if (product.Value == 0) { productList.Remove(product); }
            }
            Total = 0;
            foreach (var product in ProductList)
            {
                Total += product.Key.Price * product.Value;
            }
        }

        public object Clone()
        {
            Order clone = new Order();
            clone.total = total;
            clone.id = id;
            foreach (var item in ProductList)
            {
                Product clonedItem = (Product)item.Clone();
                clone.ProductList.Add(clonedItem);
            }
            return clone;
        }
    }
}
