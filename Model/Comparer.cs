using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{
    public class PriceComparer : IComparer<FetchDataOfMonth_Result>
    {
        public int Compare(FetchDataOfMonth_Result x, FetchDataOfMonth_Result y)
        {
            return y.Price.GetValueOrDefault().CompareTo(x.Price.GetValueOrDefault());
        }
    }

    public class QuantityComparer : IComparer<FetchDataOfMonth_Result>
    {
        public int Compare(FetchDataOfMonth_Result x, FetchDataOfMonth_Result y)
        {
            return y.Quantity.GetValueOrDefault().CompareTo(x.Quantity.GetValueOrDefault());
        }
    }
}

