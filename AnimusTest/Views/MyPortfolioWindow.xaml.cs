using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimusTest.Views
{
    public partial class MyPortfolioWindow : Window
    {
        public MyPortfolioWindow()
        {
            InitializeComponent();

            InitializeAsync();


        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            WelcomeWindow welcomeWindow = new WelcomeWindow();
            welcomeWindow.Show();
            this.Close();
        }

        async void InitializeAsync()
        {
            await Browser.EnsureCoreWebView2Async(null);
            Browser.Source = new Uri("http://localhost:8080");
        }


    }
}
