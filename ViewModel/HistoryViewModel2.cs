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
using _4NH_HAO_Coffee_Shop.Utils;
using System.Runtime.InteropServices.ComTypes;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class CheckableItem : BaseViewModel
    {
        public bool isChecked;
        public bool isCheckedProperty
        {
            get => isChecked;
            set
            {
                isChecked = value;
                notifyPropertyChange(nameof(isCheckedProperty));
            }
        }
        private string name;
        public string nameProperty
        {
            get => name;
            set
            {
                name = value;
                notifyPropertyChange(nameof(nameProperty));
            }
        }

        public CheckableItem(string name, bool isChecked)
        {
            this.name = name;
            this.isChecked = isChecked;
        }
    }

    public class TransactionLogFilter : BaseViewModel
    {
        private CalendarDateRange searchRange;
        public DateTime startDate
        {
            get => searchRange.Start;
            set
            {
                searchRange.Start = value;
                notifyPropertyChange();
            }
        }
        public DateTime endDate
        {
            get => searchRange.End;
            set
            {
                searchRange.End = value;
                notifyPropertyChange();
            }
        }
    }

    public class TransactionLogAdvancedSearcher : BaseViewModel
    {
        private const int VIEW_NONE = -1;
        private const int VIEW_RANGE = 0;
        private const int VIEW_ALL = 1;
        private const int VIEW_TODAY = 2;
        private ObservableCollection<Bill> resultLog = new ObservableCollection<Bill>();
        public FullyObservableCollection<CheckableItem> searchOptions = new FullyObservableCollection<CheckableItem>();
        public FullyObservableCollection<CheckableItem> searchOptionsProperty
        {
            get => searchOptions;
            set
            {
                searchOptions = value;
                notifyPropertyChange();
            }
        }
        public TransactionLogFilter transactionLogFilter;

        private void createDefaultOptionsGroup()
        {
            searchOptions.Add(new CheckableItem("View range", false));
            searchOptions.Add(new CheckableItem("View all", false));
            searchOptions.Add(new CheckableItem("View today", false));
        }

        public TransactionLogAdvancedSearcher()
        {
            createDefaultOptionsGroup();
        }

        public ObservableCollection<Bill> getSearchResult()
        {
            return resultLog;
        }

        public void executeSearching()
        {
            int chosenOption = getCurrentChoose();
            switch (chosenOption)
            {
                case VIEW_RANGE:
                    searchTransactionInRange();
                    break;
                case VIEW_TODAY:
                    searchTransactionInToday();
                    break;
                case VIEW_ALL:
                    searchAllTransaction();
                    break;
                default:
                    break;
            }
        }

        private void searchAllTransaction()
        {
            resultLog = fetchAllLog();
        }

        private ObservableCollection<Bill> fetchAllLog()
        {
            ObservableCollection<Bill> transactionLog;
            const string queryString = @"
                                select IdNumber, ExportTime, PromoId,
                                case 
                                    when CustomerId IS NULL then 'Guest' 
                                    else CustomerId 
                                end as CustomerId, Total   
                                from Bill";
            using (var DB = new TAHCoffeeEntities())
            {
                transactionLog = new ObservableCollection<Bill>(
                    DB.Bills.SqlQuery(
                        queryString
                    )
                    .ToList()
                    );

            }
            return transactionLog;
        }

        private void searchTransactionInToday()
        {
            resultLog = fetchTodayLog();
        }

        private ObservableCollection<Bill> fetchTodayLog()

        {
            ObservableCollection<Bill> transactionLog;
            const string queryString = @"
                                select IdNumber, ExportTime, PromoId,
                                case 
                                    when CustomerId IS NULL then 'Guest' 
                                    else CustomerId 
                                end as CustomerId, Total
                                from Bill
                                where  day(ExportTime) = @day and MONTH(ExportTime) = @month and YEAR(ExportTime) = @year";
            using (var DB = new TAHCoffeeEntities())
            {
                transactionLog = new ObservableCollection<Bill>(
                    DB.Bills.SqlQuery(
                        queryString,
                            new SqlParameter("@day", DateTime.Today.Day),
                            new SqlParameter("@month", DateTime.Today.Month),
                            new SqlParameter("@year", DateTime.Today.Year)
                        )
                        .ToList()
                    );
            }
            return transactionLog;
        }

        private void searchTransactionInRange()
        {
            resultLog = fetchLogRangeBetweenDateTime(transactionLogFilter);
        }


        private ObservableCollection<Bill> fetchLogRangeBetweenDateTime(TransactionLogFilter transactionLogFilter)
        {
            ObservableCollection<Bill> transactionLog;
            const string queryString = @"
                                select IdNumber, ExportTime, PromoId,
                                case 
                                    when CustomerId IS NULL then 'Guest' 
                                    else CustomerId 
                                end as CustomerId, Total  
                                from Bill
                                where ExportTime between @start and @end"; using (var DB = new TAHCoffeeEntities())
            {


                transactionLog = new ObservableCollection<Bill>(
                DB.Bills.SqlQuery(queryString,
                                        new SqlParameter("@start", transactionLogFilter.startDate.ToString("M/d/y")),
                                        new SqlParameter("@end", transactionLogFilter.endDate.ToString("M/d/y")))
                               .ToList());
            }
            return transactionLog;
        }

        private int getCurrentChoose()
        {
            int currentOption = -1;
            for (int index = 0; index < searchOptions.Count; index++)
            {
                if (searchOptions[index].isChecked)
                {
                    currentOption = index;
                }
            }
            return currentOption;
        }
    }

    public class HistoryViewModel2 : BaseViewModel
    {
        private ObservableCollection<Bill> transactionLog = new ObservableCollection<Bill>();
        public ObservableCollection<Bill> transactionLogProperty
        {
            get => transactionLog;
            set
            {
                if (transactionLog != value)
                {
                    transactionLog = value;
                    notifyPropertyChange();
                }
            }
        }
        private CalendarBlackoutDatesCollection backoutDates;
        public CalendarBlackoutDatesCollection blackoutDatesProperty
        {
            get => backoutDates;
            set
            {
                if (backoutDates != value)
                {
                    backoutDates = value;
                    notifyPropertyChange();
                }
            }
        }
        public ICommand changeSearchOption { get; set; }
        public TransactionLogAdvancedSearcher transactionLogSearcher = new TransactionLogAdvancedSearcher();
        public TransactionLogAdvancedSearcher transactionLogSearcherProperty
        {
            get => transactionLogSearcher;
            set
            {
                transactionLogSearcher = value;
                notifyPropertyChange();
            }
        }
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
                    foreach (Bill bill in transactionLogProperty)
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
                Console.WriteLine(e.Message);
                System.Windows.MessageBox.Show("Export unsuccessful");
            }
        }

        private void notifyOptionChanged(CheckableItem selectedOption)
        {
            foreach (CheckableItem searchOption in transactionLogSearcher.searchOptions)
            {
                if (selectedOption == searchOption)
                {
                    searchOption.isCheckedProperty = true;
                }
                else
                {
                    searchOption.isCheckedProperty = false;
                }
            }
        }
        public delegate void ExecuteDelegate();
        public static bool keepFetchingSearchResult = true;
        public HistoryViewModel2()
        {
            changeSearchOption = new RelayCommand<CheckableItem>(selectedOption => true, selectedOption =>
            {
                notifyOptionChanged(selectedOption);
                transactionLogSearcher.executeSearching();
                transactionLogProperty = transactionLogSearcher.getSearchResult();

            });
            ExportCommand = new RelayCommand<object>(p => true, p => { ExportToExcel(); });
        }
    }
}
