using _4NH_HAO_Coffee_Shop.Model;
using _4NH_HAO_Coffee_Shop.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class HRViewModel : BaseViewModel
    {
        private FullyObservableCollection<Account> _List;
        public FullyObservableCollection<Account> List
        {
            get => _List;
            set
            {
                _List = value;
                OnPropertyChanged();
            }
        }

        public string CreateMD5(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            return encoded;
        }

        private Account _Selecteditem;
        public Account Selecteditem
        {
            get => _Selecteditem;
            set
            {
                _Selecteditem = value;
                OnPropertyChanged();
                if (Selecteditem != null)
                {
                    Id = Selecteditem.Id;
                    DisplayName = Selecteditem.DisplayName;
                    Email = Selecteditem.Email;
                    PhoneNumber = Selecteditem.PhoneNumber;
                    Password = Selecteditem.Password;
                    AccountType = Selecteditem.AccountType;
                    ManagedBy = Selecteditem.ManagedBy;
                }
            }
        }

        private string _Id;
        public string Id
        {
            get => _Id;
            set
            {
                _Id = value;
                OnPropertyChanged();

            }
        }

        private string _DisplayName;
        public string DisplayName
        {
            get => _DisplayName;
            set
            {
                _DisplayName = value;
                OnPropertyChanged();

            }
        }

        private string _Email;
        public string Email
        {
            get => _Email;
            set
            {
                _Email = value;
                OnPropertyChanged();

            }
        }

        private string _PhoneNumber;
        public string PhoneNumber
        {
            get => _PhoneNumber;
            set
            {
                _PhoneNumber = value;
                OnPropertyChanged();

            }
        }

        private string _Password;
        public string Password
        {
            get => _Password;
            set
            {
                _Password = value;
                OnPropertyChanged();

            }
        }

        private string _AccountType;
        public string AccountType
        {
            get => _AccountType;
            set
            {
                _AccountType = value;
                OnPropertyChanged();

            }
        }

        private string _ManagedBy;
        public string ManagedBy
        {
            get => _ManagedBy;
            set
            {
                _ManagedBy = value;
                OnPropertyChanged();

            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand ModifyCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public HRViewModel()
        {
            List = new FullyObservableCollection<Account>(DataProvider.Ins.DB.Accounts);
            AddCommand = new RelayCommand<object>((p) => {
                var Idlist = DataProvider.Ins.DB.Accounts.Where(x => x.Id == Id);
                if (Idlist == null || Idlist.Count() != 0) return false;
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(DisplayName) ||
                     string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) ||
                     string.IsNullOrEmpty(PhoneNumber)) return false;
                if (AccountType == "staff")
                {
                    if (string.IsNullOrEmpty(ManagedBy)) return false;
                    var ManagedByList = DataProvider.Ins.DB.Accounts.Where(x => x.Id == ManagedBy);
                    if (ManagedByList == null || ManagedByList.Count() != 1) return false;
                }
                else if (AccountType == "admin")
                {
                    if (!string.IsNullOrEmpty(ManagedBy)) return false;
                }
                else return false;
                return true;
            },
            (p) => {
                Account account;
                if (AccountType == "staff")
                {
                    account = new Account()
                    {
                        Id = Id,
                        DisplayName = DisplayName,
                        Email = Email,
                        Password = CreateMD5(Password),
                        PhoneNumber = PhoneNumber,
                        AccountType = AccountType,
                        ManagedBy = ManagedBy,
                        ImageURL = @"https://i.ibb.co/gD6SVPT/Cat.jpg",

                    };
                }
                else
                {
                    account = new Account()
                    {
                        Id = Id,
                        DisplayName = DisplayName,
                        Email = Email,
                        Password = CreateMD5(Password),
                        PhoneNumber = PhoneNumber,
                        AccountType = AccountType,
                        ImageURL = @"https://i.ibb.co/gD6SVPT/Cat.jpg",
                    };
                };
                DataProvider.Ins.DB.Accounts.Add(account);
                DataProvider.Ins.DB.SaveChanges();
                List.Add(account);
            });

            ModifyCommand = new RelayCommand<object>((p) => {
                if (string.IsNullOrEmpty(Id)) return false;
                if (Selecteditem == null) return false;
                if (Id != Selecteditem.Id ) return false;
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(DisplayName) ||
                     string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) ||
                     string.IsNullOrEmpty(PhoneNumber)) return false;
                if (AccountType == "staff")
                {
                    if (string.IsNullOrEmpty(ManagedBy)) return false;
                    var ManagedByList = DataProvider.Ins.DB.Accounts.Where(x => x.Id == ManagedBy && x.AccountType == "admin");
                    if (ManagedByList == null || ManagedByList.Count() != 1) return false;
                }
                else if (AccountType == "admin")
                {
                    if (!string.IsNullOrEmpty(ManagedBy)) return false;
                }
                else return false;
                return true;
            },
            (p) => {
                var account = DataProvider.Ins.DB.Accounts.Where(x => x.Id == Selecteditem.Id).SingleOrDefault();

                account.Id = Id;
                account.DisplayName = DisplayName;
                account.Email = Email;
                if (Password != Selecteditem.Password) account.Password = CreateMD5(Password);
                account.PhoneNumber = PhoneNumber;
                account.AccountType = AccountType;
                if (AccountType == "staff") account.ManagedBy = ManagedBy;
                else account.ManagedBy = null;
                DataProvider.Ins.DB.SaveChanges();
                Selecteditem.Id = Id;
                Selecteditem.DisplayName = DisplayName;
                Selecteditem.Email = Email;
                if (Password != Selecteditem.Password) Selecteditem.Password = CreateMD5(Password);
                Selecteditem.PhoneNumber = PhoneNumber;
                Selecteditem.AccountType = AccountType;
                Selecteditem.ManagedBy = ManagedBy;
            });

            DeleteCommand = new RelayCommand<object>((p) => {
                if (Selecteditem == null) return false;
                if (Id != Selecteditem.Id || DisplayName != Selecteditem.DisplayName || Email != Selecteditem.Email ||
                    Password != Selecteditem.Password || PhoneNumber != Selecteditem.PhoneNumber || AccountType != Selecteditem.AccountType ||
                    ManagedBy != Selecteditem.ManagedBy)
                    return false;
                if (AccountType == "admin")
                {
                    var Idlist = DataProvider.Ins.DB.Accounts.Where(x => x.ManagedBy == Id);
                    if (Idlist == null || Idlist.Count() != 0) return false;
                }
                return true;
            },
            (p) => {
                DataProvider.Ins.DB.Accounts.Remove(Selecteditem);
                DataProvider.Ins.DB.SaveChanges();
                List.Remove(Selecteditem);
                Id = DisplayName = Email = Password = PhoneNumber = AccountType = ManagedBy = null;
            });
        }
    }
}
