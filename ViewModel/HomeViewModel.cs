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
            //Lấy toàn bộ data
            fetchAllCategories();
            fetchAllItems();

            //Thực hiện checkout
            CheckoutCommand = new RelayCommand<object>(parameter => isCurrentBillNotEmpty(), parameter => handleCheckoutCommand());
            

            //Một số command thực hiện chức năng cơ bản của menu
            //Xem toàn bộ menu
            ViewAll = new RelayCommand<object>(parameter => true, parameter => fetchAllItems());
            //Thêm món vào menu
            AddToBillCommand = new RelayCommand<Item>(chosenItem => true, chosenItem => addItemtoBill(chosenItem));
            //Giảm số lượng món trong menu đi 1
            DecreaseQuantityCommand = new RelayCommand<Product>(chosenProduct => true, chosenProduct => decreaseProductQuantityBy1(chosenProduct));
            //Tăng số luộng món trong menu lên 1
            IncreaseQuantityCommand = new RelayCommand<Product>(chosenProduct => true, chosenProduct => increaseProductQuantityBy1(chosenProduct));
            //Lọc các món theo Category
            CategoryChangeCommand = new RelayCommand<Category>((p) => true, (p) =>
            {
                int ChosenCategoryID = p.Id;
                itemsBelongToCurrentCategoryProperty = new ObservableCollection<Item>(
                    DataProvider.Ins.DB.Items.Where(Cond => Cond.CategoryId == ChosenCategoryID).ToList()
                );
            });
            //Xóa bill hiện tại
            ClearBillCommand = new RelayCommand<object>(parameter => true, parameter => clearCurrentOrder());
        }

        private void fetchAllCategories()
        {
            categoriesProperty = new ObservableCollection<Category>(DataProvider.Ins.DB.Categories.ToList());
        }

        private void fetchAllItems()
        {
            itemsBelongToCurrentCategoryProperty = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.ToList());
        }

        private bool isCurrentBillNotEmpty()
        {
            return !Globals.Instance.CurrBill.isEmpty();
        }

        private void handleCheckoutCommand()
        {
            insertTransactionDetailToDatabse();
            //Thêm vào hàng chờ phục vụ
            Globals.Instance.OrderQueue.Add(Globals.Instance.CurrBill);
            //xóa Bill hiện tại 
            Globals.Instance.CurrBill.Clear();
        }

        private void insertTransactionDetailToDatabse()
        {
            try
            {
                using (var DB = new TAHCoffeeEntities())
                {
                    //Lưu thông tin cơ bản của bill vào database
                    Bill bill = new Bill()
                    {
                        Total = Globals.Instance.CurrBill.Total,
                        ExportTime = DateTime.Now
                    };
                    DB.Bills.Add(bill);
                    DB.SaveChanges();

                    //Lấy Id của bill do database trả về
                    int id = Globals.Instance.CurrBill.Id = bill.IdNumber;

                    //Lưu thông tin chi tiết bao gồm từng mặt hàng, số lượng, Id trên bill vào database
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
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Transaction fail!");
            }
        }

        private void addItemtoBill(Item chosenItem)
        {
            Globals.Instance.Insert(chosenItem);
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

        private void clearCurrentOrder()
        {
            Globals.Instance.CurrBill.Clear();
        }

    }
}