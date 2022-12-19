using System.Windows;
using System.Windows.Controls;

namespace _4NH_HAO_Coffee_Shop.Utils
{
    class BlackoutDatesExtention : DependencyObject
    {
        #region RegisterBlackoutDates
        // Usage: <DatePicker hacks:AttachedProperties.RegisterBlackoutDates="{Binding BlackoutDates}" >
        public static DependencyProperty RegisterBlackoutDatesProperty = DependencyProperty.RegisterAttached("RegisterBlackoutDates", typeof(System.Windows.Controls.CalendarBlackoutDatesCollection), typeof(BlackoutDatesExtention), new PropertyMetadata(null, OnRegisterCommandBindingChanged));
        public static void SetRegisterBlackoutDates(UIElement element, System.Windows.Controls.CalendarBlackoutDatesCollection value)
        {
            if (element != null)
                element.SetValue(RegisterBlackoutDatesProperty, value);
        }
        public static System.Windows.Controls.CalendarBlackoutDatesCollection GetRegisterBlackoutDates(UIElement element)
        {
            return (element != null ? (System.Windows.Controls.CalendarBlackoutDatesCollection)element.GetValue(RegisterBlackoutDatesProperty) : null);
        }
        private static void OnRegisterCommandBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.DatePicker element = sender as System.Windows.Controls.DatePicker;
            if (element != null)
            {
                System.Windows.Controls.CalendarBlackoutDatesCollection bindings = e.NewValue as System.Windows.Controls.CalendarBlackoutDatesCollection;
                if (bindings != null)
                {
                    element.BlackoutDates.Clear();
                    foreach (var dateRange in bindings)
                    {
                        element.SelectedDate = dateRange.End.AddDays(1);
                        element.BlackoutDates.Add(dateRange);
                    }
                }
            }
        }
        #endregion
    }
    class AlterSourceExtention : DependencyObject
    {
        public static DependencyProperty RegisterAlterSourceProperty = DependencyProperty.RegisterAttached("RegisterAlterSource", typeof(string), typeof(AlterSourceExtention), new PropertyMetadata(null, OnRegisterCommandBindingChanged));
        public static void SetRegisterAlterSource(UIElement element, string value)
        {
            if (element != null)
                element.SetValue(RegisterAlterSourceProperty, value);
        }
        public static string GetRegisterAlterSource(UIElement element)
        {
            return (element != null ? (string)element.GetValue(RegisterAlterSourceProperty) : null);

        }
        private static void OnRegisterCommandBindingChanged(DependencyObject seeder, DependencyPropertyChangedEventArgs e)
        {
            Image element = seeder as Image;
            if(element!=null)
            {
                string binding = e.NewValue as string;
                if (binding != null)
                {
                    element.Source = TableOfImage.Instance.GetBitmapImage(binding);
                }
            }
        }
    }

}
