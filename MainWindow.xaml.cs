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

            root.ContextMenu = new System.Windows.Controls.ContextMenu();//creates context menu when RMB clicked
            var contextItem1 = new MenuItem { Header = "Create" };//creates button element
            contextItem1.Click += new RoutedEventHandler(ContextItemCreate);// assigns proper action to the button
            var contextItem2 = new MenuItem { Header = "Delete" };
            contextItem2.Click += new RoutedEventHandler(ContextItemDelete);
            root.ContextMenu.Items.Add(contextItem1);// adds Create and Delete buttons to context menu of tree's leaf
            root.ContextMenu.Items.Add(contextItem2);

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
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

            item.ContextMenu = new System.Windows.Controls.ContextMenu();
            var contextItem1 = new MenuItem { Header = "Open" };
            contextItem1.Click += new RoutedEventHandler(ContextItemOpen);
            var contextItem2 = new MenuItem { Header = "Delete" };
            contextItem2.Click += new RoutedEventHandler(ContextItemDelete);
            item.ContextMenu.Items.Add(contextItem1);
            item.ContextMenu.Items.Add(contextItem2);
            //item.Selected += new RoutedEventHandler(StatusBarUpdate);
            return item; 
        }

        //creates some file inside the directory
        private void ContextItemCreate(object sender, RoutedEventArgs a)
        {
            TreeViewItem mainDirectory = (TreeViewItem)treeView.SelectedItem;
            string path = (string)mainDirectory.Tag;
            CreateFileWindow dialog = new CreateFileWindow(path); // creates dialog window for creating files
            dialog.ShowDialog();

            if(dialog.Succeeded())
            {
                if(File.Exists(dialog.GetPath()))
                {
                    FileInfo file = new FileInfo(dialog.GetPath());
                    mainDirectory.Items.Add(CreateTreeFile(file));
                }
                else if(Directory.Exists(dialog.GetPath()))
                {
                    DirectoryInfo directory = new DirectoryInfo(dialog.GetPath());
                    mainDirectory.Items.Add(CreateTreeDirectory(directory));
                }
            }
        }

        private void ContextItemDelete(object sender, RoutedEventArgs a)
        {
            TreeViewItem item = (TreeViewItem)treeView.SelectedItem;
            string path = (string)item.Tag;
            FileAttributes attributes = File.GetAttributes(path);
            File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);

            if((attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                deleteDirectory(path);
            }
            else
            {
                File.Delete(path);
            }

            if( treeView.Items[0] != item)
            {
                TreeViewItem parent = (TreeViewItem)item.Parent;
                parent.Items.Remove(item);
            }
            else
            {
                treeView.Items.Clear();
            }
        }
        private void ContextItemOpen(object sender, RoutedEventArgs a)
        {
            TreeViewItem item = (TreeViewItem)treeView.SelectedItem;
            string content = File.ReadAllText(((string)item.Tag));
            //scrollViewer.Content = new TextBlock() { Text = content };
        }

        private void deleteDirectory(string path)
        {

        }
        private void Close(object sender, RoutedEventArgs e) 
        {
            Close();
        }       
    }

   

}