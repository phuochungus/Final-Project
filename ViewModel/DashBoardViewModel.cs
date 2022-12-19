using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using _4NH_HAO_Coffee_Shop.Model;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using static OfficeOpenXml.ExcelErrorValue;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class City
    {
        public string Name { get; set; }
        public double Population { get; set; }
        public double Density { get; set; }
    }
    public class DashBoardViewModel : BaseViewModel
    {
        public ISeries[] Series { get; set; } =
        {
            new LineSeries<long>
            {
                Values = new long[] { 7000000000, 2, 7, 2, 7, 2 },
                Fill = null,
                GeometrySize = 0,
                LineSmoothness = 1
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


        public DashBoardViewModel()
        {
            using (var conn = new TAHCoffeeEntities())
            {
                var result = conn.MonthlyRevenues.ToList();
                long[] temp = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                foreach(var obj in result)
                {
                    temp[obj.Month - 1] = obj.Revenue.GetValueOrDefault();
                }
                Series[0].Values = temp;
            }; 
        }
    }
}
