﻿using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class OrderedViewModel : BaseViewModel
    {
        public ICommand handleServingFinish { get; set; }
        public OrderedViewModel() {
            handleServingFinish = new RelayCommand<object>(p => true, p =>
            {
                Globals.Instance.OrderQueue.List.Remove(p as Order);
            });
            Console.WriteLine(Globals.Instance.OrderQueue.List.Count());
        }
    }
}
