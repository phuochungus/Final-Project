using _4NH_HAO_Coffee_Shop.View;
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
            ShowHRViewCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                HRView hr = new HRView();
                hr.ShowDialog();

            });
        }
    }
    
}
