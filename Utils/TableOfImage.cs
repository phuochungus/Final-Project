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
    public class TableOfImage
    {
        private Dictionary<string, BitmapImage> dict = new Dictionary<string, BitmapImage>();
        public BitmapImage GetBitmapImage(string URL)
        {
            if (dict.ContainsKey(URL)) return dict[URL];
            else
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(URL);
                image.EndInit();
                dict.Add(URL, image);
                return image;
            }
        }
        private TableOfImage() { }
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
    }
}
