<<<<<<< HEAD
﻿using _4NH_HAO_Coffee_Shop;
using _4NH_HAO_Coffee_Shop.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using _4NH_HAO_Coffee_Shop.ViewModel;
using MaterialDesignThemes.Wpf;
=======
﻿using _4NH_HAO_Coffee_Shop.View;
using System;
using System.Windows;
using _4NH_HAO_Coffee_Shop.ViewModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using _4NH_HAO_Coffee_Shop.Utils;
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d

namespace _4NH_HAO_Coffee_Shop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
<<<<<<< HEAD
            /*
=======
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LiveCharts.Configure(config =>
            {
                config
                .AddSkiaSharp()
                .AddDefaultMappers()
                .AddLightTheme();
            });
        }
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
        protected void AppicationStart(object sender, EventArgs e)
        {
            var LoginView = new LoginView();
            LoginView.Show();
            LoginView.IsVisibleChanged += (s, ev) =>
            {
<<<<<<< HEAD
                if (Globals.CurrUser != null) 
=======
                if (Globals.Instance.CurrUser != null) 
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
                {
                    var MainView = new MainWindow();
                    LoginView.Close();
                    MainView.ShowDialog();
                }
            };
        }
<<<<<<< HEAD
            */
=======
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d

        
    }
}
