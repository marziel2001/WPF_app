using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MenuItem = System.Windows.Controls.MenuItem;

namespace lab2_WPF_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open(object sender, RoutedEventArgs e) {

            var dlg = new FolderBrowserDialog()
            {
                Description = "Select directory to open"
            };

            DialogResult answer = dlg.ShowDialog();

            treeView.Items.Clear();
            DirectoryInfo directory = new DirectoryInfo(dlg.SelectedPath);
            var root = CreateTreeDirectory(directory);
            treeView.Items.Add(root);
            
        }

        private TreeViewItem CreateTreeDirectory(DirectoryInfo directory)
        {
            
        }
        private void Close(object sender, RoutedEventArgs e) 
        {
            Close();
        }       
    }
}
