using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnimusTest.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnimusTest.Views
{
    public partial class LoginPage : Window
    {

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordTextBox.Password;

            var loginSuccess = await AuthController.Login(username, password);

            if (loginSuccess)
            {
                MessageBox.Show("Вхід успішний!");
            }
            else
            {
                MessageBox.Show("Вхід не вдався 😔");
            }
        }

        
    }
}
