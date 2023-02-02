using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{
    //Lớp sử dụng Signleton Pattern để truy cập database
    internal class DataProvider
    {
        private static DataProvider _ins;
        public static DataProvider Ins
        {
            get
            {
                if (_ins == null) 
                    _ins = new DataProvider();
                return _ins;
            }
        }

        public TAHCoffeeEntities DB { get; set; }

        private DataProvider()
        {
            DB = new TAHCoffeeEntities();
        }
    }
}
