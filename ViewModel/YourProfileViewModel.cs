using _4NH_HAO_Coffee_Shop.Model;
using _4NH_HAO_Coffee_Shop.Utils;
using _4NH_HAO_Coffee_Shop.View;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class YourProfileViewModel : BaseViewModel
    {
        public ICommand ShowHRViewCommand { get; set; }

        public ICommand AvatarCommand { get; set; }
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        private string _ImageSource;
        public string ImageSource
        {
            get => _ImageSource;
            set
            {
                _ImageSource = value;
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

        public YourProfileViewModel()
        {
            _DisplayName = Globals.Instance.CurrUser.DisplayName;
            _Email = Globals.Instance.CurrUser.Email;
            _PhoneNumber = Globals.Instance.CurrUser.PhoneNumber;
            _AccountType = Globals.Instance.CurrUser.AccountType;
            _ImageSource = Globals.Instance.CurrUser.ImageURL;
            ShowHRViewCommand = new RelayCommand<object>((p) =>
            {
                if (AccountType == "admin") return true;
                else return false;
            }, p =>
            {
                HRView hr = new HRView();
                hr.ShowDialog();

            });
            AvatarCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "File ảnh| *.png;*.jpeg";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    ImageSource = open.FileName;
                    var account = DataProvider.Ins.DB.Accounts.Where(x => x.Id == Globals.Instance.CurrUser.Id).SingleOrDefault();
                    account.ImageURL = ImageSource;
                }

            });
        }
    }
}
