using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class ProductManagementViewModel : BaseViewModel
    {
        private int _currentListSignal;
        private object _currentList;
        public ICommand ShowItemListViewCommand { get; set; }
        public ICommand ShowPromoListViewCommand { get; set; }
        public ICommand ShowCategoryListViewCommand { get; set; }
        public ICommand ShowUnitListViewCommand { get; set; }
        public object CurrentList
        {
            get => _currentList;
            set { 
                _currentList = value;
                switch(CurrentListSignal)
                {
                    case 1:
                        AddCommand = ItemListViewModel.AddCommand;
                        EditCommand = ItemListViewModel.EditCommand;
                        DeleteCommand = ItemListViewModel.DeleteCommand;
                        break;
                    case 2:
                        AddCommand = PromoListViewModel.AddCommand;
                        EditCommand = PromoListViewModel.EditCommand;
                        DeleteCommand = PromoListViewModel.DeleteCommand;
                        break;
                    case 3:
                        AddCommand = CategoryListViewModel.AddCommand;
                        EditCommand = CategoryListViewModel.EditCommand;
                        DeleteCommand = CategoryListViewModel.DeleteCommand;
                        break;
                    case 4:
                        AddCommand = UnitListViewModel.AddCommand;
                        EditCommand = UnitListViewModel.EditCommand;
                        DeleteCommand = UnitListViewModel.DeleteCommand;
                        break;
                    default:
                        break;
                }
                OnPropertyChanged(); 
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public int CurrentListSignal { get => _currentListSignal; set => _currentListSignal = value; }

        public ProductManagementViewModel() 
        {
            CurrentListSignal = 1;
            CurrentList = new ItemListViewModel();
            ShowItemListViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
            {
                CurrentListSignal = 1;
                CurrentList = new ItemListViewModel();
            });
            ShowPromoListViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
            {
                CurrentListSignal = 2;
                CurrentList = new PromoListViewModel();
            });
            ShowCategoryListViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
            {
                CurrentListSignal = 3;
                CurrentList = new CategoryListViewModel();
            });
            ShowUnitListViewCommand = new RelayCommand<object>((p) => { return true; }, (p) => 
            {
                CurrentListSignal = 4;
                CurrentList = new UnitListViewModel();
            });
        }
    }
}
