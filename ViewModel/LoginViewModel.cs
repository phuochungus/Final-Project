﻿using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using _4NH_HAO_Coffee_Shop.Model;
using System.Security.Cryptography;
using System.Data.Entity;
using _4NH_HAO_Coffee_Shop.Utils;
using System.Net.Mail;

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

            //Thực hiện login khi nhấn nút login
            LoginCommand = new RelayCommand<Window>(loginWindow => isValidAccount(), loginWindow => loginFromCurrentWindow(loginWindow));
        }


        public bool isValidAccount()
        {
            if (isValidEmail(email) == false || password == null || password == "")
                return false;
            else
                return true;
        }

        private bool isValidEmail(string Email)
        {
            try
            {
                MailAddress mail = new MailAddress(Email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async void loginFromCurrentWindow(Window loginWindow)
        {
            try
            {
                showProgressCircle();
                //Mã hóa password
                string EncryptedPassword = CreateMD5(passwordProperty);
                //Tìm tài khoản
                Globals.Instance.CurrUser = await DataProvider.Ins
                    .DB.Accounts
                    .Where(x => x.Email == emailProperty && x.Password == EncryptedPassword)
                    .FirstOrDefaultAsync();
                hideProgressCircle();

                if (Globals.Instance.CurrUser == null)
                {
                    //Không tìm được
                    MessageBox.Show("Wrong email or password!");
                }
                else
                {
                    //Tìm được
                    Globals.Instance.isAdmin = (Globals.Instance.CurrUser.AccountType == "admin"); ;
                    loginWindow.Hide();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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

    }
}