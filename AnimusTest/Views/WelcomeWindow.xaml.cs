using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AnimusTest.Controls;

namespace AnimusTest.Views
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow() {

            InitializeComponent();

        }

        public void NewProject_Click(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public void OpenProject_Click(Object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); // тут треба продумати, як між двома різними компонентами передавати процес, в цьому випадку - одне вікно запускає процес передачі відкритого файлу іншому вікну
            mainWindow.Show();
            this.Close();
        }

        public void NewIllustration_Click(Object sender, EventArgs e)
        {
            IllustratorWindow illustratorWindow = new IllustratorWindow();
            illustratorWindow.Show();
            this.Close();
        }

        public void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void MyPortfolio_Click(object sender, EventArgs e)
        {
            MyPortfolioWindow myPortfolioWindow = new MyPortfolioWindow();
            myPortfolioWindow.Show();
            this.Close();
        }
    }
}
