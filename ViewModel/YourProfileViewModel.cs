using _4NH_HAO_Coffee_Shop.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class YourProfileViewModel : BaseViewModel
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
        private string _ImageSource = @"\Assets\Image\avatar.png";
        public string ImageSource
        {
            get => _ImageSource;
            set
            {
                _ImageSource = value;
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
            AvatarCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "File ảnh| *.png;*.jpeg";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    ImageSource = open.FileName;
                }

            });
        }
    }
    
}
