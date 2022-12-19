using _4NH_HAO_Coffee_Shop.ViewModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace _4NH_HAO_Coffee_Shop.View
{
    /// <summary>
    /// Interaction logic for DashBoardView.xaml
    /// </summary>
    public partial class DashBoardView : UserControl
    {
        public City[] cities = new City[]
        {
            new City { Name = "Tokyo", Population = 10, Density = 5 },
            new City { Name = "Cape Town", Population = 9, Density = 6 },
            new City { Name = "New York", Population = 8, Density = 7 }
        };
        public DashBoardView()
        {
            InitializeComponent();
        }
    }
}
