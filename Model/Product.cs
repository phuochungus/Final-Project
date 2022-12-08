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
    public class Product : BaseViewModel
    {
        public int PreValue { get; set; }
        private Item key { get; set; }
        public Item Key
        {
            get => key;
            set
            {
                if (key == value) return;
                key = value;
                OnPropertyChanged(nameof(Key));
            }
        }
        private int value { get; set; } = 0;
        unsafe public int Value
        {
            get => value;
            set
            {
                if (this.value == value) return;
                PreValue = this.value;
                this.value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
        unsafe public Product(Item item, int value) { Key = item; Value = value; }
    }
}
