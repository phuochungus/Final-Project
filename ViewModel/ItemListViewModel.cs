using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class ItemListViewModel : BaseViewModel
    {
        private ObservableCollection<Item> _List;
        public ObservableCollection<Item> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private Item _SelectedItem;
        public Item SelectedItem  
        { 
            get => _SelectedItem; 
            set 
            { 
                _SelectedItem = value; 
                OnPropertyChanged(); 
                if (SelectedItem != null)
                {
                    Id = SelectedItem.Id;
                    DisplayName = SelectedItem.DisplayName;
                    UnitId = SelectedItem.UnitId;
                    CategoryId = SelectedItem.CategoryId;
                    Price = SelectedItem.Price;
                    ImageURL = SelectedItem.ImageURL;
                }
            } 
        }

        private string _Id;
        public string Id
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

        private int _UnitId;
        public int UnitId
        {
            get => _UnitId;
            set
            {
                _UnitId = value;
                OnPropertyChanged();

            }
        }

        private int _CategoryId;
        public int CategoryId
        {
            get => _CategoryId;
            set
            {
                _CategoryId = value;
                OnPropertyChanged();

            }
        }

        private int _Price;
        public int Price
        {
            get => _Price;
            set
            {
                _Price = value;
                OnPropertyChanged();

            }
        }

        private string _ImageURL;
        public string ImageURL
        {
            get => _ImageURL;
            set
            {
                _ImageURL = value;
                OnPropertyChanged();
            }
        }
        public static ICommand AddCommand { get; set; }
        public static ICommand EditCommand { get; set; }
        public static ICommand DeleteCommand { get; set; }

        public ItemListViewModel()
        {
            List = new ObservableCollection<Item>(DataProvider.Ins.DB.Items);
            
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(DisplayName) || UnitId <= 0 || string.IsNullOrEmpty(UnitId.ToString()) || CategoryId <= 0 || string.IsNullOrEmpty(CategoryId.ToString()) || string.IsNullOrEmpty(ImageURL)) 
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Items.Where(x => (x.Id == Id || x.DisplayName == DisplayName));
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }
                var categoryList = DataProvider.Ins.DB.Categories.Where(x => (x.Id == CategoryId));
                if (categoryList == null || categoryList.Count() == 0)
                {
                    return false;
                }
                var unitList = DataProvider.Ins.DB.Units.Where(x => (x.Id == UnitId));
                if (unitList == null || unitList.Count() == 0)
                {
                    return false;
                }

                return true;

            }, (p) => 
            {
                var newItem = new Item() { Id = Id, DisplayName = DisplayName, UnitId = UnitId, CategoryId = CategoryId, Price = Price, ImageURL = ImageURL };
                DataProvider.Ins.DB.Items.Add(newItem);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(newItem);
            });
            
            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Id.ToString()) || string.IsNullOrEmpty(UnitId.ToString()) || string.IsNullOrEmpty(CategoryId.ToString()) || string.IsNullOrEmpty(ImageURL))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Items.Where(x => (x.DisplayName == DisplayName && x.Id == Id && x.UnitId == UnitId && x.CategoryId == CategoryId && x.Price == Price && x.ImageURL == ImageURL));
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }
                var categoryList = DataProvider.Ins.DB.Categories.Where(x => (x.Id == CategoryId));
                if (categoryList == null || categoryList.Count() == 0)
                {
                    return false;
                }
                var unitList = DataProvider.Ins.DB.Units.Where(x => (x.Id == UnitId));
                if (unitList == null || unitList.Count() == 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                var getItem = DataProvider.Ins.DB.Items.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                getItem.Id = Id; 
                getItem.DisplayName = DisplayName;
                getItem.UnitId = UnitId;
                getItem.Price = Price;
                getItem.CategoryId = CategoryId;
                getItem.ImageURL = ImageURL;    

                DataProvider.Ins.DB.SaveChanges();

                SelectedItem = getItem;
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(DisplayName) || UnitId <= 0 || string.IsNullOrEmpty(UnitId.ToString()) || CategoryId <= 0 || string.IsNullOrEmpty(CategoryId.ToString()) || string.IsNullOrEmpty(ImageURL))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Items.Where(x => (x.Id == Id || x.DisplayName == DisplayName));
                if (displayList == null || displayList.Count() == 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                DataProvider.Ins.DB.Items.Remove(SelectedItem);
                DataProvider.Ins.DB.SaveChanges();
                List.Remove(SelectedItem);
            });
        }
    }
}
