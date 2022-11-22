
using _4NH_HAO_Coffee_Shop;
<<<<<<< HEAD
=======
using MaterialDesignThemes.Wpf;
>>>>>>> 35d339705e58d364ced46247947ab0da16200c0a
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
using MaterialDesignThemes.Wpf;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class LoginViewModel: BaseViewModel
    {
        public bool IsLoggedIn { get; set; } = false;
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { LoggedIn(p); });

        }

        //
        //PROCESSING FUNCTIONS
        //
        public void LoggedIn(Window p)
        {
            if (p == null)
                return;

            IsLoggedIn = true;
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
            p.Close();
        }
    }
}
