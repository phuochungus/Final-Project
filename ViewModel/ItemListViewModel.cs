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
using _4NH_HAO_Coffee_Shop.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Item = _4NH_HAO_Coffee_Shop.Model.Item;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class ItemListViewModel : BaseViewModel
    {
        private ObservableCollection<Item> ListDemo;
        private ObservableCollection<ItemWrapper> _List;
        public ObservableCollection<ItemWrapper> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private ItemWrapper _SelectedItem;
        public ItemWrapper SelectedItem  
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
        private void ItemToList()
        {
            ListDemo = new ObservableCollection<Item>(DataProvider.Ins.DB.Items);
            List = new ObservableCollection<ItemWrapper>();
            foreach (Item it in ListDemo)
            {
                ItemWrapper itw = new ItemWrapper()
                {
                    item = new Item(),
                    Id = it.Id,
                    DisplayName = it.DisplayName,
                    UnitId = it.UnitId,
                    CategoryId = it.CategoryId,
                    Price = it.Price,
                    ImageURL = it.ImageURL,
                };
                List.Add(itw);
            }
        }

        public ItemListViewModel()
        {
            ItemToList();

            AddCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Item có hợp lệ ?
                if (string.IsNullOrEmpty(Id)
                    || string.IsNullOrEmpty(DisplayName)
                    || UnitId <= 0
                    || string.IsNullOrEmpty(UnitId.ToString())
                    || CategoryId <= 0
                    || string.IsNullOrEmpty(CategoryId.ToString())
                    || string.IsNullOrEmpty(ImageURL))
                {
                    return false;
                }
                // Lấy List Items có Item trùng Id với Item cần thêm 
                var displayList = DataProvider.Ins.DB.Items.Where(x => (x.Id == Id || x.DisplayName == DisplayName));
                // Kiểm tra List Items có Item trùng Id với Item cần thêm = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }
                //  Lấy List Categories có Category trùng Id với CategoryId của Item cần thêm
                var categoryList = DataProvider.Ins.DB.Categories.Where(x => (x.Id == CategoryId));
                // Kiểm tra List Categories có Category trùng Id với CategoryId của Item cần thêm = null hoặc không có phần tử hay không ?
                if (categoryList == null || categoryList.Count() == 0)
                {
                    return false;
                }
                //  Lấy List Units có Unit trùng Id với UnitId của Item cần thêm
                var unitList = DataProvider.Ins.DB.Units.Where(x => (x.Id == UnitId));
                // Kiểm tra List Units có Unit trùng Id với UnitId của Item cần thêm = null hoặc không có phần tử hay không ?
                if (unitList == null || unitList.Count() == 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                // Tạo Item cần thêm
                ItemWrapper newItem = new ItemWrapper() { item = new Item(), Id = Id, DisplayName = DisplayName, UnitId = UnitId, CategoryId = CategoryId, Price = Price, ImageURL = ImageURL };
                // Thêm newItem vào DataBase
                DataProvider.Ins.DB.Items.Add(newItem.item);
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();
                // Thêm newItem vào List Item
                List.Add(newItem);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Item có hợp lệ ?
                if (SelectedItem == null
                    || string.IsNullOrEmpty(DisplayName)
                    || string.IsNullOrEmpty(Id.ToString())
                    || string.IsNullOrEmpty(UnitId.ToString())
                    || string.IsNullOrEmpty(CategoryId.ToString())
                    || string.IsNullOrEmpty(ImageURL))
                {
                    return false;
                }
                // Lấy List Items có Item trùng Id với Item cần chỉnh sửa 
                var displayList = DataProvider.Ins.DB.Items.Where(x => (x.DisplayName == DisplayName
                                                                        && x.Id == Id
                                                                        && x.UnitId == UnitId
                                                                        && x.CategoryId == CategoryId
                                                                        && x.Price == Price
                                                                        && x.ImageURL == ImageURL));
                // Kiểm tra List Items có Item trùng Id với Item cần chỉnh sửa = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }
                //  Lấy List Categories có Category trùng Id với CategoryId của Item cần chỉnh sửa
                var categoryList = DataProvider.Ins.DB.Categories.Where(x => (x.Id == CategoryId));
                // Kiểm tra List Categories có Category trùng Id với CategoryId của Item cần chỉnh sửa = null hoặc không có phần tử hay không ?
                if (categoryList == null || categoryList.Count() == 0)
                {
                    return false;
                }
                //  Lấy List Units có Unit trùng Id với UnitId của Item cần chỉnh sửa
                var unitList = DataProvider.Ins.DB.Units.Where(x => (x.Id == UnitId));
                // Kiểm tra List Units có Unit trùng Id với UnitId của Item cần chỉnh sửa = null hoặc không có phần tử hay không ?
                if (unitList == null || unitList.Count() == 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                // Lấy Item cần chỉnh sửa
                var getItem = DataProvider.Ins.DB.Items.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                // Thay đổi thông tin của item cần sửa
                getItem.Id = Id;
                getItem.DisplayName = DisplayName;
                getItem.UnitId = UnitId;
                getItem.Price = Price;
                getItem.CategoryId = CategoryId;
                getItem.ImageURL = ImageURL;
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem.Id = getItem.Id;
                SelectedItem.DisplayName = getItem.DisplayName;
                SelectedItem.UnitId = getItem.UnitId;
                SelectedItem.CategoryId = getItem.CategoryId;
                SelectedItem.Price = getItem.Price;
                SelectedItem.ImageURL = getItem.ImageURL;
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Item có hợp lệ ?
                if (string.IsNullOrEmpty(Id)
                    || string.IsNullOrEmpty(DisplayName)
                    || UnitId <= 0
                    || string.IsNullOrEmpty(UnitId.ToString())
                    || CategoryId <= 0
                    || string.IsNullOrEmpty(CategoryId.ToString())
                    || string.IsNullOrEmpty(ImageURL))
                {
                    return false;
                }
                // Lấy List Items có Item trùng Id với Item cần xóa
                var displayList = DataProvider.Ins.DB.Items.Where(x => (x.Id == Id || x.DisplayName == DisplayName));
                // Kiểm tra List Items có Item trùng Id với Item cần xóa = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() == 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                // Xóa item khỏi Database
                DataProvider.Ins.DB.Items.Remove(SelectedItem.item);
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();
                // Xóa item khỏi List Item
                List.Remove(SelectedItem);
                // Đưa thông tin các textbox về null
                Id = DisplayName = ImageURL = "";
                UnitId = CategoryId = Price = 0;
            });
        }
    }
}
