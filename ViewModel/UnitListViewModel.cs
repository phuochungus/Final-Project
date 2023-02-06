using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class UnitListViewModel : BaseViewModel
    {
        private ObservableCollection<Unit> ListDemo;
        private ObservableCollection<UnitWrapper> _List;
        public ObservableCollection<UnitWrapper> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private UnitWrapper _SelectedUnit;
        public UnitWrapper SelectedUnit
        {
            get => _SelectedUnit;
            set
            {
                _SelectedUnit = value;
                OnPropertyChanged();
                if (SelectedUnit != null)
                {
                    Id = SelectedUnit.Id;
                    DisplayName = SelectedUnit.DisplayName;
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
        private void UnitToList()
        {
            ListDemo = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);
            List = new ObservableCollection<UnitWrapper>();
            foreach (Unit uni in ListDemo)
            {
                UnitWrapper uniw = new UnitWrapper()
                {
                    unit = new Unit(),
                    Id = uni.Id,
                    DisplayName = uni.DisplayName,
                };
                List.Add(uniw);
            }
        }
        public UnitListViewModel()
        {
            UnitToList();
            AddCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Unit có hợp lệ ?
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                // Lấy List Unit có Unit trùng DisplayName với Unit cần thêm 
                var displayList = DataProvider.Ins.DB.Units.Where(x => (x.DisplayName == DisplayName));
                // Kiểm tra List Unit có Unit trùng DisplayName với Unit cần thêm = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                // Tạo Unit cần thêm
                UnitWrapper newUnit = new UnitWrapper() { unit = new Unit(), Id = Id, DisplayName = DisplayName };
                // Thêm newUnitvào DataBase
                DataProvider.Ins.DB.Units.Add(newUnit.unit);
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();
                // Thêm newUnitD vào List Unit
                List.Add(newUnit);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Unit có hợp lệ ?
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                // Lấy List Unit có Unit trùng DisplayName với Unit chỉnh sửa
                var displayList = DataProvider.Ins.DB.Units.Where(x => (x.DisplayName == DisplayName));
                // Kiểm tra List Unit có Unit trùng DisplayName với Unit cần chỉnh sửa = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                // Lấy Unit cần chỉnh sửa
                var getUnit = DataProvider.Ins.DB.Units.Where(x => x.DisplayName == SelectedUnit.DisplayName).SingleOrDefault();
                // Thay đổi thông tin của Unit cần sửa
                getUnit.DisplayName = DisplayName;
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                SelectedUnit.DisplayName = getUnit.DisplayName;
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Unit có hợp lệ ?
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                // Lấy List Units có Unit trùng DisplayName với Unit xóa
                var displayList = DataProvider.Ins.DB.Units.Where(x => (x.DisplayName == SelectedUnit.DisplayName));
                // Kiểm tra List Units có Unit trùng DisplayName với Unit cần xóa = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() == 0)
                {
                    return false;
                }
                // Lấy List Items có UnitId được sử dụng
                var itemList = DataProvider.Ins.DB.Items.Where(x => (x.UnitId == SelectedUnit.Id));
                // Kiểm tra List Unit vừa lấy = null hoặc không có phần tử hay không ?
                if (itemList == null || itemList.Count() != 0)
                {
                    return false;
                }
                return true;

            }, (p) =>
            {
                // Xóa Unit khỏi Database
                DataProvider.Ins.DB.Units.Remove(SelectedUnit.unit);
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();
                // Xóa Unit khỏi List Unit
                List.Remove(SelectedUnit);
                // Đưa thông tin các textbox về null
                DisplayName = "";
            });
        }
    }
}
