using _4NH_HAO_Coffee_Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using _4NH_HAO_Coffee_Shop.Utils;
using System.Windows.Forms;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<Category> categories;
        private ObservableCollection<Item> itemsBelongToCurrentCategory;

        public ObservableCollection<Category> categoriesProperty
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Item> itemsBelongToCurrentCategoryProperty
        {
            get => itemsBelongToCurrentCategory;
            set
            {
                itemsBelongToCurrentCategory = value;
                OnPropertyChanged();
            }
        }

        public ICommand CategoryChangeCommand { get; set; }
        public ICommand AddToBillCommand { get; set; }
        public ICommand DecreaseQuantityCommand { get; set; }
        public ICommand CheckoutCommand { get; set; }
        public ICommand IncreaseQuantityCommand { get; set; }
        public ICommand ViewAll { get; set; }
        public ICommand ClearBillCommand { get; set; }

        public HomeViewModel()
        {
            fetchAllCategories();
            fetchAllItems();

            CheckoutCommand = new RelayCommand<object>(parameter => isCurrentNotEmpty(), parameter => handleCheckoutCommand());

            ViewAll = new RelayCommand<object>(parameter => true, parameter => fetchAllItems());

            AddToBillCommand = new RelayCommand<Item>(chosenItem => true, chosenItem => addItemtoBill(chosenItem));

            DecreaseQuantityCommand = new RelayCommand<Product>(chosenProduct => true, chosenProduct => decreaseProductQuantityBy1(chosenProduct));

            IncreaseQuantityCommand = new RelayCommand<Product>(chosenProduct => true, chosenProduct => increaseProductQuantityBy1(chosenProduct));

            CategoryChangeCommand = new RelayCommand<Category>((p) => true, (p) =>
            {
                int ChosenCategoryID = p.Id;
                itemsBelongToCurrentCategoryProperty = new ObservableCollection<Item>
                (
                    DataProvider.Ins.DB.Items
                    .Where(Cond => Cond.CategoryId == ChosenCategoryID)
                    .ToList()
                );
            });

            ClearBillCommand = new RelayCommand<object>(parameter => true, parameter => clearCurrentOrder());
        }


        private void decreaseProductQuantityBy1(Product chosenProduct)
        {
            Item ProductItem = chosenProduct.Item;
            removeItemFromBill(ProductItem);
        }

        private void removeItemFromBill(Item chosenItem)
        {
            Globals.Instance.Delete(chosenItem);
        }

        private void increaseProductQuantityBy1(Product chosenProduct)
        {
            Item ProductItem = chosenProduct.Item;
            addItemtoBill(ProductItem);
        }
        private void addItemtoBill(Item chosenItem)
        {
            Globals.Instance.Insert(chosenItem);
        }

        private bool isCurrentNotEmpty()
        {
            return !Globals.Instance.CurrBill.isEmpty();
        }

        private void fetchAllItems()
        {
            itemsBelongToCurrentCategoryProperty = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.ToList());
        }

        private void fetchAllCategories()
        {
            categoriesProperty = new ObservableCollection<Category>(DataProvider.Ins.DB.Categories.ToList());
        }

        private void handleCheckoutCommand()
        {
            handleInsertIntoDB();
            Globals.Instance.OrderQueue.Add(Globals.Instance.CurrBill);
            Globals.Instance.CurrBill.Clear();
        }

        private void handleInsertIntoDB()
        {
            try
            {
                using (var DB = new TAHCoffeeEntities())
                {
                    var bill = new Bill()
                    {
                        Total = Globals.Instance.CurrBill.Total,
                        ExportTime = DateTime.Now
                    };
                    DB.Bills.Add(bill);
                    DB.SaveChanges();
                    int id = Globals.Instance.CurrBill.Id = bill.IdNumber;


                    List<BillInfor> billInfors = new List<BillInfor>();
                    foreach (var product in Globals.Instance.CurrBill.ProductList)
                    {
                        billInfors.Add(new BillInfor()
                        {
                            IdNumber = id,
                            ItemId = product.Item.Id,
                            Quantity = product.Quantity,
                            Price = product.Quantity * product.Item.Price
                        });
                    }
                    DB.BillInfors.AddRange(billInfors);
                    DB.SaveChanges();
                    var t = DB.MonthlyRevenues.ToList();
                    Console.WriteLine(DB.MonthlyRevenues.ToList());
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Transaction fail!");
            }
        }

        private void clearCurrentOrder()
        {
            Globals.Instance.CurrBill.Clear();
        }

    }
}