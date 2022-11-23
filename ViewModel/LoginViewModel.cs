
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using MaterialDesignThemes.Wpf;
using _4NH_HAO_Coffee_Shop.Model;
using System.Net.Mail;
using System.Data.Entity;
using System.Security.Cryptography;
using _4NH_HAO_Coffee_Shop.View;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; set; }

        public bool IsLoggedIn { get; set; } = false;
        private Visibility ProgressBar { get; set; }
        private Visibility LoginButton { get; set; }
        public Visibility _LoginButton
        {
            get => LoginButton;
            set
            {
                LoginButton = value;
                OnPropertyChanged();
            }
        }
        public Visibility _ProgressBar
        {
            get => ProgressBar;
            set
            {
                ProgressBar = value;
                OnPropertyChanged();
            }
        }

        private string Email { get; set; }
        public string _Email
        {
            get => Email;
            set
            {
                Email = value;
                OnPropertyChanged();
            }
        }
        private string Password { get; set; }
        public string _Password
        {
            get => Password;
            set
            {
                Password = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel()
        {
            _ProgressBar = Visibility.Hidden;
            _LoginButton = Visibility.Visible;
            LoginCommand = new RelayCommand<Window>((p) => { return inputCheck(); }, (p) => { handleLoginButtonPress(p); });
        }

        public bool inputCheck()
        {
            return true;
            /////-----
            if (!isValidEmail(Email) || Password == null || Password == "") return false;
            return true;

            bool isValidEmail(string Email)
            {
                try
                {
                    MailAddress mail = new MailAddress(Email);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            };
        }
        public string CreateMD5(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            string encoded = BitConverter.ToString(hash)
               .Replace("-", string.Empty)
               .ToLower();
            return encoded;
        }


        public async void handleLoginButtonPress(Window p)
        {
            BaseViewModel HomeWindow = new HomeViewModel();
            p.Close();
            return;
            ////-------------
            if (p == null) return;
            _ProgressBar = Visibility.Visible;
            _LoginButton = Visibility.Hidden;
            try
            {
                string EncryptedPassword = CreateMD5(_Password);
                Globals.CurrUser = await DataProvider.Ins.DB.Accounts.Where(x => x.Email == _Email && x.Password == EncryptedPassword).FirstOrDefaultAsync();
                _ProgressBar = Visibility.Hidden;
                _LoginButton = Visibility.Visible;
                if (Globals.CurrUser == null)
                {
                    MessageBox.Show("Wrong email or password!");
                }
                else
                {
                    Globals.isAdmin = (Globals.CurrUser.AccountType == "admin");
                    BaseViewModel HomeView = new HomeViewModel();
                    p.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
