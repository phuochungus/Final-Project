using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _4NH_HAO_Coffee_Shop.Utils
{
    class AttachedProperties : DependencyObject
    {

        #region RegisterBlackoutDates

        // Adds a collection of command bindings to a date picker's existing BlackoutDates collection, since the collections are immutable and can't be bound to otherwise.
        //
        // Usage: <DatePicker hacks:AttachedProperties.RegisterBlackoutDates="{Binding BlackoutDates}" >

        public static DependencyProperty RegisterBlackoutDatesProperty = DependencyProperty.RegisterAttached("RegisterBlackoutDates", typeof(System.Windows.Controls.CalendarBlackoutDatesCollection), typeof(AttachedProperties), new PropertyMetadata(null, OnRegisterCommandBindingChanged));

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
}
