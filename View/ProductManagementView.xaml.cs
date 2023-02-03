using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _4NH_HAO_Coffee_Shop.View
{
    /// <summary>
    /// Interaction logic for ProductManagementView.xaml
    /// </summary>
    public partial class ProductManagementView : UserControl
    {
        public ProductManagementView()
        {
            InitializeComponent();
        }
        private void SetDefaultColorButtons()
        {
            PromoButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF3C186");
            ItemButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF3C186");
            CategoryButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF3C186");
            UnitButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF3C186");
        }
        private void PromoButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultColorButtons();
            PromoButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF38EB3");
        }
        private void ItemButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultColorButtons();
            ItemButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF38EB3");
        }
        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultColorButtons();
            CategoryButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF38EB3");
        }
        private void UnitButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultColorButtons();
            UnitButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFF38EB3");
        }
    }
}
