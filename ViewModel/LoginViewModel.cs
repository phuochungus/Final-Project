
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
using System.Security.Cryptography;
using _4NH_HAO_Coffee_Shop.View;
using System.Data.Entity;

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; set; }
        TAHCoffeeEntities conn = new TAHCoffeeEntities();
        private Visibility _progressBar { get; set; }
        private Visibility _loginButton { get; set; }
        private Visibility _viewVisible = Visibility.Visible;
        public Visibility ViewVisible
        {
            get => _viewVisible;
            set { _viewVisible = value; OnPropertyChanged(); }

        }
        public Visibility LoginButton
        {
            get => _loginButton;
            set { _loginButton = value; OnPropertyChanged(); }
        }
        public Visibility ProgressBar
        {
            get => _progressBar;
            set { _progressBar = value; OnPropertyChanged(); }
        }

        private string _email { get; set; }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }
        private string _password { get; set; }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }




        public LoginViewModel()
        {
            ProgressBar = Visibility.Hidden;
            LoginButton = Visibility.Visible;
            LoginCommand = new RelayCommand<Window>((p) => { return inputCheck(); }, (p) => { handleLoginButtonPress(p); });
            conn.Database.Connection.Open();
        }

        public bool inputCheck()
        {
            return true;
            /////-----
            //if (!isValidEmail(_email) || _password == null || _password == "") return false;
            //return true;

            //bool isValidEmail(string Email)
            //{
            //    try
            //    {
            //        MailAddress mail = new MailAddress(Email);
            //        return true;
            //    }
            //    catch (Exception)
            //    {
            //        return false;
            //    }
            //};
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
            Email = "nguyenvana@gmail.com";
            Password= "password";
            if (p == null) return;
            ProgressBar = Visibility.Visible;
            LoginButton = Visibility.Hidden;
            
            try
            {
                string EncryptedPassword = CreateMD5(Password);
                Globals.CurrUser = await DataProvider.Ins.DB.Accounts.Where(x => x.Email == Email && x.Password == "password").FirstOrDefaultAsync();
                ProgressBar = Visibility.Hidden;
                LoginButton = Visibility.Visible;
                if (Globals.CurrUser == null)
                {
                    MessageBox.Show("Wrong email or password!");
                }
                else
                {
                    Globals.isAdmin = (Globals.CurrUser.AccountType == "admin");
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
