<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class OrderedViewModel :BaseViewModel
    {
=======
﻿using _4NH_HAO_Coffee_Shop.Model;
using _4NH_HAO_Coffee_Shop.Utils;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class OrderedViewModel : BaseViewModel
    {
        public ICommand handleServingFinish { get; set; }
        public OrderedViewModel() {
            handleServingFinish = new RelayCommand<object>(p => true, p =>
            {
                Globals.Instance.OrderQueue.Orders.Remove(p as Order);
            });
        }
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
    }
}
