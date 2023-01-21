using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using _4NH_HAO_Coffee_Shop.Utils;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<Category> _categoryList;
        public ObservableCollection<Category> CategoryList { get => _categoryList; set { _categoryList = value; notifyPropertyChange(); } }
        private Category _getCategory;
        public Category GetCategory { get => _getCategory; set { _getCategory = value; notifyPropertyChange(); } }
        private ObservableCollection<Item> _categorizedItemList;
        public ObservableCollection<Item> categorizedItemList { get => _categorizedItemList; set { _categorizedItemList = value; notifyPropertyChange(); } }
        private int totalPrice = 0;
        public int TotalPrice
        {
            get => totalPrice;
            set
            {
                if (totalPrice != value)
                {
                    totalPrice = value;
                    notifyPropertyChange();
                }
            }
        }
        private int _currentCategory;
        public int currentCategory
        {
            get => _currentCategory;
            set
            {
                if (value != _currentCategory)
                {
                    _currentCategory = value;
                    notifyPropertyChange();
                }
            }
        }

        public ICommand CategoryChangeCommand { get; set; }
        public ICommand AddToBillCommand { get; set; }
        public ICommand DecreaseQuantityCommand { get; set; }


        private void handleInsertIntoDB()
        {
            try
            {
                using (var context = new TAHCoffeeEntities())
                {
                    var bill = new Bill()
                    {
                        Total = Globals.Instance.CurrBill.Total,
                        ExportTime = DateTime.Now
                    };
                    context.Bills.Add(bill);
                    context.SaveChanges();
                    int id = Globals.Instance.CurrBill.Id = bill.IdNumber;


                    List<BillInfor> billInfors = new List<BillInfor>();
                    foreach (var product in Globals.Instance.CurrBill.ProductList)
                    {
                        billInfors.Add(new BillInfor()
                        {
                            IdNumber = id,
                            ItemId = product.Key.Id,
                            Quantity = product.Value,
                            Price = product.Value * product.Key.Price

                        });
                    }
                    context.BillInfors.AddRange(billInfors);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private void handleCheckoutCommand(object p)
        {
            handleInsertIntoDB();
            Globals.Instance.OrderQueue.Add(Globals.Instance.CurrBill);
            Globals.Instance.CurrBill.Clear();
        }
        public ICommand CheckoutCommand { get; set; }

        public ICommand IncreaseQuantityCommand { get; set; }
        public ICommand ViewAll { get; set; }
        public ICommand ClearBillCommand { get; set; }
        public HomeViewModel()
        {
            int ChosenCategoryID = -1;// Determine which CategoryID is chosen to be shown
            CheckoutCommand = new RelayCommand<object>(p => !Globals.Instance.CurrBill.isEmpty(), p => handleCheckoutCommand(p));
            ViewAll = new RelayCommand<object>(p => true, p => categorizedItemList = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.ToList()));
            AddToBillCommand = new RelayCommand<object>((p) => true, (p) => Globals.Instance.Insert(p as Item));
            DecreaseQuantityCommand = new RelayCommand<object>((p) => true, p => Globals.Instance.Delete((p as Product).Key));
            IncreaseQuantityCommand = new RelayCommand<object>((p) => true, p => Globals.Instance.Insert((p as Product).Key));
            CategoryList = new ObservableCollection<Category>(DataProvider.Ins.DB.Categories.ToList());
            categorizedItemList = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.ToList());
            CategoryChangeCommand = new RelayCommand<Category>((p) => true, (p) =>
            {
                ChosenCategoryID = p.Id;
                categorizedItemList = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.Where(Cond => Cond.CategoryId == ChosenCategoryID).ToList());
            });
            ClearBillCommand = new RelayCommand<object>(p=>true, p=> { Globals.Instance.CurrBill.Clear(); });  
        }
    }
}
