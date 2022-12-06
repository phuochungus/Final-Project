using _4NH_HAO_Coffee_Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{

    public class Product
    {
        public Item Key { get; set; }
        public int Value { get; set; }
        public Product(Item item, int value) { Key = item; Value = value; }
    }
}
