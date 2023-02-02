using _4NH_HAO_Coffee_Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{
    public class AccountWrapper : BaseViewModel
    {
        public Account account;
        public string Id
        {
            get => account.Id;
            set
            {
                account.Id = value;
                OnPropertyChanged();
            }
        }
        public string DisplayName
        {
            get => account.DisplayName;
            set
            {
                account.DisplayName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => account.Email;
            set
            {
                account.Email = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => account.PhoneNumber;
            set
            {
                account.PhoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => account.Password;
            set
            {
                account.Password = value;
                OnPropertyChanged();
            }
        }

        public string AccountType
        {
            get => account.AccountType;
            set
            {
                account.AccountType = value;
                OnPropertyChanged();
            }
        }

        public string ImageURL
        {
            get => account.ImageURL;
            set
            {
                account.ImageURL = value;
                OnPropertyChanged();
            }
        }

        public string ManagedBy
        {
            get => account.ManagedBy;
            set
            {
                account.ManagedBy = value;
                OnPropertyChanged();
            }
        }

        public virtual ICollection<Account> Account1 
        {
            get => account.Account1; 
            set
            {
                account.Account1 = value;
                OnPropertyChanged();

            }
        }
        public virtual Account Account2
        {
            get => account.Account2;
            set
            {
                account.Account2 = value;
                OnPropertyChanged();

            }
        }

        public virtual ICollection<Bill> Bills 
        {
            get => account.Bills;
            set
            {
                account.Bills = value;
                OnPropertyChanged();
            }
        }
    }
}
