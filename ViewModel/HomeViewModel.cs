using _4NH_HAO_Coffee_Shop.Model;
using Caliburn.Micro;
using Microsoft.Expression.Interactivity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<Category> _categoryList;
        public ObservableCollection<Category> CategoryList { get => _categoryList; set { _categoryList = value; OnPropertyChanged(); } }
        private Category _getCategory;
        public Category GetCategory { get => _getCategory; set { _getCategory = value; OnPropertyChanged(); } }
        private ObservableCollection<Item> _categorizedItemList;
        public ObservableCollection<Item> categorizedItemList { get => _categorizedItemList; set { _categorizedItemList = value; OnPropertyChanged(); } }
        private int totalPrice = 0;
        public int TotalPrice
        {
            get => totalPrice;
            set
            {
                if (totalPrice != value)
                {
                    totalPrice = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _currentCategory;
        public int currentCategory
        {
            get => _currentCategory;
            set
            {
                if (value != _currentCategory)
                {
                    _currentCategory = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand CategoryChangeCommand { get; set; }
        public ICommand AddToBillCommand { get; set; }
        public ICommand DecreaseQuantityCommand { get; set; }

        private void handleCheckoutCommand(object p)
        {
            Globals.Instance.OrderQueue.Add(Globals.Instance.CurrBill);
            Globals.Instance.CurrBill.Clear();
        }
        public ICommand CheckoutCommand { get; set; }

        public ICommand IncreaseQuantityCommand { get; set; }
        public ICommand ViewAll { get; set; }

        public HomeViewModel()
        {

            //
            int ChosenCategoryID = -1;// Determine which CategoryID is chosen to be shown

            //



            //
            CheckoutCommand = new RelayCommand<object>(p => !Globals.Instance.CurrBill.isEmpty(), p => handleCheckoutCommand(p));
            ViewAll = new RelayCommand<object>(p => true, p => categorizedItemList = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.ToList()));
            AddToBillCommand = new RelayCommand<object>((p) => true, (p) => Globals.Instance.Insert(p as Item));
            DecreaseQuantityCommand = new RelayCommand<object>((p) => true, p => Globals.Instance.Delete((p as Product).Key));
            IncreaseQuantityCommand = new RelayCommand<object>((p) => true, p => Globals.Instance.Insert((p as Product).Key));
            CategoryList = new ObservableCollection<Category>(DataProvider.Ins.DB.Categories.ToList());
            categorizedItemList = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.Where(cond => cond.Category.DisplayName == "Drink").ToList());
            CategoryChangeCommand = new RelayCommand<Category>((p) => true, (p) =>
            {
                ChosenCategoryID = p.Id;
                categorizedItemList = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.Where(Cond => Cond.CategoryId == ChosenCategoryID).ToList());
            });

        }
    }
}
