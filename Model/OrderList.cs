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
        private ObservableCollection<Order> orders;

        public ObservableCollection<Order> Orders
        {
            get => orders;
            set
            {
                if (orders != value)
                {
                    orders = value;
                    OnPropertyChanged(nameof(Orders));
                }
            }

        }
        public void Add(Order order)
        {
            orders.Add((Order)order.Clone());
            OnPropertyChanged(nameof(orders));
        }
        public OrderList()
        {
            Orders = new ObservableCollection<Order>();

        }

    }
}
