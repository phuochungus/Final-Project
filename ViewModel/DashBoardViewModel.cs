using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Cryptography;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

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
            new LineSeries<double>
            {
                Values = new double[] { 7000000000, 2, 7, 2, 7, 2 },
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
        }
    }
}
