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
            treeView.Items.Add(root);// add the created hierarchy to the tree element in GUI
        
        
        }

        private TreeViewItem CreateTreeDirectory(DirectoryInfo directory)
        {
            var root = new TreeViewItem
            {
                Header = directory.Name,
                Tag = directory.FullName
            };

            foreach(DirectoryInfo subDirectory in directory.GetDirectories())
            {
                root.Items.Add(CreateTreeDirectory(subDirectory));
            }
            foreach(FileInfo file in directory.GetFiles())
            {
                root.Items.Add(CreateTreeFile(file));
            }
            return root;
        }

        private TreeViewItem CreateTreeFile(FileInfo file)
        {
            var item = new TreeViewItem
            {
                Header = file.Name,
                Tag = file.FullName
            };

            return item;
        }
        private void Close(object sender, RoutedEventArgs e) 
        {
            Close();
        }       
    }
}
