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
using System.Windows.Media.Imaging;
using static System.Net.WebRequestMethods;
using System.Windows.Media;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public struct BillCard
    {
        string displayName;
        int totalPrice;
    }
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<Category> _categoryList;
        public ObservableCollection<Category> CategoryList { get => _categoryList; set { _categoryList = value; OnPropertyChanged(); } }

        private Category _getCategory;
        public Category GetCategory { get => _getCategory; set { _getCategory = value; OnPropertyChanged(); } }



        public ICommand CategoryChangeCommand { get; set; }
        public ICommand AddToBillCommand { get; set; }


        private ObservableCollection<Item> _categorizedItemList;
        public ObservableCollection<Item> categorizedItemList { get => _categorizedItemList; set { _categorizedItemList = value; OnPropertyChanged(); } }


       



        private int _currentCategory;
        public int currentCategory
        {
            get => _currentCategory;
            set
            {
                if (value != currentCategory)
                {
                    _currentCategory = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddToBillCommand { get; set; }
        public ICommand UpdateQuantityCommand { get; set; }
        public ICommand DecreaseQuantityCommand { get; set; }
        public ICommand IncreaseQuantityCommand { get; set; }


        public HomeViewModel()
        {

            //
            int ChosenCategoryID = -1;// Determine which CategoryID is chosen to be shown
                                      //

            //
            categorizedItemList = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.Where(Cond => Cond.DisplayName=="Cafe đen").ToList());

            //
            AddToBillCommand = new RelayCommand<object>((p) => true, (p) => { Globals.Insert(p as Item); });
            UpdateQuantityCommand = new RelayCommand<object>(p => true, p =>
            {
                var values = (object[])p;
                var item = values[0] as Item;
                var quantity = (int)values[1];
                Globals.Update(item, quantity);    
            });
            DecreaseQuantityCommand = new RelayCommand<object>((p) => true, p =>
            {
                var values = (object[])p;
                var item = values[0] as Item;
                var quantity = (int)values[1];
                Globals.Delete(item);
            });
            IncreaseQuantityCommand = new RelayCommand<object>((p) => true, p =>
            {
                var values = (object[])p;
                var item = values[0] as Item;
                var quantity = (int)values[1];
                Globals.Insert(item);
            });
            CategoryList = new ObservableCollection<Category>(DataProvider.Ins.DB.Categories.ToList());

            CategoryChangeCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChosenCategoryID = (int)p;
                categorizedItemList = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.Where(Cond => Cond.CategoryId == ChosenCategoryID).ToList());
            });

        }
    }
}
