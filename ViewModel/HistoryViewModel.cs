using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class HistoryViewModel : BaseViewModel
    {
        private string _controlsEnabled;
        public string ControlsEnabled
        {
            get { return _controlsEnabled; }
            set
            {
                _controlsEnabled = value;
                OnPropertyChanged();
            }
        }
        public HistoryViewModel()
        {
            ControlsEnabled = "False";
            
        }
    }
}
