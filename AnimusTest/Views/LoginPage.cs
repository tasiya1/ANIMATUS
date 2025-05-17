using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace AnimusTest.Views
{
    public partial class LoginPage : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public LoginPage()
        {
            InitializeComponent();
        }

        public void LoginButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
