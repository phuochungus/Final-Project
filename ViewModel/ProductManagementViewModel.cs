using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class ProductManagementViewModel : BaseViewModel
    {
        private object _currentList;
        public ICommand ShowItemListViewCommand { get; set; }
        public ICommand ShowPromoListViewCommand { get; set; }
        public ICommand ShowCategoryListViewCommand { get; set; }
        public ICommand ShowUnitListViewCommand { get; set; }
        public object CurrentList
        {
            get => _currentList;
            set { _currentList = value; OnPropertyChanged(); }
        }

        public ProductManagementViewModel() 
        {
            _currentList = new ItemListViewModel();
            ShowItemListViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentList = new ItemListViewModel(); });
            ShowPromoListViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentList = new PromoListViewModel(); });
            ShowCategoryListViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentList = new CategoryListViewModel(); });
            ShowUnitListViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CurrentList = new UnitListViewModel(); });
        }
    }
}
