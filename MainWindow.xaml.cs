﻿
using System.IO;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Forms;

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

        private void Open(object sender, RoutedEventArgs e) 
        {
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

            //recursive calls to create subtrees and leafs
            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                root.Items.Add(CreateTreeDirectory(subDirectory));
            }
            foreach(FileInfo file in directory.GetFiles())
            {
                root.Items.Add(CreateTreeFile(file));
            } 
            //adds selection listener to print rahs in the status bar 
            root.Selected += new RoutedEventHandler(RefreshStatusBar);
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
            item.Selected += new RoutedEventHandler(RefreshStatusBar);
            return item; 
        }

        //creates some file inside the directory
        private void ContextItemCreate(object sender, RoutedEventArgs a)
        {
            TreeViewItem mainDirectory = (TreeViewItem)treeView.SelectedItem;
            string path = (string)mainDirectory.Tag;
            CreateFileWindow createFile = new CreateFileWindow(path); // creates dialog window for creating files
            createFile.ShowDialog();

            if(createFile.Succeeded())
            {
                if(File.Exists(createFile.GetPath()))
                {
                    FileInfo file = new FileInfo(createFile.GetPath());
                    mainDirectory.Items.Add(CreateTreeFile(file));
                }
                else if(Directory.Exists(createFile.GetPath()))
                {
                    DirectoryInfo directory = new DirectoryInfo(createFile.GetPath());
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
                //recursive method to delete the whole directory
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
            TextBlock textBlock = new TextBlock() 
            { 
                Text = content, 
                TextWrapping = TextWrapping.Wrap 
            };
            scrollViewer.Content = textBlock;
        }
        private void deleteDirectory(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            foreach(var subdirectory in directory.GetDirectories())
            {
                deleteDirectory(subdirectory.FullName);
            }
            foreach(var file in directory.GetFiles())
            {
                File.Delete(file.FullName); 
            }
            Directory.Delete(path);
        }
        private void RefreshStatusBar(object sender, RoutedEventArgs a)
        {
            TreeViewItem item = (TreeViewItem)treeView.SelectedItem;
            status.Text = "";
            FileAttributes rahs = File.GetAttributes((string)item.Tag);

            if((rahs & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                status.Text += 'r';
            }
            else
            {
                status.Text += '-';
            }            
            if((rahs & FileAttributes.Archive) == FileAttributes.Archive)
            {
                status.Text += 'a';
            }
            else
            {
                status.Text += '-';
            }            
            if((rahs & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                status.Text += 'h';
            }
            else
            {
                status.Text += '-';
            }            
            if((rahs & FileAttributes.System) == FileAttributes.System)
            {
                status.Text += 's';
            }
            else
            {
                status.Text += '-';
            }
            
        }
        private void Close(object sender, RoutedEventArgs a) 
        {
            Close();
        }       
    }
}