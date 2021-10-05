using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteUsers
{    
    public class User
    {

        public string path = null;
        public string nome = null;
        public string regpath = null;

        public string setp //profile image path
        {
            set
            {
                path = value;
                nome = value.Split('\\').Last();
            }
            get => path;
        } //C:\Users\Visual Laser 10 New

        public User(string profileImagePath, string regp)
        {
            setp = profileImagePath;
            regpath = regp;
        }

    }
}
