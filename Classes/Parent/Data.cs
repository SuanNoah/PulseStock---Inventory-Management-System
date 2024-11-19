using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseStock___Inventory_Management_System.Classes.Parent
{
    internal class Data
    {
        public string? Prompt { get; set; }
        public string[]? Options { get; set; }
        public int? Index { get; set; }
        public const string folder = "Accounts";
        public const string filename = "list_of_accounts.txt";
        public string accountListLocation;
        public string? Username;
        public string? Password;
        public string[] input = new string[2];
        public string userStockList;
        public string userSales;
        public bool status;

    }
}
