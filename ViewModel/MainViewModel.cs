using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using _4NH_HAO_Coffee_Shop.ViewModel;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;
        public ICommand ShowHomeViewCommand { get; set; }
        public ICommand ShowHistoryViewCommand { get; set; }
        public ICommand ShowOrderedViewCommand { get; set; }
        public ICommand ShowYourProfiledViewCommand { get; set; }
        public ICommand ShowSettingViewCommand { get; set; }
        

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }

        }

        public MainViewModel()
        {
            _currentView = new HomeViewModel();
            ShowHistoryViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new HistoryViewModel(); });
            ShowOrderedViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new OrderedViewModel(); });
            ShowHomeViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new HomeViewModel(); });
            ShowYourProfiledViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new YourProfileViewModel(); });
            ShowSettingViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new SettingViewModel(); });
            
        }
    }
}
