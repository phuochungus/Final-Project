using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using _4NH_HAO_Coffee_Shop.Model;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Windows.Input;
using System.Security.Cryptography;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    //Abstract class quản lý biểu đồ 
    public abstract class ChartUIController : BaseViewModel
    {
        protected ObservableCollection<ISeries> series;

        public ObservableCollection<ISeries> seriesProperty
        {
            get => series;
            set
            {
                series = value;
                OnPropertyChanged();
            }
        }

        protected ChartUIController()
        {
            series = new ObservableCollection<ISeries>();
        }
        //setup định dạng
        protected abstract void setupVisualChartWithDefaulSetting();
        //setup định dạng hình ảnh dữ liệu 
        protected abstract void setupDefaultSeries();
        //setup định dạng hình ảnh biểu đồ
        protected abstract void setupDefaultAxes();

    }
    //Interface biến đổi dữ liệu thô sang dữ liệu đồ thị 
    interface TranformDataImp
    {
        //đọc dữ liệu thô và vẽ đồ thị với LiveChart
        void tranformRawDataToVisualChart();
    }

    //Quản lý biểu đồ DailyTotalCustomer 
    public class DailyTotalCustomerChartControl : ChartUIController, TranformDataImp
    {
        private List<FetchCustomerOfMonth_Result> rawData;

        public ObservableCollection<Axis> HorizontalAxis { get; set; }

        public ObservableCollection<Axis> VerticalAxis { get; set; }

        public DailyTotalCustomerChartControl()
        {
            setupVisualChartWithDefaulSetting();
        }

        protected override void setupVisualChartWithDefaulSetting()
        {
            setupDefaultSeries();
            setupDefaultAxes();
        }

        protected override void setupDefaultSeries()
        {
            if (seriesProperty == null)
                seriesProperty = new ObservableCollection<ISeries>();
            seriesProperty.Add(new ColumnSeries<int>
            {
                Values = new ObservableCollection<int>(),
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue}",
            });
        }

        protected override void setupDefaultAxes()
        {
            VerticalAxis = new ObservableCollection<Axis>{new Axis
            {
                Labels = new ObservableCollection<string>(),
                Name = "Customer",
                MinStep = 1,
            }};

            HorizontalAxis = new ObservableCollection<Axis>{new Axis
            {
                Labels = new ObservableCollection<string>(),
                Name = "Date",
                MinStep = 1,
            }};
        }

        public void tranformRawDataToVisualChart()
        {
            var totalCustomerOfEachDate = new ObservableCollection<int>();
            var Dates = new ObservableCollection<string>();
            int numberOfColumn = determineNumberOfColumn();

            rawData.Sort(new TotalCustumerDescendingComparer());

            seriesProperty.Clear();
            seriesProperty.Add(new ColumnSeries<int>
            {
                Values = new ObservableCollection<int>(),
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue}",
            });

            for (int i = 0; i < numberOfColumn; i++)
            {
                totalCustomerOfEachDate.Add(rawData[i].Customer.GetValueOrDefault());
                Dates.Add(rawData[i].Day.ToString());
            }
            seriesProperty[0].Values = totalCustomerOfEachDate;
            HorizontalAxis[0].Labels = Dates;
        }

        private int determineNumberOfColumn()
        {
            int numberOfCulumn;
            const int maxNumberOfColumn = 5;

            if (rawData.Count > maxNumberOfColumn)
                numberOfCulumn = maxNumberOfColumn;
            else
                numberOfCulumn = rawData.Count;

            return numberOfCulumn;
        }

        public void setRawData(List<FetchCustomerOfMonth_Result> rawData)
        {
            this.rawData = rawData;
        }


    }

    //Quản lý biểu đồ MonthlyRevenue 
    public class MonthlyRevenueChartControl : ChartUIController, TranformDataImp
    {
        private List<MonthlyRevenue> rawData;

        public ObservableCollection<Axis> HorizontalAxis { get; set; }

        public ObservableCollection<Axis> VerticalAxis { get; set; }

        public MonthlyRevenueChartControl()
        {
            setupVisualChartWithDefaulSetting();
        }

        protected override void setupVisualChartWithDefaulSetting()
        {
            setupDefaultSeries();
            setupDefaultAxes();
        }

        protected override void setupDefaultSeries()
        {
            if (seriesProperty == null)
                seriesProperty = new ObservableCollection<ISeries>();
            seriesProperty.Add(new LineSeries<long>
            {
                Values = new ObservableCollection<long>(new long[12]),
                Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                Stroke = null,
                GeometryFill = null,
                GeometryStroke = null,
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"))}",
            });
        }

        protected override void setupDefaultAxes()
        {
            HorizontalAxis = new ObservableCollection<Axis>{new Axis
            {
                Labels = new List<string> {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" }
            }};
            VerticalAxis = new ObservableCollection<Axis>{new Axis
            {
                Labeler = (value) => value.ToString("C0",CultureInfo.CreateSpecificCulture("vi-VN")),
            }};
        }


        public void tranformRawDataToVisualChart()
        {
            ObservableCollection<long> dummy = new ObservableCollection<long>(new long[12]);
            foreach (MonthlyRevenue monthRevenue in rawData)
            {
                dummy[monthRevenue.Month - 1] = monthRevenue.Revenue.GetValueOrDefault();
            }
            seriesProperty[0].Values = dummy;
        }

        public void setRawData(List<MonthlyRevenue> rawData)
        {
            this.rawData = rawData;
        }

    }

    //Quản lý biểu đồ ProductSalesRevenue
    public class ProductSalesRevenueChartControl : ChartUIController, TranformDataImp
    {
        private List<FetchDataOfMonth_Result> rawData;

        public ProductSalesRevenueChartControl()
        {
            setupVisualChartWithDefaulSetting();
        }

        protected override void setupVisualChartWithDefaulSetting()
        {
            //method intentionally left empty
        }

        protected override void setupDefaultSeries()
        {
            //method intentionally left empty
        }

        protected override void setupDefaultAxes()
        {
            //method intentionally left empty
        }

        public void tranformRawDataToVisualChart()
        {
            rawData.Sort(new PriceDescendingComparer());
            seriesProperty.Clear();
            foreach (FetchDataOfMonth_Result productSalesRevenueDetail in rawData)
            {
                seriesProperty.Add(new PieSeries<int>
                {
                    Values = new ObservableCollection<int> { productSalesRevenueDetail.Price.GetValueOrDefault() },
                    Name = productSalesRevenueDetail.DisplayName,
                    Tag = productSalesRevenueDetail,
                    Mapping = (FetchDataOfMonth_Result, Point) => Point.PrimaryValue = productSalesRevenueDetail.Price.GetValueOrDefault(),
                    TooltipLabelFormatter = (chartPoint) => $"{chartPoint.Context.Series.Name} {chartPoint.PrimaryValue.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"))} {chartPoint.StackedValue.Share:P2}",
                });
            }
        }

        public void setRawData(List<FetchDataOfMonth_Result> rawData)
        {
            this.rawData = rawData;
        }

    }

    //Quản lý biểu đồ ProductSoldQuantity
    public class ProductSoldQuantityChartControl : ChartUIController, TranformDataImp
    {
        private List<FetchDataOfMonth_Result> rawData;

        protected override void setupVisualChartWithDefaulSetting()
        {
            //this method intentionally left empty
        }

        protected override void setupDefaultAxes()
        {
            //this method intentionally left empty
        }

        protected override void setupDefaultSeries()
        {
            //this method intentionally left empty
        }

        public void tranformRawDataToVisualChart()
        {
            rawData.Sort(new QuantityDescendingComparer());
            seriesProperty.Clear();
            foreach (FetchDataOfMonth_Result productSalesRevenueDetail in rawData)
            {
                seriesProperty.Add(new PieSeries<int>
                {
                    Values = new ObservableCollection<int> { productSalesRevenueDetail.Quantity.GetValueOrDefault() },
                    Name = productSalesRevenueDetail.DisplayName,
                    Tag = productSalesRevenueDetail,
                    Mapping = (FetchDataOfMonth_Result, Point) => Point.PrimaryValue = productSalesRevenueDetail.Quantity.GetValueOrDefault(),
                    TooltipLabelFormatter = (chartPoint) => $"{chartPoint.Context.Series.Name} {chartPoint.PrimaryValue} {chartPoint.StackedValue.Share:P2}",
                });
            }
        }

        public void setRawData(List<FetchDataOfMonth_Result> rawData)
        {
            this.rawData = rawData;
        }
    }


    public class DashBoardViewModel : BaseViewModel
    {
        // 4 object quản lý 4 biểu đồ khác nhau 
        public MonthlyRevenueChartControl monthlyRevenueChartControl { get; set; } = new MonthlyRevenueChartControl();
        public ProductSalesRevenueChartControl productSalesRevenueChartControl { get; set; } = new ProductSalesRevenueChartControl();
        public ProductSoldQuantityChartControl productSoldQuantityChartControl { get; set; } = new ProductSoldQuantityChartControl();
        public DailyTotalCustomerChartControl dailyTotalCustomerChartControl { get; set; } = new DailyTotalCustomerChartControl();

        // Command sẽ thực hiện Action khi người dùng chọn menu Dashboad từ ứng dụng 
        public ICommand visibleTriggerActionCommand { get; set; }

        public DashBoardViewModel()
        {
            visibleTriggerActionCommand = new RelayCommand<System.Windows.DependencyPropertyChangedEventArgs>(p => true, p =>
            {
                //nếu EventArgs.NewValue == true tức là Visible có giá trị mới là true
                if (p.NewValue as bool? == true)
                    //fetch dữ liệu và vẽ lại biểu đồ MonthlyRevenue
                    fetchAndTranformMonthlyRevenueDataOfCurrentYear();
            });
        }

        private void fetchAndTranformMonthlyRevenueDataOfCurrentYear()
        {
            var rawFetchedData = getRawMonthlyRevenuesDataFromDatabase();

            monthlyRevenueChartControl.setRawData(rawFetchedData);
            monthlyRevenueChartControl.tranformRawDataToVisualChart();
        }


        private List<MonthlyRevenue> getRawMonthlyRevenuesDataFromDatabase()
        {
            using (var DB = new TAHCoffeeEntities())
            {
                //Stored Procedure
                return DB.MonthlyRevenues.ToList();
            }
        }

        //Hiễn thị dữ liệu các biểu đồ con khi nhấn vào biểu đồ lớn
        public void handleBigChartMouseDownEvent(IChartView chart, LiveChartsCore.Kernel.ChartPoint point)
        {
            int SelectedMonth = (int)point.SecondaryValue + 1;
            fetchAndTranformDataOfMonth(SelectedMonth);
        }

        private void fetchAndTranformDataOfMonth(int selectedMonth)
        {                       
            //fetch dữ liệu và vẽ lại biểu đồ cho 2 biểu đồ tròn
            var rawProductDataFromSelectedMonth = getRawProductDataFromDatabase(selectedMonth);

            productSalesRevenueChartControl.setRawData(rawProductDataFromSelectedMonth);
            productSalesRevenueChartControl.tranformRawDataToVisualChart();

            productSoldQuantityChartControl.setRawData(rawProductDataFromSelectedMonth);
            productSoldQuantityChartControl.tranformRawDataToVisualChart();

            //fetch dữ liệu cho biểu đồ cột
            var rawFetchedTotalCustomerData = getRawTotalCustomerDataFromDatabase(selectedMonth);

            dailyTotalCustomerChartControl.setRawData(rawFetchedTotalCustomerData);
            dailyTotalCustomerChartControl.tranformRawDataToVisualChart();
        }

        private List<FetchDataOfMonth_Result> getRawProductDataFromDatabase(int selectedMonth)
        {
            using (var DB = new TAHCoffeeEntities())
            {
                //Stored Procedure
                return DB.FetchDataOfMonth(selectedMonth).ToList();
            }
        }

        private List<FetchCustomerOfMonth_Result> getRawTotalCustomerDataFromDatabase(int selectedMonth)
        {
            using (var DB = new TAHCoffeeEntities())
            {
                //Stored Procedure
                return DB.FetchCustomerOfMonth(selectedMonth).ToList();
            }
        }
    }
}
