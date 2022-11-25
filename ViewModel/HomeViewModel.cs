using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class HomeViewModel : BaseViewModel
    {
        public ICommand ShowHistoryViewCommand { get; set; }
        public ICommand ShowOrderedViewCommand { get; set; }
        public ICommand ShowYourProfileViewCommand { get; set; }
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel() 
        {
            CurrentView = new HistoryViewModel();
            ShowHistoryViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Console.WriteLine("his"); CurrentView = new HistoryViewModel(); });
            ShowOrderedViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Console.WriteLine("ord"); CurrentView = new OrderedViewModel(); });
            ShowYourProfileViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Console.WriteLine("ypr"); CurrentView = new YourProfileViewModel(); });
        }
    }
}
