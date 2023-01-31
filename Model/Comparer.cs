using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{
    public class PriceDescendingComparer : IComparer<FetchDataOfMonth_Result>
    {
        public int Compare(FetchDataOfMonth_Result x, FetchDataOfMonth_Result y)
        {
            return y.Price.GetValueOrDefault().CompareTo(x.Price.GetValueOrDefault());
        }
    }

    public class QuantityDescendingComparer : IComparer<FetchDataOfMonth_Result>
    {
        public int Compare(FetchDataOfMonth_Result x, FetchDataOfMonth_Result y)
        {
            return y.Quantity.GetValueOrDefault().CompareTo(x.Quantity.GetValueOrDefault());
        }
    }
    public class TotalCustumerDescendingComparer : IComparer<FetchCustomerOfMonth_Result>
    {
        public int Compare(FetchCustomerOfMonth_Result x, FetchCustomerOfMonth_Result y)
        {
            return y.Customer.GetValueOrDefault().CompareTo(x.Customer.GetValueOrDefault());
        }
    }
}

