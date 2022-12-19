using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.Model
{
    public class Pie
    {
        public string Name { get; set; } = "Pie name";
        public double Value { get; set; } = 0;
        public Pie(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}
