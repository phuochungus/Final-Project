using _4NH_HAO_Coffee_Shop.Model;
using _4NH_HAO_Coffee_Shop.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class HRViewModel : BaseViewModel
    {
        private FullyObservableCollection<Account> ListDemo;

        private FullyObservableCollection<AccountWrapper> _List;
        public FullyObservableCollection<AccountWrapper> List
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

        private AccountWrapper _Selecteditem;
        public AccountWrapper Selecteditem
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

        private void AccountToList()
        {
            ListDemo = new FullyObservableCollection<Account>(DataProvider.Ins.DB.Accounts);
            List = new FullyObservableCollection<AccountWrapper>();
            foreach (Account ac in ListDemo)
            {
                AccountWrapper acw = new AccountWrapper()
                {
                    account = new Account(),
                    Id = ac.Id,
                    DisplayName = ac.DisplayName,
                    Email = ac.Email,
                    Password = ac.Password,
                    PhoneNumber = ac.PhoneNumber,
                    AccountType = ac.AccountType,
                    ManagedBy = ac.ManagedBy,
                    ImageURL = ac.ImageURL,
                };              
                List.Add(acw);
            }
        }
        public HRViewModel()
        {
            AccountToList();

            AddCommand = new RelayCommand<object>((p) => {
                // Kiểm tra xem id nhập vào đã tồn tại hay chưa
                var Idlist = DataProvider.Ins.DB.Accounts.Where(x => x.Id == Id);
                if (Idlist == null || Idlist.Count() != 0) return false;
                // Kiểm tra các textbox nhập thông tin có hợp lệ
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(DisplayName) ||
                     string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) ||
                     string.IsNullOrEmpty(PhoneNumber)) return false;
                // Kiểm tra nếu AccountType là staff thì thông tin Id người quản lý có hợp lệ
                if (AccountType == "staff")
                {
                    if (string.IsNullOrEmpty(ManagedBy)) return false;
                    var ManagedByList = DataProvider.Ins.DB.Accounts.Where(x => x.Id == ManagedBy);
                    if (ManagedByList == null || ManagedByList.Count() != 1) return false;
                }
                // Kiểm tra nếu AccountType là admin thì thông tin ManageBy có hợp lệ 
                else if (AccountType == "admin")
                {
                    if (!string.IsNullOrEmpty(ManagedBy)) return false;
                }
                else return false;
                return true;
            },
            (p) => {
                // Khởi tạo account mới tùy theo  AccountType là staff hoặc admin
                AccountWrapper NewaAcount;
                if (AccountType == "staff")
                {
                    NewaAcount = new AccountWrapper()
                    {
                        account = new Account(),
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
                    NewaAcount = new AccountWrapper()
                    {
                        account = new Account(),
                        Id = Id,
                        DisplayName = DisplayName,
                        Email = Email,
                        Password = CreateMD5(Password),
                        PhoneNumber = PhoneNumber,
                        AccountType = AccountType,
                        ImageURL = @"https://i.ibb.co/gD6SVPT/Cat.jpg",
                    };
                };
                // Thêm account mới vào database
                DataProvider.Ins.DB.Accounts.Add(NewaAcount.account);

                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                // Thêm account mới vào list account
                List.Add(NewaAcount);
            });

            ModifyCommand = new RelayCommand<object>((p) => {
                // Kiểm tra có chọn Selecteditem chưa
                if (Selecteditem == null) return false;

                // Kiểm tra các thông tin trên textbox có hợp lệ vào Id có bị thay đổi không
                if (Id != Selecteditem.Id ) return false;
                if (string.IsNullOrEmpty(Id) || 
                    string.IsNullOrEmpty(DisplayName) ||
                    string.IsNullOrEmpty(Email) || 
                    string.IsNullOrEmpty(Password) ||
                    string.IsNullOrEmpty(PhoneNumber)) return false;

                if (AccountType == "staff")
                {
                    // Kiểm tra thông tin ManagedBy có hợp lệ của account staff 
                    if (string.IsNullOrEmpty(ManagedBy)) return false;
                    // Lấy list tài khoản có id trùng với id người quản lý xem có chính xác
                    var ManagedByList = DataProvider.Ins.DB.Accounts.Where(x => x.Id == ManagedBy && x.AccountType == "admin");
                    if (ManagedByList == null || ManagedByList.Count() != 1) return false;
                }
                else if (AccountType == "admin")
                {
                    // Kiểm tra thông tin ManagedBy có hợp lệ của account admin
                    if (!string.IsNullOrEmpty(ManagedBy)) return false;
                }
                else return false;
                return true;
            },
            (p) => {
                // Tìm thông tin account tương ứng trong database để cập nhật thông tin mới
                var account = DataProvider.Ins.DB.Accounts.Where(x => x.Id == Selecteditem.Id).SingleOrDefault();

                account.Id = Id;
                account.DisplayName = DisplayName;
                account.Email = Email;
                if (Password != Selecteditem.Password) account.Password = CreateMD5(Password);
                account.PhoneNumber = PhoneNumber;
                account.AccountType = AccountType;
                if (AccountType == "staff") account.ManagedBy = ManagedBy;
                else account.ManagedBy = null;
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();
                // Lưu thay đổi tại Selecteditem của list account
                Selecteditem.Id = Id;
                Selecteditem.DisplayName = DisplayName;
                Selecteditem.Email = Email;
                if (Password != Selecteditem.Password) Selecteditem.Password = CreateMD5(Password);
                Selecteditem.PhoneNumber = PhoneNumber;
                Selecteditem.AccountType = AccountType;
                Selecteditem.ManagedBy = ManagedBy;
            });

            DeleteCommand = new RelayCommand<object>((p) => {
                // Kiểm tra có chọn Selecteditem chưa
                if (Selecteditem == null) return false;

                // Kiểm tra các thông tin trên textbox có bị thay đổi không
                if (Id != Selecteditem.Id || 
                   DisplayName != Selecteditem.DisplayName || 
                   Email != Selecteditem.Email || 
                   Password != Selecteditem.Password ||
                   PhoneNumber != Selecteditem.PhoneNumber || 
                   AccountType != Selecteditem.AccountType ||
                   ManagedBy != Selecteditem.ManagedBy)
                    return false;
                if (AccountType == "admin")
                {
                    // Với account admin kiểm tra xem có tài khoản nào đang chịu quản lý bời account cần xóa không, nếu có thì không thể xóa 
                    var Idlist = DataProvider.Ins.DB.Accounts.Where(x => x.ManagedBy == Id);
                    if (Idlist == null || Idlist.Count() != 0) return false;
                }
                return true;
            },
            (p) => {
                
                // Xóa account có id tương ứng trong database
                DataProvider.Ins.DB.Accounts.Remove(Selecteditem.account);

                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                // Xóa account trong list account 
                List.Remove(Selecteditem);

                // Đưa thông tin các textbox về null
                Id = DisplayName = Email = Password = PhoneNumber = AccountType = ManagedBy = null;
            });
        }
    }
}
