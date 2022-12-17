using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class CategoryListViewModel : BaseViewModel
    {
        private ObservableCollection<Category> _List;
        public ObservableCollection<Category> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private Category _SelectedCategory;
        public Category SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                _SelectedCategory = value;
                OnPropertyChanged();
                if (SelectedCategory != null)
                {
                    Id = SelectedCategory.Id;
                    DisplayName = SelectedCategory.DisplayName;
                }
            }
        }

        private int _Id;
        public int Id
        {
            get => _Id;
            set
            {
                _Id = value;
                OnPropertyChanged();

            }
        }

        private string _DisplayName;
        public string DisplayName
        {
            get => _DisplayName;
            set
            {
                _DisplayName = value;
                OnPropertyChanged();
            }
        }
        public static ICommand AddCommand { get; set; }
        public static ICommand EditCommand { get; set; }
        public static ICommand DeleteCommand { get; set; }
        public CategoryListViewModel()
        {
            List = new ObservableCollection<Category>(DataProvider.Ins.DB.Categories);
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Categories.Where(x => (x.DisplayName == DisplayName));
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                var newCategory = new Category() { Id = Id, DisplayName = DisplayName};
                DataProvider.Ins.DB.Categories.Add(newCategory);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(newCategory);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Categories.Where(x => (x.DisplayName == DisplayName/* && x.Id == Id*/)); ;
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                var getCategory = DataProvider.Ins.DB.Categories.Where(x => x.DisplayName == SelectedCategory.DisplayName).SingleOrDefault();
                /*
                var itemList = DataProvider.Ins.DB.Items.Where(x => x.CategoryId == SelectedCategory.Id);
                if (itemList == null || itemList.Count() != 0)
                {
                    foreach (var item in itemList)
                    {
                        item.CategoryId = Id;
                    }
                }
                getCategory.Id = Id;
                */
                getCategory.DisplayName = DisplayName;
                DataProvider.Ins.DB.SaveChanges();

                SelectedCategory = getCategory;
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Categories.Where(x => (x.DisplayName == SelectedCategory.DisplayName));
                if (displayList == null || displayList.Count() == 0)
                {
                    return false;
                }
                var itemList = DataProvider.Ins.DB.Items.Where(x => (x.CategoryId == SelectedCategory.Id));
                if (itemList == null || itemList.Count() != 0)
                {
                    return false;
                }
                return true;

            }, (p) =>
            {
                DataProvider.Ins.DB.Categories.Remove(SelectedCategory);
                DataProvider.Ins.DB.SaveChanges();
                List.Remove(SelectedCategory);
            });
        }
    }
}
