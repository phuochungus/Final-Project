using LiveChartsCore.SkiaSharpView.WPF;
using System;
using System.Windows.Controls;

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

        private void cartesianChart_ChartPointPointerDown(LiveChartsCore.Kernel.Sketches.IChartView chart, LiveChartsCore.Kernel.ChartPoint point)
        {
        }
    }
}
