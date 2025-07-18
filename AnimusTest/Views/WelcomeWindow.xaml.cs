using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnimusTest.Controls;

namespace AnimusTest.Views
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {

            InitializeComponent();

            if (AuthController.isLoggedIn())
            {
                SetWelcomeWindowStateToLoggedIn();
                
            }
        }

        public void NewProject_Click(object sender, EventArgs e)
        {
            IllustratorWindow illustratorWindow = new IllustratorWindow();
            illustratorWindow.Show();
            this.Close();
        }

        public void OpenProject_Click(Object sender, EventArgs e)
        {
            IllustratorWindow illustratorWindow = new IllustratorWindow(); // тут треба продумати, як між двома різними компонентами передавати процес, в цьому випадку - одне вікно запускає процес передачі відкритого файлу іншому вікну
            illustratorWindow.Show();
            this.Close();
        }

        public void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoginButton_Click(object sender, EventArgs e)
        {
            LoginPage loginWindow = new();
            loginWindow.ShowDialog();
            if (AuthController.isLoggedIn())
            {
                SetWelcomeWindowStateToLoggedIn();
            }
            //this.Close();
        }

        public void MyPortfolio_Click(object sender, EventArgs e)
        {
            MyProjectsWindow myPortfolioWindow = new();
            myPortfolioWindow.Show();
            //this.Close();
        }

        private void SetWelcomeWindowStateToLoggedIn()
        {
            WelcomeText.Text = "Welcome back!";
            MyPortfolioButton.Visibility = Visibility.Visible;
        }
    }
}
