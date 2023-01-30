using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _4NH_HAO_Coffee_Shop.Utils
{
    class BlackoutDatesExtention : DependencyObject
    {
        public static DependencyProperty RegisterBlackoutDatesProperty =
            DependencyProperty
            .RegisterAttached("RegisterBlackoutDates",
                typeof(System.Windows.Controls.CalendarBlackoutDatesCollection),
                typeof(BlackoutDatesExtention),
                new PropertyMetadata(null, OnRegisterCommandBindingChanged));

        public static void SetRegisterBlackoutDates(UIElement element, CalendarBlackoutDatesCollection value)
        {
            element?.SetValue(RegisterBlackoutDatesProperty, value);
        }
        public static CalendarBlackoutDatesCollection GetRegisterBlackoutDates(UIElement element)
        {
            return (element != null ? (CalendarBlackoutDatesCollection)element.GetValue(RegisterBlackoutDatesProperty) : null);
        }
        private static void OnRegisterCommandBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DatePicker element = sender as DatePicker;
            if (element != null)
            {
                CalendarBlackoutDatesCollection bindings = e.NewValue as CalendarBlackoutDatesCollection;
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
    }
    class AlterSourceExtention : DependencyObject
    {
        public static DependencyProperty RegisterAlterSourceProperty =
            DependencyProperty
            .RegisterAttached("RegisterAlterSource",
                typeof(string),
                typeof(AlterSourceExtention),
                new PropertyMetadata(null, OnRegisterCommandBindingChanged));

        public static void SetRegisterAlterSource(DependencyObject element, string value)
        {
            element?.SetValue(RegisterAlterSourceProperty, value);
        }

        public static string GetRegisterAlterSource(UIElement element)
        {
            return (element != null ? (string)element.GetValue(RegisterAlterSourceProperty) : null);
        }

        private static void OnRegisterCommandBindingChanged(DependencyObject seeder, DependencyPropertyChangedEventArgs e)
        {
            switch (seeder.GetType().Name)
            {
                case "Image":
                    (seeder as Image).Source = TableOfImage.Instance.GetBitmapImage(e.NewValue as string);
                    break;
                case "ImageBrush":
                    (seeder as ImageBrush).ImageSource = TableOfImage.Instance.GetBitmapImage(e.NewValue as string);
                    break;
                default:
                    break;
            }

        }
    }

}
