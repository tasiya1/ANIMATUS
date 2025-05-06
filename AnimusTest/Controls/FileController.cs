using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using AnimusTest.Models;
using Formatting = Newtonsoft.Json.Formatting;
using Newtonsoft.Json;
using System.Windows;

namespace AnimusTest.Controls
{
    public class FileController
    {
        private string fileExtension = "aiau";
        public FileController() { }

        public Timeline OpenFile()
        {

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "моя онямація";
            dialog.DefaultExt = $".{fileExtension}";
            dialog.Filter = $"Text documents (.{fileExtension})|*.{fileExtension}";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                string json = File.ReadAllText(filename);
                return JsonConvert.DeserializeObject<Timeline>(json);
            }
            else
            {
                MessageBox.Show("Could not open project");
                return null;
            }


}

        public void SaveProjectToFile(Timeline project)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "моя оняиація"; 
            dialog.DefaultExt = $".{fileExtension}"; 
            dialog.Filter = $"Text documents (.{fileExtension})|*.{fileExtension}"; 

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                string json = JsonConvert.SerializeObject(project, Formatting.Indented);
                File.WriteAllText(filename, json);
            } else
            {
                MessageBox.Show("Project was not saved");
            }

            
        }
    }
}
