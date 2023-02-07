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
        private ObservableCollection<Promo> _List;
        public ObservableCollection<Promo> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private Promo _SelectedPromo;
        public Promo SelectedPromo
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
        public PromoListViewModel()
        {
            List = new ObservableCollection<Promo>(DataProvider.Ins.DB.Promoes);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Script) || string.IsNullOrEmpty(StartTime.ToString()) || string.IsNullOrEmpty(EndTime.ToString()))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Promoes.Where(x => (x.Id == Id && x.DisplayName == DisplayName));
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                var newPromo = new Promo() { Id = Id, DisplayName = DisplayName, Script = Script, StartTime = StartTime, EndTime = EndTime };
                DataProvider.Ins.DB.Promoes.Add(newPromo);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(newPromo);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Script) || string.IsNullOrEmpty(StartTime.ToString()) || string.IsNullOrEmpty(EndTime.ToString()))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Promoes.Where(x => (x.DisplayName == DisplayName && x.Id == Id && x.Script == Script && x.StartTime == StartTime && x.EndTime == EndTime));
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                var getPromo = DataProvider.Ins.DB.Promoes.Where(x => x.Id == SelectedPromo.Id).SingleOrDefault();
                getPromo.Id = Id;
                getPromo.DisplayName = DisplayName;
                getPromo.Script = Script;
                getPromo.StartTime = StartTime;
                getPromo.EndTime = EndTime;
                DataProvider.Ins.DB.SaveChanges();

                SelectedPromo = getPromo;
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Script) || string.IsNullOrEmpty(StartTime.ToString()) || string.IsNullOrEmpty(EndTime.ToString()))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Promoes.Where(x => (x.DisplayName == DisplayName && x.Id == Id && x.Script == Script && x.StartTime == StartTime && x.EndTime == EndTime));
                if (displayList == null || displayList.Count() == 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                DataProvider.Ins.DB.Promoes.Remove(SelectedPromo);
                DataProvider.Ins.DB.SaveChanges();
                List.Remove(SelectedPromo);
            });
        }
    }
}
