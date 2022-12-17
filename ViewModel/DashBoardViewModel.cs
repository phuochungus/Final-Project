using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4NH_HAO_Coffee_Shop.Model;
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
        public City[] cities = new City[]
            {
            new City { Name = "Tokyo", Population = 10, Density = 5 },
            new City { Name = "Cape Town", Population = 9, Density = 6 },
            new City { Name = "New York", Population = 8, Density = 7 }
            };

        public ObservableCollection<ISeries> myChartControlSeries { get; set; }

        public DashBoardViewModel()
        {
            myChartControlSeries = new ObservableCollection<ISeries>
            { new LineSeries<City> { Values = cities } };
        }
    }
}
