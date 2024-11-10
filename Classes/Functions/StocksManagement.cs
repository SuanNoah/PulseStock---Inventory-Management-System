using PulseStock___Inventory_Management_System.Classes.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using PulseStock___Inventory_Management_System.Classes.Parent;
using System.Xml.Linq;

namespace PulseStock___Inventory_Management_System.Classes.Functions
{
    internal class StocksManagement : Data, IStocksManipulation
    {
        public StocksManagement(string userFile, string username)
        {
            userfile = userFile;
            Username = username;
        }

        public StocksManagement(string userFile)
        {
            userfile = userFile;
        }
        public void StocksStart()
        {
            Console.Title = "PulseStock - Inventory Manager";
            StockMenu();
        }

        private void StockMenu()
        {
            Prompt = "Inventory Manager";
            string[] options = { " Add Product", " View Product List", " View Product Quantity", " Search for Product", " Modify Product",
                                " Remove Product", " Make a Transaction"," Delete Account"," Log Out"};
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
                        Console.Clear();
                        DisplayStock();
                        Console.Write("Nothing Follows. Press any key to exit.");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        CountStock();
                        break;
                    case 3:
                        SearchProduct();
                        break;
                    case 4:
                        ModifyStock();
                        break;
                    case 5:
                        RemoveStock();
                        break;
                    case 6:
                        break;
                    case 7:
                        AccountSetting();
                        if (status == true) 
                        {
                            loop = false;
                            break;
                        }
                        break;
                    case 8:
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
            //Check if the input is empty, program must return to menu so that the user can choose again
            if (string.IsNullOrEmpty(stockID) || string.IsNullOrEmpty(stockName) || string.IsNullOrEmpty(stockPrice) || string.IsNullOrEmpty(stockMFG) || string.IsNullOrEmpty(stockEXP)) 
            {
                Console.Write("The input must not be empty. Press any key to return.");
                Console.ReadKey();
                return;
            }
            using (StreamWriter stockWrite = File.AppendText(userfile)) 
            {
                stockWrite.WriteLine($"{stockID},{stockName},{stockPrice},{stockMFG},{stockEXP}");
            }
            Console.Write($"{stockID} {stockName} - succesfully added to the database.");
            Console.ReadKey();
        }

        public void DisplayStock()
        {
            Console.WriteLine();
            string[] lines = File.ReadAllLines(userfile);
            string[] headers = lines[0].Split(',');
            var table = new Table(); //using the spectre console we are creating a table for GUI
            table.Border = TableBorder.Square;
            table.ShowRowSeparators = true;
            table.Alignment(Justify.Center);
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
            Console.Clear();
            Table header = new Table();
            header.Border = TableBorder.Square;
            header.Expand();
            header.AddColumn(new TableColumn("[Bold Yellow]Change Product Details[/]").Centered());
            header.AddRow("Please put the Product ID of the product that you want to modify.").Centered();
            AnsiConsole.Write(header);
            DisplayStock();
            Console.ReadKey();

            Console.Clear();
            AnsiConsole.Write(header);
            Console.Write("Enter the Product ID: ");
            string productId  = Console.ReadLine()?.Trim();

            List<string> read = File.ReadAllLines(userfile).ToList();
            var productREAD = read.FirstOrDefault(read => read.StartsWith(productId + ","));

            if (productREAD == null)
            {
                Console.Write("Product not found. Press any key to return.");
                Console.ReadKey();
                return;
            }
            //If successful then split the details of the product
            string[] split = productREAD.Split(',');
            Prompt = "[Bold Yellow]Modify Product Details[/]";
            string[] options = { $" Product ID: {split[0]}", $" Product Name: {split[1]}", $" Price: {split[2]}", $" Product MFG: {split[3]}", $" Product EXP: {split[4]}", " Cancel"};
            StockMenu stockMenu = new StockMenu(Prompt, options, userfile);
            bool loop = true;
            do 
            {
                int index = stockMenu.Run();
                switch (index) 
                {
                    case 0:
                    Console.Write("Enter new Product ID: ");
                    split[0] = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(split[0]))
                    {
                        Console.WriteLine("The input must not be empty. Please any key to return.");
                        Console.ReadLine();
                        return;
                    }
                    loop = false;
                    break;

                    case 1:
                    Console.Write("Enter new Product Name: ");
                    split[1] = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(split[1]))
                    {
                        Console.WriteLine("The input must not be empty. Please any key to return.");
                        Console.ReadLine();
                        return;
                    }
                    loop = false;
                    break;

                    case 2:
                    Console.Write("Enter new Product Price: ");
                    split[2] = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(split[2]))
                    {
                        Console.WriteLine("The input must not be empty. Please any key to return.");
                        Console.ReadLine();
                        return;
                    }
                    loop = false;
                    break;

