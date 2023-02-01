using _4NH_HAO_Coffee_Shop.Model;
using _4NH_HAO_Coffee_Shop.ViewModel;

namespace _4NH_HAO_Coffee_Shop.Utils
{
    /// <summary>
    /// Lớp Global sử dụng kiến trúc Singleton Pattern
    /// Lớp này dùng như một object lưu biến và hàm toàn cục có thể truy cập từ mọi nơi trong chương trình
    /// </summary>
    public class Globals : BaseViewModel
    {
        private Account currUser;
        public Account CurrUser
        {
            get => currUser;
            set
            {
                if (currUser == value) return;
                currUser = value;
                OnPropertyChanged();
            }
        }

        private bool _isAdmin;
        public bool isAdmin
        {
            get => _isAdmin;
            set
            {
                if (_isAdmin == value) return;
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        private Order _currBill;
        public Order CurrBill
        {
            get => _currBill;
            set
            {
                if (_currBill != value)
                {
                    _currBill = value;
                    OnPropertyChanged();
                }
            }
        }

        private OrderList orderQueue;
        public OrderList OrderQueue
        {
            get => orderQueue;
            set
            {
                if (orderQueue == value) return;
                orderQueue = value;
                OnPropertyChanged();
            }
        }

        //Singleton Pattern
        private Globals() 
        {
            CurrBill = new Order();
            OrderQueue = new OrderList();
        }

        private static Globals _instance;

        public static Globals Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Globals();
                return _instance;
            }
        }
        
        //Hàm thực hiện thao tác update trong bill
        //input: item và quantity mới 
        //output: True(Thành công) hoặc False(Thất bại)
        public bool Update(Item item, int quantity)
        {
            //Xác định item tồn tại trong bill hay không
            for (int i = 0; i < CurrBill.ProductList.Count; ++i)
            {
                if (CurrBill.ProductList[i].Item.Id == item.Id)
                {
                    //Nếu tồn tại thực hiện cập nhật
                    CurrBill.Total += (quantity - CurrBill.ProductList[i].Quantity);
                    CurrBill.ProductList[i].Quantity = quantity;

                    //Nếu quantity = 0  tức là số lượng sản phẩm của item = 0 trong bill thì xóa khỏi bill
                    if (quantity == 0) 
                        CurrBill.ProductList.RemoveAt(i);
                    Instance.OnPropertyChanged();
                    
                    return true;
                }
            }
            //Nếu không tồn tại thì update thất bại
            return false;
        }

        //Hàm thực hiện thao tác insert trong bill
        //input: item
        //output: True(Thành công) hoặc False(Thất bại)
        public bool Insert(Item item)
        {
            CurrBill.Total += item.Price;
            //Xác định item tồn tại trong bill hay không
            for (int i = 0; i < CurrBill.ProductList.Count; ++i)
            {
                if (CurrBill.ProductList[i].Item.Id == item.Id)
                {
                    //Nếu tồn tại thực hiện tăng 
                    CurrBill.ProductList[i].Quantity++;
                    Instance.OnPropertyChanged();
                    return true;
                }
            }
            //Nếu không tồn tại thì insert vào  
            CurrBill.ProductList.Add(new Product(item, 1));
            Instance.OnPropertyChanged();
            return true;
        }

        //Hàm thực hiện thao tác delete trong bill
        //input: item
        //output: True(Thành công) hoặc False(Thất bại)
        public bool Delete(Item item)
        {
            //Xác định item tồn tại trong bill hay không
            for (int i = 0; i < CurrBill.ProductList.Count; ++i)
            {
                if (CurrBill.ProductList[i].Item.Id == item.Id)
                {
                    //Nếu tồn tại thực hiện xóa
                    CurrBill.Total -= item.Price;
                    CurrBill.ProductList[i].Quantity--;
                    Instance.OnPropertyChanged();
                    return true;
                }
            }            
            //Nếu không tồn tại thì delete thất bại
            return false;
        }
    }
}