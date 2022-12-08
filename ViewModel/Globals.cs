﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using _4NH_HAO_Coffee_Shop.Model;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class Globals : BaseViewModel
    {
        private Account currUser { get; set; }
        public Account CurrUser
        {
            get => currUser; set
            {
                if (currUser == value) return;
                currUser = value;
                OnPropertyChanged();
            }
        }
        private bool _isAdmin { get; set; }
        public bool isAdmin
        {
            get => _isAdmin;
            set
            {
                if (_isAdmin == value) return;
                _isAdmin = value;
                OnPropertyChanged();
            }
        }
        private Cart _currBill { get; set; } = new Cart();
        public Cart CurrBill
        {
            get => _currBill;
            set
            {
                if (_currBill != value)
                {
                    _currBill = value;
                    OnPropertyChanged();
                }
            }
        }


        private Globals() { }

        private static Globals _instance;
        public static Globals Instance
        {
            get { return _instance ?? (_instance = new Globals()); }
        }

        public bool Update(Item item, int quantity)
        {
            for (int i = 0; i < CurrBill.ProductList.Count; ++i)
            {
                if (CurrBill.ProductList[i].Key.Id == item.Id)
                {
                    CurrBill.Total += (quantity - CurrBill.ProductList[i].Value);
                    CurrBill.ProductList[i].Value = quantity;
                    if (quantity == 0) CurrBill.ProductList.RemoveAt(i);
                    Console.WriteLine(item.DisplayName, quantity);
                    Instance.OnPropertyChanged();
                    return true;
                }
            }
            return false;
        }
        public bool Insert(Item item)
        {
            CurrBill.Total += item.Price;

            for (int i = 0; i < CurrBill.ProductList.Count; ++i)
            {
                if (CurrBill.ProductList[i].Key.Id == item.Id)
                {
                    CurrBill.ProductList[i].Value++;
                    Instance.OnPropertyChanged(nameof(CurrBill));
                    return true;
                }
            }
            CurrBill.ProductList.Add(new Product(item, 1));
            Instance.OnPropertyChanged(nameof(CurrBill));
            return true;
        }
        public bool Delete(Item item)
        {
            for (int i = 0; i < CurrBill.ProductList.Count; ++i)
            {
                if (CurrBill.ProductList[i].Key.Id == item.Id)
                {
                    CurrBill.Total -= item.Price;
                    CurrBill.ProductList[i].Value--;
                    if (CurrBill.ProductList[i].Value == 0) CurrBill.ProductList.RemoveAt(i);
                    Instance.OnPropertyChanged(nameof(CurrBill));
                    return true;
                }
            }
            return false;
        }
    }
}
