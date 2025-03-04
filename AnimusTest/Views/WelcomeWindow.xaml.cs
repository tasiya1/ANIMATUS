using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            MessageBox.Show("Open project message!");
        }

        public void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
