using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    internal class HistoryViewModel : BaseViewModel
    {
        private ObservableCollection<Bill> _historyList;
        public ObservableCollection<Bill> HistoryList
        {
            get => _historyList;
            set
            {
                if (_historyList != value)
                {
                    _historyList = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _controlsEnabled;
        public string ControlsEnabled
        {
            get { return _controlsEnabled; }
            set
            {
                _controlsEnabled = value;
                OnPropertyChanged();
            }
        }
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Today.AddDays(1);
        private CalendarBlackoutDatesCollection _blackoutDates;
        public CalendarBlackoutDatesCollection BlackoutDates
        {
            get => _blackoutDates;
            set
            {
                if (_blackoutDates != value)
                {
                    _blackoutDates = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    ExecuteViewCalendarRange();
                    OnPropertyChanged();
                }
            }
        }
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    DatePicker t = new DatePicker();

                    t.BlackoutDates.Add(new CalendarDateRange(new DateTime(), _startDate));
                    BlackoutDates = t.BlackoutDates;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isCheckedViewAll = false;
        private bool _isCheckedToday = false;
        public bool IsCheckedViewAll
        {
            get => _isCheckedViewAll;
            set
            {
                if (_isCheckedViewAll != value)
                {
                    _isCheckedViewAll = value;
                    if (value) IsCheckedToday = !value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsCheckedToday
        {
            get => _isCheckedToday;
            set
            {
                if (_isCheckedToday != value)
                {
                    _isCheckedToday = value;
                    if (value) IsCheckedViewAll = !value;
                    OnPropertyChanged();
                }
            }
        }

        public void ExecuteViewAllQuery()
        {
            using (var conn = new TAHCoffeeEntities())
            {
                string queryString = @"
                                select IdNumber, ExportTime, PromoId,
                                case 
                                    when CustomerId IS NULL then 'Guest' 
                                    else CustomerId 
                                end as CustomerId, Total   
                                from Bill";

                HistoryList = new ObservableCollection<Bill>(conn.Bills.SqlQuery(queryString).ToList());
            }
        }
        public void ExecuteViewToday()
        {
            using (var conn = new TAHCoffeeEntities())
            {
                string queryString = @"
                                select IdNumber, ExportTime, PromoId,
                                case 
                                    when CustomerId IS NULL then 'Guest' 
                                    else CustomerId 
                                end as CustomerId, Total
                                from Bill
                                where  day(ExportTime) = @day and MONTH(ExportTime) = @month and YEAR(ExportTime) = @year";
                HistoryList = new ObservableCollection<Bill>(
                    conn.Bills.SqlQuery(queryString,
                                        new SqlParameter("@day", DateTime.Today.Day),
                                        new SqlParameter("@month", DateTime.Today.Month),
                                        new SqlParameter("@year", DateTime.Today.Year))
                                .ToList());
            }
        }
        public void ExecuteViewCalendarRange()
        {
            try
            {
                using (var conn = new TAHCoffeeEntities())
                {
                    string queryString = @"
                                select IdNumber, ExportTime, PromoId,
                                case 
                                    when CustomerId IS NULL then 'Guest' 
                                    else CustomerId 
                                end as CustomerId, Total  
                                from Bill
                                where ExportTime between @start and @end";

                    HistoryList = new ObservableCollection<Bill>(
                        conn.Bills.SqlQuery(queryString,
                                            new SqlParameter("@start", StartDate.ToString("M/d/y")),
                                            new SqlParameter("@end", EndDate.ToString("M/d/y")))
                                   .ToList());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }
        public ICommand executeViewAllCommand { get; set; }
        public ICommand executeViewTodayCommand { get; set; }
        public ICommand executeViewCalendarRange { get; set; }
        public ICommand DatePicker_SelectedDateChanged { get; set; }

        public HistoryViewModel()
        {

            ControlsEnabled = "False";

            ExecuteViewCalendarRange();
            executeViewTodayCommand = new RelayCommand<bool>((p) => { return true; }, (p) => { if (p) ExecuteViewToday(); });
            executeViewAllCommand = new RelayCommand<bool>((p) => { return true; }, (p) => { if (p) ExecuteViewAllQuery(); });
            executeViewCalendarRange = new RelayCommand<string>((p) => { return true; }, (p) => { ExecuteViewCalendarRange(); });
            DatePicker_SelectedDateChanged = new RelayCommand<object>(p => true, p => { IsCheckedToday = IsCheckedViewAll = false; });
        }
    }
}
