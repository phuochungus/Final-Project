using _4NH_HAO_Coffee_Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class MainControlBarViewModel : BaseViewModel
    {
        public ICommand ExitCommand { get; set; }

        public MainControlBarViewModel()
        {
            ExitCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { });
        }
    }
}