                    case 3:
                    Console.Write("Enter new Product MFG Date(DD/MM/YYYY): ");
                    split[3] = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(split[3]))
                    {
                        Console.WriteLine("The input must not be empty. Please any key to return.");
                        Console.ReadLine();
                        return;
                    }
                    loop = false;
                    break;

                    case 4:
                    Console.Write("Enter new Product EXP Date(DD/MM/YYYY): ");
                    split[4] = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(split[4]))
                    {
                        Console.WriteLine("The input must not be empty. Please any key to return.");
                        Console.ReadLine();
                        return;
                    }
                    loop = false;
                    break;

                    case 5:
                    Console.Write("Modification canceled. Press any key to continue...");
                    Console.ReadKey();
                    loop = false;
                    break;
                }
            }while (loop);

            Console.Write("Are you sure you want change the product details?(yes/no): ");
            string choice = Console.ReadLine()?.Trim().ToLower();
            if (choice != "yes")
            {
                Console.Write("You've cancelled the modification. Press any key to return.");
                Console.ReadKey();
                return;
            }
            string updateDetail = string.Join(',', split);
            int location = read.IndexOf(productREAD);
            read[location] = updateDetail;
            File.WriteAllLines(userfile, read);
            Console.WriteLine("Product details updated successfully. Press any key to continue...");
            Console.ReadKey();
        }

        public void RemoveStock()
        {
            Prompt = "Remove Product";
            string[] options = { " Stock ID", " Product Name", " Exit"};
            StockMenu removeProduct = new StockMenu(Prompt, options, userfile);
            //Make a list to manipulate the contents of the userFile
            List<string> lines = File.ReadAllLines (userfile).ToList();
            //Store the ID or Product name so that the list can compare it to the userFile
            string search;
            bool loop = false;
            //For conformation if the user really wants to delete multiple products with the same name
            string confirmation;
            do
            {
                int index = removeProduct.Run();
                switch (index)
                {
                    case 0:
                        Console.CursorVisible = true;
                        //display stock so that the user can choose/see what they want to remove
                        DisplayStock();
                        Console.Write("Press any key to continue.");
                        Console.ReadKey();
                        Console.Clear();
                        Table header = new Table();
                        header.Expand();
                        header.Border = TableBorder.Square;
                        header.AddColumn(new TableColumn("You are about to delete a product using Product ID").Centered());
                        AnsiConsole.Write(header);
                        Console.Write("Enter Product ID: ");
                        search = Console.ReadLine();
                        //Comparing the lines to 0 so that the program can confirm the return value of RemoveAll and if succesfully removed the boolean loop will be true
                        loop = (lines.RemoveAll(line => line.StartsWith(search + ','))) > 0;
                        break;

                    case 1:
                        Console.CursorVisible = true;
                        //display stock so that the user can choose/see what they want to remove
                        DisplayStock();
                        Console.Write("Press any key to continue.");
                        Console.ReadKey();
                        Console.Clear();
                        Table header2 = new Table();
                        header2.Expand();
                        header2.Border = TableBorder.Square;
                        header2.AddColumn(new TableColumn("You are about to remove all products with the same name!").Centered());
                        AnsiConsole.Write(header2);
                        Console.Write("Enter Product Name: ");
                        search = Console.ReadLine();
                        DisplayStock();
                        Console.Write($"Are you sure you want to delete {search}?(yes/no)");
                        confirmation = Console.ReadLine().Trim().ToLower();
                        if (confirmation != "yes")
                        {
                            Console.WriteLine("Product deleting cancelled. Press any key to Continue.");
                            Console.ReadKey();
                            return;
                        }
                        loop = (lines.RemoveAll(line => line.Split(',')[1].Equals(search, StringComparison.OrdinalIgnoreCase))) > 0;
                        break;

                    case 2:
                        // If the user wants to cancel in removing the product
                        loop = false;
                        return;
                }

                //Updating the file, transferring the list to the text file
                if (loop)
                {
                    File.WriteAllLines(userfile, lines);
                    Console.Clear();
                    DisplayStock();
                    Console.CursorVisible = false;
                    Console.Write("Product was removed succesfully. Press any key to continue.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Stock not found. Press any key to continue...");
                }
            }while (!loop);
        }


        public void Transaction()
        {
            throw new NotImplementedException();
        }
        public void CountStock()
        {
            string[] lines = File.ReadAllLines(userfile); 
           // Create the stock counting table
             var stockTable = new Table() 
            { 
                 Border = TableBorder.Square, 
                 ShowRowSeparators = true, 
                 Alignment = Justify.Center
             }; 
            // Add headers to the table
            stockTable.AddColumn(new TableColumn("[Green][bold]QTY[/][/]").Centered()); 
            stockTable.AddColumn(new TableColumn("[Green][bold]Product Name[/][/]").Centered()); 
            stockTable.AddColumn(new TableColumn("[Green][bold]Price[/][/]").Centered()); 
            // Use a dictionary to count quantities 
            var stockCounts = new Dictionary<string, (int quantity, string price)>(); 
            foreach (string row in lines.Skip(1)) 
            {
                string[] fields = row.Split(',');
                string productName = fields[1]; 
                string price = fields[2]; 
                if (stockCounts.ContainsKey(productName))
                {
                    stockCounts[productName] = (stockCounts[productName].quantity + 1, price);
                } 
                else
                { 
                    stockCounts[productName] = (1,price); 
                }
            } // Add rows to the table 
            foreach (var item in stockCounts) 
            { 
                stockTable.AddRow(item.Value.quantity.ToString(), item.Key, item.Value.price);
            } // Render the stock counting table
            AnsiConsole.Write(stockTable);
            Console.Write("Nothing Follows. Press any key to exit.");
            Console.ReadKey();
        }
        

        public void AccountSetting()
        {
            DeleteUser(folder, filename, Username);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

         public void DeleteUser(string dir, string filename, string username)
        {
            string AccountFileLocation = Path.Combine(dir,filename);
            try
            {
                if (!File.Exists(AccountFileLocation))
                {
                    Console.WriteLine($"File not found: {AccountFileLocation}"); 
                    return;
                }

                var lines = File.ReadAllLines(AccountFileLocation).ToList();
                // Find the user's line
                var userLine = lines.FirstOrDefault(line => line.StartsWith(username + ","));

                if (userLine == null)
                {
                    Console.WriteLine("User not found.");
                    return;
                }

                // Confirm deletion
                Console.Write("Are you sure you want to delete your account? (yes/no): ");
                string confirmation = Console.ReadLine().Trim().ToLower();

                if (confirmation != "yes")
                {
                    Console.WriteLine("Account deletion canceled.");
                    return;
                }

                // Remove the user's line
                lines.Remove(userLine);

                // Write the updated lines back to the file
                File.WriteAllLines(AccountFileLocation, lines);
                string userDirectory = Path.Combine(dir, username);
                if (Directory.Exists(userDirectory))
                {
                    Directory.Delete(userDirectory, true);
                    Console.WriteLine($"The directory of {userDirectory} has been successfully deleted.");
                    status = true;
                }
                else
                {
                    Console.WriteLine("User directory not found.");
                }
        
                status = true;
                Console.WriteLine("Your account has been successfully deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Add search method
        //When Searching for a product the Cases must be lowered and when the Name or ID is Compared it must also be converted into lower case
        // So that no problems will occur in the program
        //List
        public void SearchProduct()
        {
            string Search;
            Console.Clear();
            Table header = new Table();
            header.Border = TableBorder.Square;
            header.ShowRowSeparators = true;    
            header.Expand();
            header.AddColumn(new TableColumn("[Bold Yellow]Search for a Product[/]").Centered());
            AnsiConsole.Write(header);
            Console.CursorVisible = true;
            Console.Write("Enter product ID or Product Name: ");
            Search = Console.ReadLine() ?.Trim().ToLower();

            if (string.IsNullOrEmpty(Search))
            {
                Console.Write("Input cannot be empty. Press any key to return.");
                Console.ReadKey();
                return;
            }

            string[] read = File.ReadAllLines(userfile);
            var result = new List<string>();

            foreach (string word in read.Skip(1))
            {
                string[] field = word.Split(',');
                if (field[0].ToLower().Contains(Search) || field[1].ToLower().Contains(Search)) 
                {
                    result.Add(word);
                }
            }

            if (result.Count > 0)
            {
                var SearchTable = new Table
                {
                    Border = TableBorder.Square,
                    ShowRowSeparators = true,
                    Expand = true,
                    Alignment = Justify.Center
                };
                foreach (string head in read[0].Split(','))
                {

                    SearchTable.AddColumn(new TableColumn($"[Bold Yellow]{head}[/]").Centered());
                }
                foreach (var searched in result)
                {
                    string[] split = searched.Split(',');
                    SearchTable.AddRow(split[0], split[1], split[2], split[3], split[4]);
                }
                AnsiConsole.Write(SearchTable);
                Console.Write("Nothing follows. Press any key to continue.");
                Console.ReadKey();
            }
            else 
            {
                Console.Write("No products found matching that term.");
                Console.ReadKey();
            }
        }
    }
}