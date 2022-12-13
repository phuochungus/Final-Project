using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

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
        public ICommand ExportCommand { get; set; }

        private void ExportToExcel()
        {

            try
            {
                string filePath = "";
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    filePath = sf.FileName;
                    ExcelPackage excel = new ExcelPackage();
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;

                    workSheet.Cells[1, 1].Value = "No";
                    workSheet.Cells[1, 2].Value = "Date";
                    workSheet.Cells[1, 3].Value = "Customer ID";
                    workSheet.Cells[1, 4].Value = "Total";
                    int index = 2;
                    foreach (Bill bill in HistoryList)
                    {
                        workSheet.Cells[index, 1].Value = bill.IdNumber;
                        workSheet.Cells[index, 2].Value = bill.ExportTime.ToString();
                        workSheet.Cells[index, 3].Value = bill.CustomerId;
                        workSheet.Cells[index, 4].Value = bill.Total;
                        index++;
                    }
                    workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).AutoFit();
                    workSheet.Column(4).AutoFit();
                    if (File.Exists(filePath)) File.Delete(filePath);
                    FileStream objFilestrm = File.Create(filePath);
                    objFilestrm.Close();
                    File.WriteAllBytes(filePath, excel.GetAsByteArray());
                    excel.Dispose();
                    System.Windows.MessageBox.Show("Export successful");
                }

               
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Export unsuccessful");
            }
        }

        public void VisibleTrigger()
        {
            Console.WriteLine("event fired");
        }

        public HistoryViewModel()
        {

            ControlsEnabled = "False";
            ExecuteViewCalendarRange();
            executeViewTodayCommand = new RelayCommand<bool>((p) => { return true; }, (p) => { if (p) ExecuteViewToday(); });
            executeViewAllCommand = new RelayCommand<bool>((p) => { return true; }, (p) => { if (p) ExecuteViewAllQuery(); });
            executeViewCalendarRange = new RelayCommand<string>((p) => { return true; }, (p) => { ExecuteViewCalendarRange(); });
            DatePicker_SelectedDateChanged = new RelayCommand<object>(p => true, p => { IsCheckedToday = IsCheckedViewAll = false; });
            ExportCommand = new RelayCommand<object>(p => true, p => { ExportToExcel(); });
        }

    }
}
