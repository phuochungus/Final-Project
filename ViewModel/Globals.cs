using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using _4NH_HAO_Coffee_Shop.Model;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class Globals
    {
        public static Account CurrUser { get; set; }  
        public static bool isAdmin { get; set; }
        public static ObservableCollection<Product> ProductList { get; set; } = new ObservableCollection<Product>();
        public static void Update(Item item, int quantity)
        {
            for (int i = 0; i < ProductList.Count; ++i)
            {
                if (ProductList[i].Key.Id == item.Id)
                {
                    ProductList[i].Value = quantity;
                    if (quantity == 0) ProductList.RemoveAt(i);
                    return;
                }
            }
        }
        public static void Insert(Item item)
        {
            for (int i = 0; i < ProductList.Count; ++i)
            {
                if (ProductList[i].Key.Id == item.Id)
                {
                    ProductList[i].Value++;
                    return;
                }
            }
            ProductList.Add(new Product(item, 1));

        }
        public static void Delete(Item item)
        {
            for (int i = 0; i < ProductList.Count; ++i)
            {
                if (ProductList[i].Key.Id == item.Id)
                {
                    ProductList[i].Value--;
                    if (ProductList[i].Value == 0) ProductList.RemoveAt(i);

                    return;
                }
            }
        }
    }
}
