using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class YourProfileViewModel : BaseViewModel
    {
        public ICommand ShowHRViewCommand { get; set; }
        private object _currentView1;
        public object CurrentView1
        {
            get => _currentView1;
            set
            {
                _currentView1 = value;
                OnPropertyChanged();
            }
        }

        private string _DisplayName = "DisplayName";
        public string DisplayName
        {
            get => _DisplayName;
            set
            {
                _DisplayName = value;
                OnPropertyChanged();

            }
        }
         
        private string _Email = "Email";
        public string Email
        {
            get => _Email;
            set
            {
                _Email = value;
                OnPropertyChanged();

            }
        }

        private string _PhoneNumber = "PhoneNumber";
        public string PhoneNumber
        {
            get => _PhoneNumber;
            set
            {
                _PhoneNumber = value;
                OnPropertyChanged();

            }
        }

        private string _AccountType = "AccountType";
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
            CurrentView1 = new HRViewModel();
            ShowHRViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Console.WriteLine("hr"); CurrentView1 = new HRViewModel(); });
        }
    }
    
}
