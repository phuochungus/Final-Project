using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class CategoryWrapper : BaseViewModel
    {
        Category category;
        public string DisplayName
        {
            get
            {
                return category.DisplayName;
            }
            set
            {
                category.DisplayName = value;
                OnPropertyChanged();
            }
        }
    }
}
