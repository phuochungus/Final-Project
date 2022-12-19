using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Input;
using _4NH_HAO_Coffee_Shop.Model;
using _4NH_HAO_Coffee_Shop.Utils;
using LiveChartsCore;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using static OfficeOpenXml.ExcelErrorValue;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class DashBoardViewModel : BaseViewModel
    {

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
                Mapping = (month, point) =>
                {
                    point.PrimaryValue=(double)month;
                    point.SecondaryValue=point.Context.Index;
                }
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
        public PieSeries<Pie> pieChartSeries { get; set; }

        ObservableCollection<Pie> data = new ObservableCollection<Pie>() { new Pie("1", 4), new Pie("2", 4), new Pie("3", 1) };

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
        private void fetchAndTranformDataWholeMonth()
        {

        }

        public void handlCartesianChartMouseDownEvent(LiveChartsCore.Kernel.Sketches.IChartView chart, LiveChartsCore.Kernel.ChartPoint point)
        {
            Console.WriteLine(chart);
            Console.WriteLine(point);
            int SelectedMonth = (int)point.SecondaryValue + 1;

        }

        //public ISeries[] Series { get; set; }
        //   = new ISeries[]
        //   {
        //        new PieSeries<double> { Values = new double[] { 2 } },
        //        new PieSeries<double> { Values = new double[] { 4 } },
        //        new PieSeries<double> { Values = new double[] { 1 } },
        //        new PieSeries<double> { Values = new double[] { 4 } },
        //        new PieSeries<double> { Values = new double[] { 3 } }
        //   };

        public ISeries[] citiesSeries { get; set; } =
        {
            new PieSeries<int>
            {
                Values = new ObservableCollection<int>{1},
                Name ="pie 1"
                
            },
            new PieSeries<int>
            {
                Values = new ObservableCollection<int>{2},
                Name="pie 2"
            }
        };


        public DashBoardViewModel()
        {

            fetchAndTranformDataWholeYear();
        }

        public class City
        {
            public string Name { get; set; }
            public int Population { get; set; }
        }
    }
}
