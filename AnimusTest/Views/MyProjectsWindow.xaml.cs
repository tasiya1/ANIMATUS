using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimusTest.Views
{
    public partial class MyProjectsWindow : Window
    {
        public MyProjectsWindow()
        {
            InitializeComponent();

            InitializeAsync();


        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            WelcomeWindow welcomeWindow = new WelcomeWindow();
            welcomeWindow.Show();
            this.Close();
        }

        async void InitializeAsync()
        {
            await Browser.EnsureCoreWebView2Async(null);
            Browser.Source = new Uri("http://localhost:3000");
        }


    }
}
