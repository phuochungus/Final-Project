<<<<<<< HEAD

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
=======
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using _4NH_HAO_Coffee_Shop.Model;
using System.Security.Cryptography;
using System.Data.Entity;
using _4NH_HAO_Coffee_Shop.Utils;
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
<<<<<<< HEAD
        public ICommand LoginCommand { get; set; }
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
        }

        public bool inputCheck()
        {
            return true;
            /////-----
=======
        private Visibility progressCircleVisibility { get; set; }
        private Visibility loginButtonVisibility { get; set; }
        private string password { get; set; }
        private string email { get; set; }

        public ICommand LoginCommand { get; set; }

        public Visibility loginButtonVisibilityProperty
        {
            get => loginButtonVisibility;
            set { loginButtonVisibility = value; OnPropertyChanged(); }
        }
        public Visibility progressCircleVisibilityProperty
        {
            get => progressCircleVisibility;
            set
            {
                progressCircleVisibility = value;
                OnPropertyChanged();
            }
        }
        public string emailProperty
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }
        public string passwordProperty
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel()
        {
            progressCircleVisibilityProperty = Visibility.Hidden;
            loginButtonVisibilityProperty = Visibility.Visible;
            LoginCommand = new RelayCommand<Window>(loginWindow => isEmailValidated(), loginWindow => loginFromCurrentWindow(loginWindow));
        }

        public bool isEmailValidated()
        {

            return true;


            // CODE COMMENTED FOR DEVELPOPMENT PURPOSE
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
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
<<<<<<< HEAD
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
            try
            {
                Email = "nguyenvana@gmail.com";
                Password = "password";
                if (p == null) return;
                ProgressBar = Visibility.Visible;
                LoginButton = Visibility.Hidden;
                string EncryptedPassword = CreateMD5(Password);
                using (var conn = new TAHCoffeeEntities())
                {
                    Globals.CurrUser = await conn.Accounts.Where(x => x.Email == Email && x.Password == EncryptedPassword).FirstOrDefaultAsync<Account>();
                }
                ProgressBar = Visibility.Hidden;
                LoginButton = Visibility.Visible;
                if (Globals.CurrUser == null)
=======

        private void showProgressCircle()
        {
            progressCircleVisibilityProperty = Visibility.Visible;
            loginButtonVisibilityProperty = Visibility.Hidden;
        }

        private void hideProgressCircle()
        {
            progressCircleVisibilityProperty = Visibility.Hidden;
            loginButtonVisibilityProperty = Visibility.Visible;
        }

        public async void loginFromCurrentWindow(Window loginWindow)
        {
            try
            {
                emailProperty = "nguyenthib@gmail.com";
                passwordProperty = "password";

                showProgressCircle();
                string EncryptedPassword = CreateMD5(passwordProperty);
                Globals.Instance.CurrUser = await DataProvider.Ins
                    .DB.Accounts
                    .Where(x => x.Email == emailProperty && x.Password == EncryptedPassword)
                    .FirstOrDefaultAsync();
                hideProgressCircle();

                if (Globals.Instance.CurrUser == null)
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
                {
                    MessageBox.Show("Wrong email or password!");
                }
                else
                {
<<<<<<< HEAD
                    Globals.isAdmin = (Globals.CurrUser.AccountType == "admin"); ;
                    p.Hide();
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
=======
                    Globals.Instance.isAdmin = (Globals.Instance.CurrUser.AccountType == "admin"); ;
                    loginWindow.Hide();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Login fail!");
            }
        }

        public string CreateMD5(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            return encoded;
        }
    }
}
>>>>>>> 911f89f27d58cdff66191ef34e0b40255ca4413d
