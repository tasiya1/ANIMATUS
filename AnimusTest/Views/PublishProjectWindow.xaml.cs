using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnimusTest.Controls;
using AnimusTest.Models;

namespace AnimusTest.Views
{
    /// <summary>
    /// Interaction logic for PublishProjectWindow.xaml
    /// </summary>
    public partial class PublishProjectWindow : Window
    {

        private Project project;

        public PublishProjectWindow(Project project)
        {
            InitializeComponent();

            this.project = project;
        }

        public async void PublishProject_Click(object sender, RoutedEventArgs e)
        {
            if (project.Frames.Count == 0)
            {
                MessageBox.Show("Nothing to publish.");
                return;
            }

            project.Title = TitleTextBox.Text;
            project.Description = DescriptionTextBox.Text;

            await FileController.PublishProjectAsync(project);
        }
    }
}
