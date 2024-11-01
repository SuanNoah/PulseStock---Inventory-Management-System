using PulseStock___Inventory_Management_System.Classes.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using PulseStock___Inventory_Management_System.Classes.Parent;

namespace PulseStock___Inventory_Management_System.Classes.Functions
{
    internal class StocksManagement : Data, IStocksManipulation
    {
        
        public StocksManagement(string userFile)
        {
            this.userfile = userFile;
        }
        public void StocksStart()
        {
            Console.Title = "PulseStock - Inventory Manager";
            StockMenu();
        }

        private void StockMenu()
        {
            Prompt = "        Inventory Manager";// Add search method
            string[] options = { "Add Stock", "Count Stocks", "Modify Stock", "Remove Stock", "Make a Transaction","Account Settings","Log Out"};
            StockMenu stockMenu = new StockMenu(Prompt, options, userfile);
            bool loop = true;

            do 
            {
                int index = stockMenu.Run();

                switch (index) 
                {
                    case 0:
                        AddStock();
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        loop = false;
                        break;
                }
            }while (loop);
        }
        public void AddStock()
        {
            Console.CursorVisible = true;
            Console.Write("Enter Product ID: ");
            string stockID = Console.ReadLine();
            Console.Write("Enter Product Name: ");
            string stockName = Console.ReadLine();
            Console.Write("Enter Product Price: ");
            string stockPrice = Console.ReadLine();
            Console.Write("Enter Product MFG Date(DD/MM/YYYY): ");
            string stockMFG = Console.ReadLine();
            Console.Write("Enter Product EXP Date(DD/MM/YYYY): ");
            string stockEXP = Console.ReadLine();

            using (StreamWriter stockWrite = File.AppendText(userfile)) 
            {
                stockWrite.WriteLine($"{stockID},{stockName},{stockPrice},{stockMFG},{stockEXP}");
            }
            Console.Write($"{stockID} {stockName} - succesfully added to the database.");
            Console.ReadKey();
        }

        public void DisplayStock()
        {
            string[] lines = File.ReadAllLines(userfile);
            string[] headers = lines[0].Split(',');
            var table = new Table(); //using the spectre console we are creating a table for GUI
            table.Border = TableBorder.Square;
            table.ShowRowSeparators = true;
            table.Alignment(Justify.Right);
            foreach (string header in headers) //Add built in header for the table
            {
                table.AddColumn($"[green]{header}[/]");
            }
            foreach (string row in lines.Skip(1)) //Skipping the first line so that the program can add the stock details 
            {
                string[] fields = row.Split(',');
                table.AddRow($"{fields[0]}", $"{fields[1]}", $"{fields[2]}", $"{fields[3]}", $"{fields[4]}");
            }
            AnsiConsole.Write(table);
        }

        public void ModifyStock()
        {
            throw new NotImplementedException();
        }

        public void RemoveStock()
        {
            throw new NotImplementedException();
        }


        public void Transaction()
        {
            throw new NotImplementedException();
        }
        public void CountStock()
        {
            throw new NotImplementedException();
        }

        public void AccountSetting()
        {
            AccountManager accountSettings = new AccountManager();
            accountSettings.DeleteUser();
        }
    }
}
