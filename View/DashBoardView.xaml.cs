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
            var parentOfPiechartRevenue = VisualTreeHelper.GetParent(productSalesRevenueChart);
            var parentOfPiechartProductSoldQuantity = VisualTreeHelper.GetParent(productSoldQuantityChart);
            var parentOfDailyTotalCustomerChart = VisualTreeHelper.GetParent(dailyTotalCustomerChart);
            if (parentOfPiechartRevenue != null)
            {
                Grid Parent = parentOfPiechartRevenue as Grid;
                Parent.Children.Remove(productSalesRevenueChart);
                productSalesRevenueChart = new PieChart();
                productSalesRevenueChart.Series = (DataContext as DashBoardViewModel).productSalesRevenueChartControl.seriesProperty;
                productSalesRevenueChart.InitialRotation = -90;
                Parent.Children.Add(productSalesRevenueChart);
            }
            if (parentOfPiechartProductSoldQuantity != null)
            {
                Grid Parent = parentOfPiechartProductSoldQuantity as Grid;
                Parent.Children.Remove(productSoldQuantityChart);
                productSoldQuantityChart = new PieChart();
                productSoldQuantityChart.Series = (DataContext as DashBoardViewModel).productSoldQuantityChartControl.seriesProperty;
                productSoldQuantityChart.InitialRotation = -90;
                Parent.Children.Add(productSoldQuantityChart);
            }
            if (parentOfDailyTotalCustomerChart != null)
            {
                Grid Parent = parentOfDailyTotalCustomerChart as Grid;
                Parent.Children.Remove(dailyTotalCustomerChart);
                dailyTotalCustomerChart = new CartesianChart();
                dailyTotalCustomerChart.YAxes = (DataContext as DashBoardViewModel).dailyTotalCustomerChartControl.VerticalAxis;
                dailyTotalCustomerChart.XAxes = (DataContext as DashBoardViewModel).dailyTotalCustomerChartControl.HorizontalAxis;
                dailyTotalCustomerChart.Series = (DataContext as DashBoardViewModel).dailyTotalCustomerChartControl.seriesProperty;
                Parent.Children.Add(dailyTotalCustomerChart);
            }
            ((DashBoardViewModel)DataContext).handlCartesianChartMouseDownEvent(chart, point);
        }
    }
}
