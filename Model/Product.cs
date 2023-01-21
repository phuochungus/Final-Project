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
    public class Product : BaseViewModel,ICloneable
    {
        private Item key { get; set; }
        public Item Key
        {
            get => key;
            set
            {
                if (key == value) return;
                key = value;
                notifyPropertyChange(nameof(Key));
            }
        }
        private int value { get; set; } = 0;
        unsafe public int Value
        {
            get => value;
            set
            {
                if (this.value == value) return;
                this.value = value;
                notifyPropertyChange(nameof(Value));
            }
        }
        unsafe public Product(Item item, int value) { Key = item; Value = value; }

        public object Clone()
        {
            return new Product(Key, Value);
        }
    }
}
