using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private ObservableCollection<Category> ListDemo;
        private ObservableCollection<CategoryWrapper> _List;
        public ObservableCollection<CategoryWrapper> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private CategoryWrapper _SelectedCategory;
        public CategoryWrapper SelectedCategory
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
        private void CategoryToList()
        {
            ListDemo = new ObservableCollection<Category>(DataProvider.Ins.DB.Categories);
            List = new ObservableCollection<CategoryWrapper>();
            foreach (Category cate in ListDemo)
            {
                CategoryWrapper catew = new CategoryWrapper()
                {
                    category = new Category(),
                    Id = cate.Id,
                    DisplayName = cate.DisplayName,
                };
                List.Add(catew);
            }
        }
        public CategoryListViewModel()
        {
            CategoryToList();
            AddCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Category có hợp lệ ?
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                // Lấy List Category có Category trùng DisplayName với Category cần thêm 
                var displayList = DataProvider.Ins.DB.Categories.Where(x => (x.DisplayName == DisplayName));
                // Kiểm tra List Category có Category trùng DisplayName với Category cần thêm = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                // Tạo Category cần thêm
                CategoryWrapper newCategory = new CategoryWrapper() { category = new Category(), Id = Id, DisplayName = DisplayName };
                // Thêm newCategory vào DataBase
                DataProvider.Ins.DB.Categories.Add(newCategory.category);
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();
                // Thêm newCategory vào List Category
                List.Add(newCategory);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Category có hợp lệ ?
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                // Lấy List Category có Category trùng DisplayName với Category chỉnh sửa
                var displayList = DataProvider.Ins.DB.Categories.Where(x => (x.DisplayName == DisplayName)); ;
                // Kiểm tra List Category có Category trùng DisplayName với Category cần chỉnh sửa = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                // Lấy Category cần chỉnh sửa
                var getCategory = DataProvider.Ins.DB.Categories.Where(x => x.DisplayName == SelectedCategory.DisplayName).SingleOrDefault();
                // Thay đổi thông tin của category cần sửa
                getCategory.DisplayName = DisplayName;
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                SelectedCategory.DisplayName = getCategory.DisplayName;
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Category có hợp lệ ?
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                // Lấy List Category có Category trùng DisplayName với Category xóa
                var displayList = DataProvider.Ins.DB.Categories.Where(x => (x.DisplayName == SelectedCategory.DisplayName));
                // Kiểm tra List Categorys có Category trùng DisplayName với Item cần xóa = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() == 0)
                {
                    return false;
                }
                // Lấy List Item có CategoryId được sử dụng
                var itemList = DataProvider.Ins.DB.Items.Where(x => (x.CategoryId == SelectedCategory.Id));
                // Kiểm tra List Item vừa lấy = null hoặc không có phần tử hay không ?
                if (itemList == null || itemList.Count() != 0)
                {
                    return false;
                }
                return true;

            }, (p) =>
            {
                // Xóa category khỏi Database
                DataProvider.Ins.DB.Categories.Remove(SelectedCategory.category);
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();
                // Xóa category khỏi List Category
                List.Remove(SelectedCategory);
                // Đưa thông tin các textbox về null
                DisplayName = "";
            });
        }
    }
}
