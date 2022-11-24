using _4NH_HAO_Coffee_Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class MainControlBarViewModel : BaseViewModel
    {
        public ICommand ExitCommand { get; set; }

        public MainControlBarViewModel()
        {
            ExitCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = GetWindowParent(p);
                Window w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });
        }



        //
        //
        //

        public FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        }
    }
}
