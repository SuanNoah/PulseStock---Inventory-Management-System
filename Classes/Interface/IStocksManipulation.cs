using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseStock___Inventory_Management_System.Classes.Interface
{
    internal interface IStocksManipulation
    {
        public void DisplayStock();
        public void AddStock();
        public void RemoveStock();
        public void ModifyStock();
        public void CountStock();
        public void Transaction();
        public void AccountSetting();
        public void DeleteUser(string dir, string userFile, string username);
        public void SearchProduct();
    }
}
