﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using _4NH_HAO_Coffee_Shop.ViewModel;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

=======
using System.Windows;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private object _currentView;
        public ICommand ShowHomeViewCommand { get; set; }
        public ICommand ShowHistoryViewCommand { get; set; }
        public ICommand ShowOrderedViewCommand { get; set; }

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }

        }

        public MainViewModel()
        {
            _currentView = new HomeViewModel();
            ShowHistoryViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { CurrentView = new HistoryViewModel(); });
            ShowOrderedViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { CurrentView = new OrderedViewModel(); });
            ShowHomeViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { CurrentView = new HomeViewModel(); });
        }
>>>>>>> 8c423deabff0486af91476fcde10113b2732d8c9
    }
}
