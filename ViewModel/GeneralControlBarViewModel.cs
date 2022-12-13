﻿
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public  class GeneralControlBarViewModel : BaseViewModel
    {
        public ICommand DragMoveWindowCommand { get; set; }
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
