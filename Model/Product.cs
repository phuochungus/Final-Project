using _4NH_HAO_Coffee_Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{
    public class Product : BaseViewModel, ICloneable
    {
        private Item item;

        public Item Item
        {
            get => item;
            set
            {
                if (item != value)
                    item = value;
                OnPropertyChanged();
            }
        }
        private int quantity;

        public int Quantity
        {
            get => quantity;
            set
            {
                if (this.quantity != value)
                    this.quantity = value;
                OnPropertyChanged();
            }
        }

        public Product(Item item, int value)
        {
            Item = item;
            Quantity = value;
        }

        public object Clone()
        {
            return new Product(item, Quantity);
        }
    }
}