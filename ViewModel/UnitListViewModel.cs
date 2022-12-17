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
        private ObservableCollection<Unit> _List;
        public ObservableCollection<Unit> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private Unit _SelectedUnit;
        public Unit SelectedUnit
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
        public UnitListViewModel()
        {
            List = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Units.Where(x => (x.DisplayName == DisplayName));
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;

            }, (p) =>
            {
                var newUnit = new Unit() { Id = Id, DisplayName = DisplayName };
                DataProvider.Ins.DB.Units.Add(newUnit);
                DataProvider.Ins.DB.SaveChanges();

                List.Add(newUnit);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Units.Where(x => (x.DisplayName == DisplayName));
                if (displayList == null || displayList.Count() != 0)
                {
                    return false;
                }

                return true;
            }, (p) =>
            {
                var getUnit = DataProvider.Ins.DB.Units.Where(x => x.DisplayName == SelectedUnit.DisplayName).SingleOrDefault();
                getUnit.DisplayName = DisplayName;
                DataProvider.Ins.DB.SaveChanges();

                SelectedUnit = getUnit;
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName))
                {
                    return false;
                }
                var displayList = DataProvider.Ins.DB.Units.Where(x => (x.DisplayName == SelectedUnit.DisplayName));
                if (displayList == null || displayList.Count() == 0)
                {
                    return false;
                }
                var itemList = DataProvider.Ins.DB.Items.Where(x => (x.UnitId == SelectedUnit.Id));
                if (itemList == null || itemList.Count() != 0)
                {
                    return false;
                }
                return true;

            }, (p) =>
            {
                DataProvider.Ins.DB.Units.Remove(SelectedUnit);
                DataProvider.Ins.DB.SaveChanges();
                List.Remove(SelectedUnit);
            });
        }
    }
}
