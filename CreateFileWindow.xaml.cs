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
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;
using System.Windows.Navigation;
using MenuItem = System.Windows.Controls.MenuItem;
using System.Text.RegularExpressions;



namespace lab2_WPF_app
{
    /// <summary>
    /// Logika interakcji dla klasy CreateFileWindow.xaml
    /// </summary>

    public partial class CreateFileWindow : Window
    {
        private string path;
        private bool success;
        private string name;
        public CreateFileWindow(string path)
        {
            InitializeComponent();
            this.path = path;
            success = false;
        }

        public void OK_Button(object sender, RoutedEventArgs a)
        {
            bool isDirectory = (bool)radioDirectory.IsChecked;
            bool isFile = (bool)radioDirectory.IsChecked;

            if (isFile && !Regex.IsMatch(CreateFileWindowName.Text, "^[a-zA-Z0-9-~_]{1,8}\\.(html|php|txt)$"))
            {
                System.Windows.MessageBox.Show("Incorrect name.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else if (!isFile && !isDirectory)
            {
                System.Windows.MessageBox.Show("Please choose over File or Directory", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                Name = CreateFileWindowName.Text;
                path = path + "\\" + Name;
                FileAttributes attributes = FileAttributes.Normal;//assigns default mask
                if ((bool)checkBoxR.IsChecked)
                {
                    attributes |= FileAttributes.ReadOnly;
                }
                if ((bool)checkBoxA.IsChecked)
                {
                    attributes |= FileAttributes.Archive;
                }
                if ((bool)checkBoxH.IsChecked)
                {
                    attributes |= FileAttributes.Hidden;
                }
                if ((bool)checkBoxS.IsChecked)
                {
                    attributes |= FileAttributes.System;
                }
                if (isFile)
                {
                    File.Create(path);
                }
                else if (isDirectory)
                {
                    Directory.CreateDirectory(path);
                }
                File.SetAttributes(path, attributes);
                success = true;
                Close();
            }
        }

        public bool Succeeded()
        { 
            return success;
        }

        private void Cancel_Button(object sender, RoutedEventArgs a)
        {
            Close();
        }

        public string GetPath()
        {
            return path;
        }


    }
}
