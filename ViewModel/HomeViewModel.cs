using _4NH_HAO_Coffee_Shop.Model;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<Category> _CategoryList;
        public ObservableCollection<Category> CategoryList { get => _CategoryList; set { _CategoryList = value; OnPropertyChanged(); } }

        private Category _GetCategory;
        public Category GetCategory { get => _GetCategory; set { _GetCategory = value; OnPropertyChanged(); } }

        public ICommand CategoryChangeCommand { get; set; }




        public HomeViewModel()
        {
            CategoryList = new ObservableCollection<Category>(DataProvider.Ins.DB.Categories.ToList());
           
            

            CategoryChangeCommand = new RelayCommand<Button>((p) => { return true; }, (p) => {
                string gay = (string)p.Content;
                MessageBox.Show(gay);
            });

            
        }
    }
}
