using _4NH_HAO_Coffee_Shop.ViewModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.WPF;
using System;
using System.Windows.Controls;
using System.Windows.Diagnostics;
using System.Windows.Media;

namespace _4NH_HAO_Coffee_Shop.View
{
    /// <summary>
    /// Interaction logic for DashBoardView.xaml
    /// </summary>
    public partial class DashBoardView : UserControl
    {
        public DashBoardView()
        {
            InitializeComponent();
        }

        private void LargeCartesianChart_ChartPointPointerDown(LiveChartsCore.Kernel.Sketches.IChartView chart, LiveChartsCore.Kernel.ChartPoint point)
        {
            var parent1 = VisualTreeHelper.GetParent(pieChartRevenue);
            var parent2 = VisualTreeHelper.GetParent(pieChartQuantity);
            var parent3 = VisualTreeHelper.GetParent(CustomerCartesianChart);
            if (parent1 != null)
            {
                Grid Parent = parent1 as Grid;
                Parent.Children.Remove(pieChartRevenue);
                pieChartRevenue = new PieChart();
                pieChartRevenue.Series = (DataContext as DashBoardViewModel).PieChartRevenueSeries;
                pieChartRevenue.InitialRotation = -90;
                Parent.Children.Add(pieChartRevenue);
            }
            if (parent2 != null)
            {
                Grid Parent = parent2 as Grid;
                Parent.Children.Remove(pieChartQuantity);
                pieChartQuantity = new PieChart();
                pieChartQuantity.Series = (DataContext as DashBoardViewModel).PieChartQuantitySeries;
                pieChartQuantity.InitialRotation = -90;
                Parent.Children.Add(pieChartQuantity);
            }
            if (parent3 != null)
            {
                Grid Parent = parent3 as Grid;
                Parent.Children.Remove(CustomerCartesianChart);
                CustomerCartesianChart = new CartesianChart();
                CustomerCartesianChart.Series = (DataContext as DashBoardViewModel).CustomerCartesianSeries;
                Parent.Children.Add(CustomerCartesianChart);
            }
            ((DashBoardViewModel)DataContext).handlCartesianChartMouseDownEvent(chart, point);
        }
    }
}
