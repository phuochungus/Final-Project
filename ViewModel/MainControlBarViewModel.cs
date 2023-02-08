using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        public ICommand ShowProductManagementViewCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public ICommand LeftBarColorChange { get; set; }



        private System.Windows.Media.Brush _BGColor;
        public System.Windows.Media.Brush BGColor { get => _BGColor; set { _BGColor = value; OnPropertyChanged(); } }
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
            ShowDashBoardViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentView = new DashBoardViewModel(); });
            ShowProductManagementViewCommand = new RelayCommand<object>((p) => true, (p) => { CurrentView = new ProductManagementViewModel(); });

            ExitCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                FrameworkElement window = GetWindowParent(p);
                Window w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });


            BGColor = new SolidColorBrush(Colors.DarkOrange);
            LeftBarColorChange = new RelayCommand<Haley.WPF.Controls.ColorPickerButton>((p) => { return true; }, (p) =>
            {
                BGColor = new SolidColorBrush(p.SelectedColor);
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
