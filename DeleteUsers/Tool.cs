using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeleteUsers
{
    public class Tool
    {
        const string regPathInitial = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";
        public List<User> search_users()
        {
            List<User> userList = new List<User>();

            //get users profile list from re
            RegistryKey key = Registry.LocalMachine.OpenSubKey(regPathInitial);
            if (key != null)
            {
                foreach (var keyUser in key.GetSubKeyNames())
                {                    
                    //get user information from registry
                    RegistryKey userInfo = Registry.LocalMachine.OpenSubKey(regPathInitial + "\\" + keyUser);
                    string profile = userInfo.GetValue("ProfileImagePath").ToString();
                    if (Possible(profile))
                    {
                        //add user to list
                        userList.Add(new User(profile, regPathInitial + "\\" + keyUser));
                        userInfo.Close();
                    }
                }
            }
            key.Close();
            return userList;
        }

        public static void message(string input)
        {
            MessageBox.Show(input, "Users Deleter", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void backup()
        {
            uint a = 0;
            while (File.Exists($"backupProfileList{a}.reg"))
            { 
                a++;
            }


            using(Process proc = new Process())
            {
                proc.StartInfo.FileName = "reg.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.Arguments = $"export \"HKLM\\{regPathInitial}\" .\\backupProfileList{a}.reg";
                proc.Start();
                proc.WaitForExit();
            }
        }

        private bool Possible(string input)
        {
            string[] deined = { "adm", "administrator", "admin", "fuligni", "zanzola", "default", 
                "public", "systemprofile", "localservice", "networkservice" };
            foreach (var test in deined)
            {
                if (test.Equals(input.Split('\\').Last(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }            
            return true;
        }
    }
}