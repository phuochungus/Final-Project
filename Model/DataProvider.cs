using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{
<<<<<<< HEAD
=======
    //Lớp sử dụng Signleton Pattern để truy cập database
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
    internal class DataProvider
    {
        private static DataProvider _ins;
        public static DataProvider Ins
        {
            get
            {
                if (_ins == null) _ins = new DataProvider();
                return _ins;
            }
<<<<<<< HEAD
            set
            {
                _ins = value;
            }
        }
        
        public TAHCoffeeEntities DB { get; set; }
        
        public DataProvider()
=======
        }

        public TAHCoffeeEntities DB { get; set; }

        private DataProvider()
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
        {
            DB = new TAHCoffeeEntities();
        }
    }
}
