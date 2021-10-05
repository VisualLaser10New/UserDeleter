using Microsoft.Win32;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeleteUsers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //get the users list
        public static Tool tools = new Tool();
        List<User> usersList = new List<User>();

        

        public MainWindow()
        {
            InitializeComponent();
            seekBTN_Click(new object(), new RoutedEventArgs());
        }
        
        private void seekBTN_Click(object sender, RoutedEventArgs e)
        {
            //associate usersList to listbox
            usersList.Clear();
            try
            {
                usersList = tools.search_users();
            }
            catch
            {
                Tool.message("Error to get profile list");
            }

            usersLST.ItemsSource = null;
            usersLST.ItemsSource = usersList.Select(a => a.nome).ToList();
        }

        private void delUserBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!usersLST.Items.IsEmpty)
            {
                if (usersLST.SelectedIndex != -1)
                {
                    if(bkpCheckBox.IsChecked==true) //do backup of key
                        Tool.backup();

                    bool err = false;
                    string error_msg = "Error during deleting:";
                    for (int j = 0; j < usersLST.Items.Count; j++)
                    {
                        if (usersLST.SelectedItems.Contains(usersLST.Items[j]))
                        {
                            try
                            {
                                if (Directory.Exists(usersList[j].path))
                                    Directory.Delete(usersList[j].path, true);
                                Registry.LocalMachine.DeleteSubKey(usersList[j].regpath);
                            }
                            catch
                            {
                                err = true;
                                error_msg += " " + usersList[j].nome;

                            }
                        }
                    }

                    if(err)
                        Tool.message(error_msg);
                    //recarge the list
                    seekBTN_Click(sender, e);
                }
                else
                {
                    Tool.message("Please select at least one user");
                }
            }
            else
            {
                Tool.message("Please press Seek button");
            }

            
        }


        //Function to select
        private void selectBTN_Click(object sender, RoutedEventArgs e)
        {
            //select all users
            if (!usersLST.Items.IsEmpty)
                usersLST.SelectAll();
            else
                Tool.message("Please press Seek button");
        }

        private void deselectBTN_Click(object sender, RoutedEventArgs e)
        {
            //deselect all users
            if (!usersLST.Items.IsEmpty)
                usersLST.UnselectAll();
            else
                Tool.message("Please press Seek button");
        }

        private void selectNumberBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!usersLST.Items.IsEmpty)
            {
                usersLST.UnselectAll();
                //select all users where name is a number
                foreach (var i in usersLST.Items)
                {
                    if (Int32.TryParse(i.ToString(), out _))
                    {
                        usersLST.SelectedItems.Add(i);
                    }
                }
            }
            else
                Tool.message("Please press Seek button");
        }

        private void bkpCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (bkpCheckBox.IsChecked == true)
                bkpCheckBox.Content = "Backup (Enabled)";
            else if(bkpCheckBox.IsChecked == false)
                bkpCheckBox.Content = "Backup (Disabled)";
        }
    }   
}
