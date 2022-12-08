using _4NH_HAO_Coffee_Shop;
using _4NH_HAO_Coffee_Shop.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using _4NH_HAO_Coffee_Shop.ViewModel;

namespace _4NH_HAO_Coffee_Shop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void AppicationStart(object sender, EventArgs e)
        {
            var LoginView = new LoginView();
            LoginView.Show();
            LoginView.IsVisibleChanged += (s, ev) =>
            {
                if (Globals.Instance.CurrUser != null) 
                {
                    var MainView = new MainWindow();
                    LoginView.Close();
                    MainView.ShowDialog();
                }
            };
        }

        
    }
}
