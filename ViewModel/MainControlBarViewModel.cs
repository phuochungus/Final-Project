using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class MainControlBarViewModel : BaseViewModel
    {
        private static object _currentView;
        public ICommand ShowHomeViewCommand { get; set; }
        public ICommand ShowHistoryViewCommand { get; set; }
        public ICommand ShowOrderedViewCommand { get; set; }
        public ICommand ShowYourProfiledViewCommand { get; set; }
        public ICommand ShowSettingViewCommand { get; set; }
        public ICommand ShowDashBoardViewCommand { get; set; }
        public ICommand ExitCommand { get; set; }

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

            _currentView = new HomeViewModel();
            ShowHistoryViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new HistoryViewModel(); });
            ShowOrderedViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new OrderedViewModel(); });
            ShowHomeViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new HomeViewModel(); });
            ShowSettingViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new SettingViewModel(); });
            ShowYourProfiledViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new YourProfileViewModel(); });
            ShowDashBoardViewCommand = new RelayCommand<object>(p => true, p => { CurrentView = new DashBoardViewModel(); });

            ExitCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                FrameworkElement window = GetWindowParent(p);
                Window w = window as Window;
                if (w != null)
                {
                    w.Close();
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
