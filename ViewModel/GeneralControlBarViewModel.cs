
using MaterialDesignThemes.Wpf;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class GeneralControlBarViewModel : BaseViewModel
    {
        public ICommand DragMoveWindowCommand { get; set; }
        public ICommand ResizeWindowCommand { get; set; }
        public ICommand ChangeControlBarColor { get; set; }


        private System.Windows.Media.Brush _BGColor;
        public System.Windows.Media.Brush BGColor { get => _BGColor; set { _BGColor = value; OnPropertyChanged(); } }

        public GeneralControlBarViewModel()
        {
            DragMoveWindowCommand = new RelayCommand<UserControl>((p) =>
            {
                return true;
            }, (p) =>
            {
                FrameworkElement window = GetWindowParent(p);
                Window w = window as Window;
                if (w != null)
                {
                    w.DragMove();

                }
            });

            ResizeWindowCommand = new RelayCommand<UserControl>((p) =>
            {
                return true;
            }, (p) =>
            {
                FrameworkElement window = GetWindowParent(p);
                Window w = window as Window;
                if (w != null)
                {
                    if (w.WindowState != WindowState.Maximized)
                        w.WindowState = WindowState.Maximized;
                    else if (w.WindowState != WindowState.Normal)
                        w.WindowState = WindowState.Normal;
                }
            });

            ChangeControlBarColor = new RelayCommand<ColorPicker>((p) =>
            {
                return true;
            }, (p) =>   
            {
                BaseViewModel us = new SettingViewModel();
                
                MessageBox.Show(p.Color.ToString());
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
