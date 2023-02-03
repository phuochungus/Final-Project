using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class PromoWrapper : BaseViewModel
    {
        Promo promo;
        public string Id
        {
            get
            {
                return promo.Id;
            }
            set
            {
                promo.Id = value;
                OnPropertyChanged();
            }
        }
        public string DisplayName
        {
            get
            {
                return promo.DisplayName;
            }
            set
            {
                promo.DisplayName = value;
                OnPropertyChanged();
            }
        }
        public string Script
        {
            get
            {
                return promo.Script;
            }
            set
            {
                promo.Script = value;
                OnPropertyChanged();
            }
        }
        public Nullable<System.DateTime> StartTime
        {
            get
            {
                return promo.StartTime;
            }
            set
            {
                promo.StartTime = value;
                OnPropertyChanged();
            }
        }
        public Nullable<System.DateTime> EndTime
        {
            get
            {
                return promo.EndTime;
            }
            set
            {
                promo.EndTime = value;
                OnPropertyChanged();
            }
        }
    }
}
