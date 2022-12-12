using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public  class GeneralControlBarViewModel : BaseViewModel
    {
        public ICommand DragMoveWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public GeneralControlBarViewModel() 
        {
            DragMoveWindowCommand = new RelayCommand<UserControl>((p) => {
                return true;
            }, (p) => {
                FrameworkElement window = GetWindowParent(p);
                Window w = window as Window;
                if (w != null)
                {
                    w.DragMove();
                }
            });

            MaximizeWindowCommand = new RelayCommand<UserControl>((p) => {
                return true;
            }, (p) => {
                FrameworkElement window = GetWindowParent(p);
                Window w = window as Window;
                if (w != null)
                {
                    if (w.WindowState != WindowState.Maximized)
                        w.WindowState = WindowState.Maximized;
                    else if(w.WindowState != WindowState.Normal)
                        w.WindowState = WindowState.Normal;
                }
            });

            
        }


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
