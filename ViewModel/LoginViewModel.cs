using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace CoffeeShop.ViewModel
{
    class LoginViewModel: BaseViewModel
    {
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
            Task.Run(() =>
            {
                while(true)
                {
                    Debug.WriteLine("email: "+ _email);
                    Debug.WriteLine("password: " + _password);
                    Thread.Sleep(500);
                }
            });
        }
    }
}
