using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using _4NH_HAO_Coffee_Shop.Model;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class DashBoardViewModel : BaseViewModel
    {
        private ObservableCollection<ISeries> pieChartRevenueSeries = new ObservableCollection<ISeries>();
        private ObservableCollection<ISeries> pieChartQuantitySeries = new ObservableCollection<ISeries>();
        private ObservableCollection<ISeries> customerCartesianSeries = new ObservableCollection<ISeries>();
        public ISeries[] cartesianChartSeries { get; set; } =
        {
            new LineSeries<long>
            {
                Values = new ObservableCollection<long>{0},
                Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                Stroke = null,
                GeometryFill = null,
                GeometryStroke = null,
                TooltipLabelFormatter =
                    (chartPoint) => $"{chartPoint.PrimaryValue.ToString("C3",CultureInfo.CreateSpecificCulture("vi-VN"))}",
            }

        };
        public List<Axis> X { get; set; } = new List<Axis>
        {
            new Axis
            {
                Labels= new List<string> {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec" }
            }
        };
        public List<Axis> Y { get; set; } = new List<Axis>
        {
            new Axis
            {
                Labeler=(value) => value.ToString("C3",CultureInfo.CreateSpecificCulture("vi-VN")),
            }
        };

        public ObservableCollection<ISeries> PieChartRevenueSeries
        {
            get => pieChartRevenueSeries;
            set
            {
                pieChartRevenueSeries = value;
                OnPropertyChanged(nameof(PieChartRevenueSeries));
            }
        }
        public ObservableCollection<ISeries> PieChartQuantitySeries
        {
            get => pieChartQuantitySeries;
            set
            {
                pieChartQuantitySeries = value;
                OnPropertyChanged(nameof(PieChartQuantitySeries));
            }
        }
        public ObservableCollection<ISeries> CustomerCartesianSeries
        {
            get => customerCartesianChart;
            set
            {
                customerCartesianChart = value;
                OnPropertyChanged(nameof(CustomerCartesianChart));
            }
        }

        private void fetchAndTranformDataWholeYear()
        {
            using (var conn = new TAHCoffeeEntities())
            {
                var result = conn.MonthlyRevenues.ToList();
                ObservableCollection<long> temp = new ObservableCollection<long> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                foreach (var obj in result)
                {
                    temp[obj.Month - 1] = obj.Revenue.GetValueOrDefault();
                }
                cartesianChartSeries[0].Values = temp;
            };
        }
        private void fetchAndTranformDataWholeMonth(int SeletedMonth)
        {
            List<FetchDataOfMonth_Result> table1, table2;
            using (var conn = new TAHCoffeeEntities())
            {
                table1 = table2 = conn.FetchDataOfMonth(SeletedMonth).ToList();
            }
            PieChartRevenueSeries.Clear();
            PieChartQuantitySeries.Clear();
            table1.Sort(new PriceComparer());
            table2.Sort(new QuantityComparer());
            foreach (FetchDataOfMonth_Result row in table1)
            {
                PieChartRevenueSeries.Add(
                    new PieSeries<int>
                    {
                        Values = new ObservableCollection<int> { row.Price.GetValueOrDefault() },
                        Name = row.DisplayName,
                        Tag = row,
                        Mapping = (FetchDataOfMonth_Result, Point) =>
                        {
                            Point.PrimaryValue = row.Price.GetValueOrDefault();
                        },
                        TooltipLabelFormatter =
                            (chartPoint) => $"{chartPoint.Context.Series.Name} {chartPoint.PrimaryValue.ToString("C3", CultureInfo.CreateSpecificCulture("vi-VN"))} {chartPoint.StackedValue.Share:P2}",
                    }
                );
            }

            foreach (FetchDataOfMonth_Result row in table2)
            {
                PieChartQuantitySeries.Add(
                   new PieSeries<int>
                   {
                       Values = new ObservableCollection<int> { row.Quantity.GetValueOrDefault() },
                       Name = row.DisplayName,
                       Tag = row,
                       Mapping = (FetchDataOfMonth_Result, Point) =>
                       {
                           Point.PrimaryValue = row.Quantity.GetValueOrDefault();
                       },
                       TooltipLabelFormatter =
                            (chartPoint) => $"{chartPoint.Context.Series.Name} {chartPoint.PrimaryValue} {chartPoint.StackedValue.Share:P2}",
                   }
                );
            }
        }

        public void handlCartesianChartMouseDownEvent(IChartView chart, LiveChartsCore.Kernel.ChartPoint point)
        {
            int SelectedMonth = (int)point.SecondaryValue + 1;
            fetchAndTranformDataWholeMonth(SelectedMonth);
        }
        public DashBoardViewModel()
        {
            fetchAndTranformDataWholeYear();
        }
    }
}
