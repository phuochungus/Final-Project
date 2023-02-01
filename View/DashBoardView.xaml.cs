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
            var parentOfPiechartRevenue = VisualTreeHelper.GetParent(ProductSalesRevenue);
            var parentOfPiechartProductSoldQuantity = VisualTreeHelper.GetParent(ProductSoldQuantity);
            var parentOfDailyTotalCustomerChart = VisualTreeHelper.GetParent(DailyTotalCustomer);
            if (parentOfPiechartRevenue != null)
            {
                Grid Parent = parentOfPiechartRevenue as Grid;
                Parent.Children.Remove(ProductSalesRevenue);
                ProductSalesRevenue = new PieChart();
                ProductSalesRevenue.Series = (DataContext as DashBoardViewModel).productSalesRevenueChartControl.seriesProperty;
                ProductSalesRevenue.InitialRotation = -90;
                Parent.Children.Add(ProductSalesRevenue);
            }
            if (parentOfPiechartProductSoldQuantity != null)
            {
                Grid Parent = parentOfPiechartProductSoldQuantity as Grid;
                Parent.Children.Remove(ProductSoldQuantity);
                ProductSoldQuantity = new PieChart();
                ProductSoldQuantity.Series = (DataContext as DashBoardViewModel).productSoldQuantityChartControl.seriesProperty;
                ProductSoldQuantity.InitialRotation = -90;
                Parent.Children.Add(ProductSoldQuantity);
            }
            if (parentOfDailyTotalCustomerChart != null)
            {
                Grid Parent = parentOfDailyTotalCustomerChart as Grid;
                Parent.Children.Remove(DailyTotalCustomer);
                DailyTotalCustomer = new CartesianChart();
                DailyTotalCustomer.YAxes = (DataContext as DashBoardViewModel).dailyTotalCustomerChartControl.VerticalAxis;
                DailyTotalCustomer.XAxes = (DataContext as DashBoardViewModel).dailyTotalCustomerChartControl.HorizontalAxis;
                DailyTotalCustomer.Series = (DataContext as DashBoardViewModel).dailyTotalCustomerChartControl.seriesProperty;
                Parent.Children.Add(DailyTotalCustomer);
            }
            ((DashBoardViewModel)DataContext).handleBigChartMouseDownEvent(chart, point);
        }
    }
}
