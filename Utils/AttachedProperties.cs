using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _4NH_HAO_Coffee_Shop.Utils
{
    class BlackoutDatesExtention : DependencyObject
    {
        /// <summary>
        /// Đăng ký một attached property 
        /// </summary>
        /// cách dùng 
        /// <DatePicker utils:BlackoutDatesExtention.RegisterBlackoutDates = "{Binding ...}" />
        /// 
        public static DependencyProperty RegisterBlackoutDatesProperty =
            DependencyProperty
            .RegisterAttached("RegisterBlackoutDates",
                typeof(System.Windows.Controls.CalendarBlackoutDatesCollection),
                typeof(BlackoutDatesExtention),
                new PropertyMetadata(null, OnRegisterCommandBindingChanged));
        //getter và setter mặc định 
        public static void SetRegisterBlackoutDates(UIElement element, CalendarBlackoutDatesCollection value)
        {
            element?.SetValue(RegisterBlackoutDatesProperty, value);
        }
        public static CalendarBlackoutDatesCollection GetRegisterBlackoutDates(UIElement element)
        {
            return (element != null ? (CalendarBlackoutDatesCollection)element.GetValue(RegisterBlackoutDatesProperty) : null);
        }
        /// <summary>
        /// Method sẽ thực hiện khi giá trị binding tại Attached Property này thay đổi 
        /// </summary>
        private static void OnRegisterCommandBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //chuyển kiểu sender 
            DatePicker element = sender as DatePicker;
            if (element != null)
            {
                //chuyển kiểu và lấy giá trị mới tại property mà binding với attached property này  
                CalendarBlackoutDatesCollection bindings = e.NewValue as CalendarBlackoutDatesCollection;
                if (bindings != null)
                {
                    //Cập nhật BlackoutDates 
                    //Xóa BlackoutDates cũ
                    element.BlackoutDates.Clear();
                    //binding sẽ chỉ chứa 1 phần tử
                    foreach (CalendarDateRange dateRange in bindings)
                    {   
                        //giá trị SelectedDate của DatePicker sẽ là sau dateRange 1 ngày 
                        element.SelectedDate = dateRange.End.AddDays(1);
                        //Thêm dateRange vào BlackoutDates
                        element.BlackoutDates.Add(dateRange);
                    }
                    //Giá trị SelectedDate lớn nhất mà DatePicker có thể chọn
                    element.DisplayDateEnd = DateTime.Today;
                }
            }
        }
    }

    class AlterSourceExtention : DependencyObject
    {
        /// <summary>
        /// Đăng ký một attached property 
        /// </summary>
        /// cách dùng 
        /// <Image utils:AlterSourceExtention.RegisterAlterSource="{Binding ...}"/>
        /// <ImageBrush utils:AlterSourceExtention.RegisterAlterSource="{Binding ...}"/>
        public static DependencyProperty RegisterAlterSourceProperty =
            DependencyProperty
            .RegisterAttached("RegisterAlterSource",
                typeof(string),
                typeof(AlterSourceExtention),
                new PropertyMetadata(null, OnRegisterCommandBindingChanged));
        
        //getter và setter mặc định 
        public static void SetRegisterAlterSource(DependencyObject element, string value)
        {
            element?.SetValue(RegisterAlterSourceProperty, value);
        }
        public static string GetRegisterAlterSource(UIElement element)
        {
            return (element != null ? (string)element.GetValue(RegisterAlterSourceProperty) : null);
        }
        /// <summary>
        /// Method sẽ thực hiện khi giá trị binding tại Attached Property này thay đổi
        /// </summary>
        private static void OnRegisterCommandBindingChanged(DependencyObject seeder, DependencyPropertyChangedEventArgs e)
        {
            //Xem xét kiểu của seender và đặt thuộc tính Source hay ImageSource
            switch (seeder.GetType().Name)
            {
                case "Image":
                    (seeder as Image).Source = TableOfImage.Instance.getBitmapImage(e.NewValue as string);
                    break;
                case "ImageBrush":
                    (seeder as ImageBrush).ImageSource = TableOfImage.Instance.getBitmapImage(e.NewValue as string);
                    break;
                default:
                    break;
            }

        }
    }

}