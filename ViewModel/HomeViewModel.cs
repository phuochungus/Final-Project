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
        private ObservableCollection<Category> _categoryList;
        public ObservableCollection<Category> CategoryList { get => _categoryList; set { _categoryList = value; OnPropertyChanged(); } }

        private Category _getCategory;
        public Category GetCategory { get => _getCategory; set { _getCategory = value; OnPropertyChanged(); } }

        public ICommand CategoryChangeCommand { get; set; }

        private ObservableCollection<Item> _categorizedItemList;
        public ObservableCollection<Item> categorizedItemList { get => _categorizedItemList; set { _categorizedItemList= value; OnPropertyChanged(); } }

        private int _currentCategory;
        public int currentCategory { get => _currentCategory; 
            set { 
                if (value != currentCategory)
                {
                    _currentCategory = value;
                    OnPropertyChanged();
                }
            } }


       

        public HomeViewModel()
        {
            //
            int ChosenCategoryID = -1;// Determine which CategoryID is chosen to be shown
            //

            CategoryList = new ObservableCollection<Category>(DataProvider.Ins.DB.Categories.ToList());
            
            CategoryChangeCommand = new RelayCommand<object>((p) => { 
                return true;
            }, (p) => {
                ChosenCategoryID = (int)p;
                categorizedItemList = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.Where(Cond => Cond.CategoryId == ChosenCategoryID).ToList());
            });




            
        }
    }
}
