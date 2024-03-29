﻿using _4NH_HAO_Coffee_Shop.Model;
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
    //class này quy định khoảng thời gian mà TransactionLogAdvancedSearcher có thể thực hiện search khi search option = VIEW_RANGE
    public class TransactionLogFilter : BaseViewModel
    {
        private CalendarBlackoutDatesCollection blachoutDates;
        private CalendarDateRange searchRange;

        public CalendarBlackoutDatesCollection blackoutDatesProperty
        {
            get => blachoutDates;
            set
            {
                blachoutDates = value;
                OnPropertyChanged();
            }
        }
        public DateTime startDate
        {
            get => searchRange.Start;
            set
            {
                searchRange.Start = value;
                DatePicker dummy = new DatePicker();
                dummy.BlackoutDates.Add(new CalendarDateRange(new DateTime(), startDate));
                blackoutDatesProperty = dummy.BlackoutDates;
                OnPropertyChanged();
            }
        }
        public DateTime endDate
        {
            get => searchRange.End;
            set
            {
                searchRange.End = value;
                OnPropertyChanged();
            }
        }

        public TransactionLogFilter()
        {
            searchRange = new CalendarDateRange(DateTime.Now, DateTime.Today.AddDays(1));
        }
    }

    public class CheckableItem : BaseViewModel
    {
        public bool isChecked;
        private string name;

        public bool isCheckedProperty
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged(nameof(isCheckedProperty));
            }
        }
        public string nameProperty
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(nameProperty));
            }
        }

        public CheckableItem(string name, bool isChecked)
        {
            this.name = name;
            this.isChecked = isChecked;
        }
    }

    public class TransactionLogAdvancedSearcher : BaseViewModel
    {
        public const int VIEW_RANGE = 0;
        public const int VIEW_ALL = 1;
        public const int VIEW_TODAY = 2;

        private int currentChoose = VIEW_TODAY;
        private ObservableCollection<BillWrapper> resultLog;
        public FullyObservableCollection<CheckableItem> searchOptions;
        public TransactionLogFilter transactionLogFilterProperty { get; set; }

        public FullyObservableCollection<CheckableItem> searchOptionsProperty
        {
            get => searchOptions;
            set
            {
                searchOptions = value;
                OnPropertyChanged();
            }
        }

        private void createDefaultOptionsGroup()
        {
            searchOptions.Add(new CheckableItem("View range", false));
            searchOptions.Add(new CheckableItem("View all", false));
            searchOptions.Add(new CheckableItem("View today", false));
        }

        public TransactionLogAdvancedSearcher()
        {
            resultLog = new ObservableCollection<BillWrapper>();
            searchOptions = new FullyObservableCollection<CheckableItem>();
            transactionLogFilterProperty = new TransactionLogFilter();

            createDefaultOptionsGroup();
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

        public int getCurrentChoose()
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


        public int currentChooseProperty
        {
            get
            {
                currentChoose = getCurrentChoose();
                return currentChoose;
            }
            set
            {
                currentChoose = value;
                foreach (var option in searchOptions)
                {
                    option.isCheckedProperty = false;
                }
                searchOptions[currentChoose].isCheckedProperty = true;
            }
        }

        private void searchTransactionInRange()
        {
            resultLog = fetchLogRangeBetweenDateTime(transactionLogFilterProperty);
        }

        private ObservableCollection<BillWrapper> fetchLogRangeBetweenDateTime(TransactionLogFilter transactionLogFilter)
        {
            const string queryString = @"
                                select IdNumber, ExportTime, PromoId,
                                case 
                                    when CustomerId IS NULL then 'Guest' 
                                    else CustomerId 
                                end as CustomerId, Total  
                                from Bill
                                where ExportTime between @start and @end";

            ObservableCollection<BillWrapper> transactionLog = new ObservableCollection<BillWrapper>();

            using (var DB = new TAHCoffeeEntities())
            {
                var rawData = new ObservableCollection<Bill>(
                DB.Bills.SqlQuery(queryString,
                                        new SqlParameter("@start", transactionLogFilter.startDate.ToString("M/d/y")),
                                        new SqlParameter("@end", transactionLogFilter.endDate.ToString("M/d/y")))
                               .ToList());
                foreach (Bill raw in rawData)
                {
                    transactionLog.Add(new BillWrapper { bill = raw });
                }
            }
            return transactionLog;
        }

        private void searchTransactionInToday()
        {
            resultLog = fetchTodayLog();
        }

        private ObservableCollection<BillWrapper> fetchTodayLog()
        {
            ObservableCollection<BillWrapper> transactionLog = new ObservableCollection<BillWrapper>();

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
                var rawData = new ObservableCollection<Bill>(
                    DB.Bills.SqlQuery(
                        queryString,
                            new SqlParameter("@day", DateTime.Today.Day),
                            new SqlParameter("@month", DateTime.Today.Month),
                            new SqlParameter("@year", DateTime.Today.Year)
                        )
                        .ToList()
                    );
                foreach (Bill raw in rawData)
                {
                    transactionLog.Add(new BillWrapper { bill = raw });
                }
            }
            return transactionLog;
        }

        private void searchAllTransaction()
        {
            resultLog = fetchAllLog();
        }

        private ObservableCollection<BillWrapper> fetchAllLog()
        {
            ObservableCollection<BillWrapper> transactionLog = new ObservableCollection<BillWrapper>();
            const string queryString = @"
                                select IdNumber, ExportTime, PromoId,
                                case 
                                    when CustomerId IS NULL then 'Guest' 
                                    else CustomerId 
                                end as CustomerId, Total   
                                from Bill";
            using (var DB = new TAHCoffeeEntities())
            {
                var rawData = new ObservableCollection<Bill>(DB.Bills.SqlQuery(queryString).ToList());
                foreach (Bill raw in rawData)
                {
                    transactionLog.Add(new BillWrapper { bill = raw });
                }

            }
            return transactionLog;
        }

        public ObservableCollection<BillWrapper> getSearchResult()
        {
            return resultLog;
        }
    }

    public class ExcelExporter
    {
        private string resultPath = "";
        private SaveFileDialog saveFileDialog;

        private ObservableCollection<BillWrapper> sourceLog { get; set; }

        public ExcelExporter()
        {
            createDefaultSaveFileDialog();
        }

        private void createDefaultSaveFileDialog()
        {
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
        }

        public void ExportToExcel()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    resultPath = getFileNameWithPath();
                    ExcelWorksheet sheet = createDefaultSheet(excelPackage);
                    writeTransactionogToSheet(sourceLog, sheet);

                    if (successWriteToFile(excelPackage) == true)
                    {
                        System.Windows.MessageBox.Show("Export successful");
                    }
                }
            }
        }

        private string getFileNameWithPath()
        {
            return saveFileDialog.FileName;
        }

        private ExcelWorksheet createDefaultSheet(ExcelPackage excelPackage)
        {
            ExcelWorksheet sheet = null;
            sheet = createExcelFileWitlEmptySheet(sheet, excelPackage);
            addColumnNames(ref sheet);
            formatSheetToDefaultStyle(ref sheet);
            return sheet;

        }

        private void writeTransactionogToSheet(ObservableCollection<BillWrapper> sourceLog, ExcelWorksheet sheet)
        {
            int columnIndex = 2;
            foreach (BillWrapper bill in sourceLog)
            {
                sheet.Cells[columnIndex, 1].Value = bill.IdNumber;
                sheet.Cells[columnIndex, 2].Value = bill.ExportTime.ToString();
                sheet.Cells[columnIndex, 3].Value = bill.CustomerId;
                sheet.Cells[columnIndex, 4].Value = bill.Total;
                columnIndex++;
            }
            fitColumn(sheet);
        }

        private ExcelWorksheet createExcelFileWitlEmptySheet(ExcelWorksheet sheet, ExcelPackage excelPackage)
        {
            sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
            return sheet;
        }

        private void addColumnNames(ref ExcelWorksheet sheet)
        {
            sheet.Cells[1, 1].Value = "No";
            sheet.Cells[1, 2].Value = "Date";
            sheet.Cells[1, 3].Value = "Customer ID";
            sheet.Cells[1, 4].Value = "Total";
        }

        private void formatSheetToDefaultStyle(ref ExcelWorksheet sheet)
        {
            sheet.TabColor = System.Drawing.Color.Black;
            sheet.DefaultRowHeight = 12;
            sheet.Row(1).Height = 20;
            sheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(1).Style.Font.Bold = true;
        }

        private bool successWriteToFile(ExcelPackage excelPackage)
        {
            try
            {
                if (File.Exists(resultPath))
                {
                    File.Delete(resultPath);
                }
                FileStream objFilestrm = File.Create(resultPath);
                objFilestrm.Close();
                File.WriteAllBytes(resultPath, excelPackage.GetAsByteArray());
                return true;
            }
            catch
            {
                return false;
            }
        }


        private void fitColumn(ExcelWorksheet sheet)
        {
            sheet.Column(1).AutoFit();
            sheet.Column(2).AutoFit();
            sheet.Column(3).AutoFit();
            sheet.Column(4).AutoFit();
        }

        public void setSource(ObservableCollection<BillWrapper> sourceSheet)
        {
            this.sourceLog = sourceSheet;
        }
    }

    public class HistoryViewModel : BaseViewModel
    {
        private Timer refreshTransactionScreenTimer;
        private ObservableCollection<BillWrapper> transactionLog;
        public TransactionLogAdvancedSearcher transactionLogSearcher;
        public ExcelExporter excelExporter;

        public ICommand notifyEndDateChangedCommand { get; set; }
        public ICommand exportCommand { get; set; }
        public ICommand changeSearchOption { get; set; }

        public TransactionLogAdvancedSearcher transactionLogSearcherProperty
        {
            get => transactionLogSearcher;
            set
            {
                transactionLogSearcher = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<BillWrapper> transactionLogProperty
        {
            get => transactionLog;
            set
            {
                if (transactionLog != value)
                {
                    transactionLog = value;
                    OnPropertyChanged();
                }
            }
        }

        public static bool keepFetchingSearchResult = true;

        public HistoryViewModel()
        {
            //Giữ giao dịch sẽ được hiển thị 
            transactionLog = new ObservableCollection<BillWrapper>();

            //Công cụ tìm kiếm lịch sữ giao dịch
            transactionLogSearcher = new TransactionLogAdvancedSearcher();

            //Công cụ xuất file excel
            excelExporter = new ExcelExporter();

            //Timer phục vụ cho việc theo dõi kết quả realtime
            refreshTransactionScreenTimer = createDefaultRefetchTimer();
            refreshTransactionScreenTimer.Start();

            //Thông báo khi search option trên UI thay đổi
            changeSearchOption = new RelayCommand<CheckableItem>(selectedOption => true, selectedOption =>
            {
                //Thông báo search option thay đổi 
                notifyOptionChanged(selectedOption);
                //Tìm kiếm lại các giao dịch đúng yêu cầu
                transactionLogSearcher.executeSearching();
                //Hiển thị kết quả tìm kiếm
                showResultLog();
            });

            //Thực hiện khi nhấn nút Export Report
            exportCommand = new RelayCommand<System.Windows.Controls.Button>(btnExport => true, btnExport =>
            {
                //Đặt nguồn dữ liệu cho exporter là các giao dịch được hiển thị 
                excelExporter.setSource(transactionLogProperty);
                //Xuất file
                excelExporter.ExportToExcel();
            });

            //Thực hiện khi giá trị End Date bị thay đổi trong DateRange dùng trong Search Range
            notifyEndDateChangedCommand = new RelayCommand<DatePicker>(endDatePicker => true, endDatePicker =>
            {
                //Tìm kiếm lại các giao dịch đúng yêu cầu
                transactionLogSearcher.executeSearching();
                //Hiển thị kết quả tìm kiếm
                showResultLog();
            });
        }

        private Timer createDefaultRefetchTimer()
        {
            Timer refetchTransactionScreenTimer = new Timer();
            setIntervalBetweenFetchingTransaction(refetchTransactionScreenTimer);
            assignDefaultFetchingTransactionMethod(refetchTransactionScreenTimer);
            return refetchTransactionScreenTimer;
        }

        private void setIntervalBetweenFetchingTransaction(Timer timer)
        {
            timer.Interval = 1000;
        }

        private void assignDefaultFetchingTransactionMethod(Timer timer)
        {
            timer.Tick += (s, e) =>
            {
                int currentSearchChoose = transactionLogSearcher.getCurrentChoose();
                if (keepFetchingSearchResult == true)
                {
                    if (currentSearchChoose == TransactionLogAdvancedSearcher.VIEW_ALL || currentSearchChoose == TransactionLogAdvancedSearcher.VIEW_TODAY)
                    {
                        transactionLogSearcher.executeSearching();
                        showResultLog();
                        Console.WriteLine("fetched");
                    }
                }
            };
        }

        private void showResultLog()
        {
            transactionLogProperty = transactionLogSearcher.getSearchResult();
        }

        private void notifyOptionChanged(CheckableItem selectedOption)
        {
            foreach (CheckableItem searchOption in transactionLogSearcher.searchOptions)
            {
                if (selectedOption == searchOption)
                    searchOption.isCheckedProperty = true;
                else
                    searchOption.isCheckedProperty = false;
            }
        }

    }
}
