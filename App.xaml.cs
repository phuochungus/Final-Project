using _4NH_HAO_Coffee_Shop.View;
using System;
using System.Windows;
using _4NH_HAO_Coffee_Shop.ViewModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace _4NH_HAO_Coffee_Shop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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
