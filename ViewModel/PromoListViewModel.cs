using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class PromoListViewModel : BaseViewModel
    {
        private ObservableCollection<Promo> ListDemo;
        private ObservableCollection<PromoWrapper> _List;

        public ObservableCollection<PromoWrapper> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private PromoWrapper _SelectedPromo;
        public PromoWrapper SelectedPromo
        {
            get => _SelectedPromo;
            set
            {
                _SelectedPromo = value;
                OnPropertyChanged();
                if (SelectedPromo != null)
                {
                    Id = SelectedPromo.Id;
                    DisplayName = SelectedPromo.DisplayName;
                    Script = SelectedPromo.Script;
                    StartTime = SelectedPromo.StartTime;
                    EndTime = SelectedPromo.EndTime;
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

        private string _Script;
        public string Script
        {
            get => _Script;
            set
            {
                _Script = value;
                OnPropertyChanged();

            }
        }

        private Nullable<System.DateTime> _StartTime;
        public Nullable<System.DateTime> StartTime
        {
            get => _StartTime;
            set
            {
                _StartTime = value;
                OnPropertyChanged();

            }
        }

        private Nullable<System.DateTime> _EndTime;
        public Nullable<System.DateTime> EndTime
        {
            get => _EndTime;
            set
            {
                _EndTime = value;
                OnPropertyChanged();

            }
        }
        public static ICommand AddCommand { get; set; }
        public static ICommand EditCommand { get; set; }
        public static ICommand DeleteCommand { get; set; }
        private void PromoToList()
        {
            ListDemo = new ObservableCollection<Promo>(DataProvider.Ins.DB.Promoes);
            List = new ObservableCollection<PromoWrapper>();
            foreach (Promo prom in ListDemo)
            {
                PromoWrapper promw = new PromoWrapper()
                {
                    promo = new Promo(),
                    Id = prom.Id,
                    DisplayName = prom.DisplayName,
                    Script = prom.Script,
                    StartTime = prom.StartTime,
                    EndTime = prom.EndTime,
                };
                List.Add(promw);
            }
        }
        public PromoListViewModel()
        {
            PromoToList();

            AddCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Promo có hợp lệ ?
                if (string.IsNullOrEmpty(Id)
                    || string.IsNullOrEmpty(DisplayName)
                    || string.IsNullOrEmpty(Script)
                    || string.IsNullOrEmpty(StartTime.ToString())
                    || string.IsNullOrEmpty(EndTime.ToString()))
                {
                    return false;
                }
                // Lấy List Promos có Promo trùng Id với Promo cần thêm
                var displayList = DataProvider.Ins.DB.Promoes.Where(x => (x.Id == Id && x.DisplayName == DisplayName));
                // Kiểm tra List Promos có Promo trùng Id với Promo cần thêm = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                // Tạo promo cần thêm
                PromoWrapper newPromo = new PromoWrapper()
                {
                    promo = new Promo(),
                    Id = Id,
                    DisplayName = DisplayName,
                    Script = Script,
                    StartTime = StartTime,
                    EndTime = EndTime
                };
                // Thêm newPromo vào DataBase
                DataProvider.Ins.DB.Promoes.Add(newPromo.promo);
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();
                // Thêm newPromo vào List Promo
                List.Add(newPromo);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Promo có hợp lệ ?
                if (string.IsNullOrEmpty(Id)
                    || string.IsNullOrEmpty(DisplayName)
                    || string.IsNullOrEmpty(Script)
                    || string.IsNullOrEmpty(StartTime.ToString())
                    || string.IsNullOrEmpty(EndTime.ToString()))
                {
                    return false;
                }
                // Lấy List Promo có promo trùng Id với promo cần chỉnh sửa 

                var displayList = DataProvider.Ins.DB.Promoes.Where(x => (x.DisplayName == DisplayName
                                                                        && x.Id == Id
                                                                        && x.Script == Script
                                                                        && x.StartTime == StartTime
                                                                        && x.EndTime == EndTime));
                // Kiểm tra List Promos có promo trùng Id với promo cần chỉnh sửa = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                // Lấy promo cần chỉnh sửa
                var getPromo = DataProvider.Ins.DB.Promoes.Where(x => x.Id == SelectedPromo.Id).SingleOrDefault();
                // Thay đổi thông tin của promo cần sửa
                getPromo.Id = Id;
                getPromo.DisplayName = DisplayName;
                getPromo.Script = Script;
                getPromo.StartTime = StartTime;
                getPromo.EndTime = EndTime;
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();

                SelectedPromo.Id = getPromo.Id;
                SelectedPromo.Script = getPromo.Script;
                SelectedPromo.DisplayName = getPromo.DisplayName;
                SelectedPromo.StartTime = getPromo.StartTime;
                SelectedPromo.EndTime = getPromo.EndTime;
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                // Kiểm tra các TextBox hiển thị thông tin Promo có hợp lệ ?
                if (string.IsNullOrEmpty(Id)
                    || string.IsNullOrEmpty(DisplayName)
                    || string.IsNullOrEmpty(Script)
                    || string.IsNullOrEmpty(StartTime.ToString())
                    || string.IsNullOrEmpty(EndTime.ToString()))
                {
                    return false;
                }
                // Lấy List Promos có Promo trùng thông tin với Promo cần xóa
                var displayList = DataProvider.Ins.DB.Promoes.Where(x => (x.DisplayName == DisplayName
                                                                            && x.Id == Id
                                                                            && x.Script == Script
                                                                            && x.StartTime == StartTime
                                                                            && x.EndTime == EndTime));
                // Kiểm tra List Promos có Promo trùng thông tin với Promo cần xóa = null hoặc không có phần tử hay không ?
                if (displayList == null || displayList.Count() == 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                // Xóa item khỏi Database
                DataProvider.Ins.DB.Promoes.Remove(SelectedPromo.promo);
                // Lưu thay đổi
                DataProvider.Ins.DB.SaveChanges();
                // Xóa promo khỏi List Promos
                List.Remove(SelectedPromo);
                // Đưa thông tin các textbox về null
                Id = DisplayName = Script = "";
                StartTime = EndTime = null;
            });
        }
    }
}
