
using System.Windows;

using System.IO;

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
            bool isFile = (bool)radioFile.IsChecked;

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
                name = CreateFileWindowName.Text;
                path = path + "\\" + name;
                FileAttributes rahs = FileAttributes.Normal;//assigns default mask
                if ((bool)checkBoxR.IsChecked)
                {
                    rahs |= FileAttributes.ReadOnly;
                }
                if ((bool)checkBoxA.IsChecked)
                {
                    rahs |= FileAttributes.Archive;
                }
                if ((bool)checkBoxH.IsChecked)
                {
                    rahs |= FileAttributes.Hidden;
                }
                if ((bool)checkBoxS.IsChecked)
                {
                    rahs |= FileAttributes.System;
                }
                if (isFile)
                {
                    File.Create(path);
                }
                else if (isDirectory)
                {
                    Directory.CreateDirectory(path);
                }
                File.SetAttributes(path, rahs);
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