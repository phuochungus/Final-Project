using _4NH_HAO_Coffee_Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _4NH_HAO_Coffee_Shop.Utils
{
    /// <summary>
    /// Lớp TableOfImage sử dụng kiến trúc Singleton Pattern
    /// Lớp này dùng như một object có thể truy cập tại mọi nơi 
    /// Lớp này có tác dụng là lưu trữ ảnh theo phương pháp <Key,Value>
    /// </summary>
    public class TableOfImage
    {
        //Lưu trữ
        private Dictionary<string, BitmapImage> dict = new Dictionary<string, BitmapImage>();
        //Truy xuất ảnh thông qua URL
        public BitmapImage getBitmapImage(string URL)
        {
            if (dict.ContainsKey(URL)) 
                return dict[URL];
            else
            {
                //Không có sẵn ảnh
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                //Tải ảnh
                image.UriSource = new Uri(URL);
                image.EndInit();
                //Thêm vào lưu trữ 
                dict.Add(URL, image);
                return image;
            }
        }
        //Singleton 
        private static TableOfImage _instance;
        
        public static TableOfImage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TableOfImage();
                return _instance;
            }
        }
        private TableOfImage() { }
    }
}