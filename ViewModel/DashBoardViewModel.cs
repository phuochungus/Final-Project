using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Input;
using _4NH_HAO_Coffee_Shop.Model;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using static OfficeOpenXml.ExcelErrorValue;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class DashBoardViewModel : BaseViewModel
    {
        public ISeries[] Series { get; set; } =
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
        private void GetData()
        {
            using (var conn = new TAHCoffeeEntities())
            {
                var result = conn.MonthlyRevenues.ToList();
                ObservableCollection<long> temp = new ObservableCollection<long> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                foreach (var obj in result)
                {
                    temp[obj.Month - 1] = obj.Revenue.GetValueOrDefault();
                }
                Series[0].Values = temp;
            };
        }
        public ICommand handlMouseDownEventCommand { get; set; }
        void handlMouseDownEvent(object e)
        {
            Console.Write(e);
        }


        public DashBoardViewModel()
        {
            GetData();
            handlMouseDownEventCommand = new RelayCommand<object>(p => true, p => handlMouseDownEvent(p));
        }
    }
}
