using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseStock___Inventory_Management_System.Classes.Interface
{
    internal interface IAccountManipulation
    {
        public void CreateUser();
        public void LogIn();
        public bool CheckUser(string username);
        public string InputNotNull(string prompt, bool password);
        public string ReadPassword();
        public void DeleteUser();
    }
}
