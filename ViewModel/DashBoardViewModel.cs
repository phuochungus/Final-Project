using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management;
using _4NH_HAO_Coffee_Shop.Model;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using static SkiaSharp.HarfBuzz.SKShaper;
using System.Windows.Input;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    //abstract class quản lý Chart
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

        public ChartUIController()
        {
            series = new ObservableCollection<ISeries>();
        }
    }

    interface TranformDataImp
    {
        void TranformRawDataToVisualChart();

        void setupUIWithDefaulSetting();
    }

    public class DailyTotalCustomerChartControl : ChartUIController, TranformDataImp
    {
        private List<FetchCustomerOfMonth_Result> rawData;

        public ObservableCollection<Axis> HorizontalAxis { get; set; }

        public ObservableCollection<Axis> VerticalAxis { get; set; }

        public DailyTotalCustomerChartControl()
        {
            setupUIWithDefaulSetting();
        }

        public void setupUIWithDefaulSetting()
        {
            if (seriesProperty == null)
                seriesProperty = new ObservableCollection<ISeries>();
            seriesProperty.Add(new ColumnSeries<int>
            {
                Values = new ObservableCollection<int>(),
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue}",
            });

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

        public void TranformRawDataToVisualChart()
        {
            rawData.Sort(new TotalCustumerDescendingComparer());
            seriesProperty.Clear();
            seriesProperty.Add(new ColumnSeries<int>
            {
                Values = new ObservableCollection<int>(),
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.PrimaryValue}",
            });

            var totalCustomerOfDates = new ObservableCollection<int>();
            var Dates = new ObservableCollection<string>();

            int maxNumberOfColumn = determineNumberOfColumn();

            for (int i = 0; i < maxNumberOfColumn; i++)
            {

                totalCustomerOfDates.Add(rawData[i].Customer.GetValueOrDefault());
                Dates.Add(rawData[i].Day.ToString());
            }
            seriesProperty[0].Values = totalCustomerOfDates;
            HorizontalAxis[0].Labels = Dates;
        }

        private int determineNumberOfColumn()
        {
            int maxNumberOfColumn;

            if (rawData.Count > 5)
                maxNumberOfColumn = 5;
            else
                maxNumberOfColumn = rawData.Count;

            return maxNumberOfColumn;
        }

        public void setRawData(List<FetchCustomerOfMonth_Result> rawData)
        {
            this.rawData = rawData;
        }
    }

    public class MonthlyRevenueChartControl : ChartUIController, TranformDataImp
    {
        private List<MonthlyRevenue> rawData;

        public ObservableCollection<Axis> HorizontalAxis { get; set; }

        public ObservableCollection<Axis> VerticalAxis { get; set; }

        public MonthlyRevenueChartControl()
        {
            setupUIWithDefaulSetting();
        }

        public void setupUIWithDefaulSetting()
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

            HorizontalAxis = new ObservableCollection<Axis>{new Axis
            {
                Labels = new List<string> {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" }
            }};
            VerticalAxis = new ObservableCollection<Axis>{new Axis
            {
                Labeler = (value) => value.ToString("C0",CultureInfo.CreateSpecificCulture("vi-VN")),
            }};
        }

        public void setRawData(List<MonthlyRevenue> rawData)
        {
            this.rawData = rawData;
        }

        public void TranformRawDataToVisualChart()
        {
            ObservableCollection<long> dummy = new ObservableCollection<long>(new long[12]);
            foreach (MonthlyRevenue monthRevenue in rawData)
            {
                dummy[monthRevenue.Month - 1] = monthRevenue.Revenue.GetValueOrDefault();
            }
            seriesProperty[0].Values = dummy;
        }
    }

    public class ProductSalesRevenueChartControl : ChartUIController, TranformDataImp
    {
        private List<FetchDataOfMonth_Result> rawData;

        public ProductSalesRevenueChartControl()
        {
            setupUIWithDefaulSetting();
        }

        public void setupUIWithDefaulSetting()
        {
            //method intentionally left empty
        }

        public void setRawData(List<FetchDataOfMonth_Result> rawData)
        {
            this.rawData = rawData;
        }

        public void TranformRawDataToVisualChart()
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
    }

    public class ProductSoldQuantityChartControl : ChartUIController, TranformDataImp
    {
        private List<FetchDataOfMonth_Result> rawData;

        public void setRawData(List<FetchDataOfMonth_Result> rawData)
        {
            this.rawData = rawData;
        }

        public void setupUIWithDefaulSetting()
        {
            //this method intentionally left empty
        }

        public void TranformRawDataToVisualChart()
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
    }


    public class DashBoardViewModel : BaseViewModel
    {

        public MonthlyRevenueChartControl monthlyRevenueChartControl { get; set; } = new MonthlyRevenueChartControl();
        public ProductSalesRevenueChartControl productSalesRevenueChartControl { get; set; } = new ProductSalesRevenueChartControl();
        public ProductSoldQuantityChartControl productSoldQuantityChartControl { get; set; } = new ProductSoldQuantityChartControl();
        public DailyTotalCustomerChartControl dailyTotalCustomerChartControl { get; set; } = new DailyTotalCustomerChartControl();

        public ICommand visibleTriggerCommand { get; set; }

        public DashBoardViewModel()
        {
            visibleTriggerCommand = new RelayCommand<System.Windows.DependencyPropertyChangedEventArgs>(p => true, p =>
            {
                if (p.NewValue as bool? == true)
                    fetchAndTranformMonthlyRevenueDataOfCurrentYear();
            });

        }

        private void fetchAndTranformMonthlyRevenueDataOfCurrentYear()
        {
            var rawFetchedData = getRawMonthlyRevenuesDataFromDatabase();

            monthlyRevenueChartControl.setRawData(rawFetchedData);
            monthlyRevenueChartControl.TranformRawDataToVisualChart();
        }

        private void fetchAndTranformDataOfMonth(int selectedMonth)
        {
            var rawProductDataFromSelectedMonth = getRawProductDataFromDatabase(selectedMonth);

            productSalesRevenueChartControl.setRawData(rawProductDataFromSelectedMonth);
            productSalesRevenueChartControl.TranformRawDataToVisualChart();

            productSoldQuantityChartControl.setRawData(rawProductDataFromSelectedMonth);
            productSoldQuantityChartControl.TranformRawDataToVisualChart();


            var rawFetchedTotalCustomerData = getRawTotalCustomerDataFromDatabase(selectedMonth);
            dailyTotalCustomerChartControl.setRawData(rawFetchedTotalCustomerData);
            dailyTotalCustomerChartControl.TranformRawDataToVisualChart();
        }

        private List<MonthlyRevenue> getRawMonthlyRevenuesDataFromDatabase()
        {
            using (var DB = new TAHCoffeeEntities())
            {
                return DB.MonthlyRevenues.ToList();
            }
        }
        private List<FetchDataOfMonth_Result> getRawProductDataFromDatabase(int selectedMonth)
        {
            using (var DB = new TAHCoffeeEntities())
            {
                return DB.FetchDataOfMonth(selectedMonth).ToList();
            }
        }

        private List<FetchCustomerOfMonth_Result> getRawTotalCustomerDataFromDatabase(int selectedMonth)
        {
            using (var DB = new TAHCoffeeEntities())
            {
                return DB.FetchCustomerOfMonth(selectedMonth).ToList();
            }
        }

        public void handlCartesianChartMouseDownEvent(IChartView chart, LiveChartsCore.Kernel.ChartPoint point)
        {
            int SelectedMonth = (int)point.SecondaryValue + 1;
            fetchAndTranformDataOfMonth(SelectedMonth);
        }

    }
}
