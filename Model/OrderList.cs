using _4NH_HAO_Coffee_Shop.Utils;
using _4NH_HAO_Coffee_Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{
    public class OrderList : BaseViewModel
    {
        private ObservableCollection<Order> list;

        public ObservableCollection<Order> List
        {
            get => list;
            set
            {
                if (list != value)
                {
                    list = value;
                    notifyPropertyChange(nameof(list));
                }
            }

        }
        public void Add(Order order)
        {
            list.Add((Order)order.Clone());
            notifyPropertyChange(nameof(list));
        }
        public OrderList()
        {
            List = new ObservableCollection<Order>();

        }

    }
}
