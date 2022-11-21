using _4NH_HAO_Coffee_Shop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CoffeeShop.ViewModel
{
    class LoginViewModel: BaseViewModel
    {
        public ICommand LoginCommand { get; set; }

        private string email { get; set; } = "";
        private string password { get; set; } = "";
        public string _email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
                OnPropertyChanged();
            }
        }
        public string _password
        {
            get { return password; } 
            set { password = value; OnPropertyChanged(); }
        }
        public LoginViewModel()
        {
            
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { LoggedIn(p); });

        }

        //
        //PROCESSING FUNCTIONS
        //
        public void LoggedIn(Window p)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
            p.Close();
        }
    }
}
