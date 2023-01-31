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

namespace _4NH_HAO_Coffee_Shop.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
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
                {
                    MessageBox.Show("Wrong email or password!");
                }
                else
                {
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