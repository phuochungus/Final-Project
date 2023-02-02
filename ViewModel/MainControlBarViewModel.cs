<<<<<<< HEAD
﻿using _4NH_HAO_Coffee_Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
=======
﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class MainControlBarViewModel : BaseViewModel
    {
<<<<<<< HEAD
        public ICommand ExitCommand { get; set; }

        public MainControlBarViewModel()
        {
            ExitCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
=======
        private static object _currentView;

        private System.Windows.Media.Brush _BGColor;
        public System.Windows.Media.Brush BGColor { get => _BGColor; set { _BGColor = value; OnPropertyChanged(); } }
        public ICommand ShowHomeViewCommand { get; set; }
        public ICommand ShowHistoryViewCommand { get; set; }
        public ICommand ShowOrderedViewCommand { get; set; }
        public ICommand ShowYourProfiledViewCommand { get; set; }
        public ICommand ShowSettingViewCommand { get; set; }
        public ICommand ShowDashBoardViewCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public ICommand LeftBarColorChange { get; set; }    

        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    if (value is HistoryViewModel)
                    {
                        HistoryViewModel.keepFetchingSearchResult = true;
                    }
                    else
                    {
                        HistoryViewModel.keepFetchingSearchResult = false;
                    }
                    _currentView = value;
                    OnPropertyChanged();
                }
            }

        }
        public MainControlBarViewModel()
        {
            BGColor = new SolidColorBrush(Colors.DarkOrange);

            _currentView = new HomeViewModel();
            ShowHistoryViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new HistoryViewModel(); });
            ShowOrderedViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new OrderedViewModel(); });
            ShowHomeViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new HomeViewModel(); });
            ShowSettingViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new SettingViewModel(); });
            ShowYourProfiledViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new YourProfileViewModel(); });
            ShowDashBoardViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new DashBoardViewModel(); });

            ExitCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
                FrameworkElement window = GetWindowParent(p);
                Window w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });
<<<<<<< HEAD
        }



        //
        //
        //

=======

            LeftBarColorChange = new RelayCommand<Haley.WPF.Controls.ColorPickerButton>((p) => { return true; }, (p) =>
            {
                BGColor = new SolidColorBrush(p.SelectedColor);
            });
        }

>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
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
