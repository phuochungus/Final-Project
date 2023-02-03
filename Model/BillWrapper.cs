using _4NH_HAO_Coffee_Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{
    public class BillWrapper : BaseViewModel
    {
        public Bill bill;
        public int IdNumber
        {
            get => bill.IdNumber;
            set
            {
                bill.IdNumber = value;
                OnPropertyChanged();
            }
        }
        public System.DateTime ExportTime
        {
            get => bill.ExportTime;
            set
            {
                bill.ExportTime = value;
                OnPropertyChanged();
            }
        }
        public string CustomerId
        {
            get => bill.CustomerId;
            set
            {
                bill.CustomerId = value;
                OnPropertyChanged();
            }
        }
        public string PromoId
        {
            get => bill.CustomerId;
            set
            {
                bill.CustomerId = value;
                OnPropertyChanged();
            }
        }
        public int Total
        {
            get => bill.Total;
            set
            {
                bill.Total = value;
                OnPropertyChanged();
            }
        }

        public virtual Account Account
        {
            get => bill.Account;
            set
            {
                bill.Account = value;
                OnPropertyChanged();
            }
        }
        public virtual Promo Promo
        {
            get => bill.Promo;
            set
            {
                bill.Promo = value;
                OnPropertyChanged();
            }
        }
        public virtual ICollection<BillInfor> BillInfors
        {
            get => bill.BillInfors;
            set
            {
                bill.BillInfors = value;
                OnPropertyChanged();
            }
        }
    }
}
