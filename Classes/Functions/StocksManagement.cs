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
using System.Net.Http.Headers;
using System.IO;

namespace PulseStock___Inventory_Management_System.Classes.Functions
{
    internal class StocksManagement : Data, IStocksManipulation
    {
        public StocksManagement(string userstock, string usersales,string username)
        {
            userStockList = userstock;
            userSales = usersales;
            Username = username;
        }

        public StocksManagement(string userstock)
        {
            userStockList = userstock;
        }
        public void StocksStart()
        {
            Console.Title = "PulseStock - Inventory Manager";
            StockMenu();
        }

        private void StockMenu()
        {
            Prompt = "Inventory Manager";
            string[] options = { " Add Product", " View Product List", " Search for Product", " Modify Product",
                                " Remove Product", " Transaction and History"," Delete Account"," Log Out"};
            StockMenu stockMenu = new StockMenu(Prompt, options, userStockList);
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
                        DisplayStock();
                        Console.ReadKey();
                        break;
                    case 2:
                        SearchProduct();
                        break;
                    case 3:
                        ModifyStock();
                        break;
                    case 4:
                        RemoveStock();
                        break;
                    case 5:
                        Transaction();
                        break;
                    case 6:
                        AccountSetting();
                        if (status == true) 
                        {
                            loop = false;
                            break;
                        }
                        break;
                    case 7:
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
            Console.Write("Enter Product Quantity: ");
            string stockQTY = Console.ReadLine();
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
            using (StreamWriter stockWrite = File.AppendText(userStockList)) 
            {
                stockWrite.WriteLine($"{stockID},{stockName},{stockPrice},{stockQTY},{stockMFG},{stockEXP}");
            }
            Console.Write($"{stockID} {stockName} - succesfully added to the database.");
            Console.ReadKey();
        }

        public void DisplayStock()
        {
            Console.Clear();
            Console.WriteLine();
            string[] lines = File.ReadAllLines(userStockList);
            string[] check = lines.Skip(1).ToArray();
            string[] headers = lines[0].Split(',');
            if (check.Length == 0) 
            {
                Console.Write("File is empty, try adding products. Press any key to return.");
                Console.ReadKey();
                return;
            }
            var table = new Table(); //using the spectre console we are creating a table for GUI
            table.Border = TableBorder.Square;
            table.ShowRowSeparators = true;
            table.Alignment(Justify.Center);
            foreach (string header in headers) //Add built in header for the table
            {
                table.AddColumn($"[Bold Yellow]{header}[/]");
            }
            foreach (string row in lines.Skip(1)) //Skipping the first line so that the program can add the stock details 
            {
                string[] fields = row.Split(',');
                table.AddRow($"{fields[0]}", $"{fields[1]}", $"{fields[2]}", $"{fields[3]}", $"{fields[4]}", $"{fields[5]}");
            }
            AnsiConsole.Write(table);
            Console.Write("Nothing Follows. Press any key to exit.");
        }

        public void ModifyStock()
        {
            Console.Clear();
            Console.CursorVisible = true;
            Table header = new Table();
            header.Border = TableBorder.Square;
            header.Expand();
            header.AddColumn(new TableColumn("[Bold Yellow]Change Product Details[/]").Centered());
            header.AddRow("Please put the Product ID of the product that you want to modify.").Centered();
            AnsiConsole.Write(header);
            DisplayStock();
            Console.ReadKey(true);
            Console.Clear();

            AnsiConsole.Write(header);
            Console.Write("Enter the Product ID: ");
            string productId  = Console.ReadLine()?.Trim();

            List<string> read = File.ReadAllLines(userStockList).ToList();
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
            string[] options = { $" Product ID: {split[0]}", $" Product Name: {split[1]}", $" Price: {split[2]}", $" QTY: {split[3]}", $" Product MFG: {split[4]}", $" Product EXP: {split[5]}", " Cancel"};
            StockMenu stockMenu = new StockMenu(Prompt, options, userStockList);
            bool loop = true;
            do 
            {
                int index = stockMenu.Run();
                switch (index) 
                {
                    case 0:
                    Console.CursorVisible = true;
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
                    Console.CursorVisible = true;
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
                    Console.CursorVisible = true;
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
                    Console.CursorVisible = true;
                    Console.Write("Enter new Product Quantity: ");
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
                    Console.CursorVisible = true;
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
                    Console.CursorVisible = true;
                    Console.Write("Enter new Product EXP Date(DD/MM/YYYY): ");
                    split[5] = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(split[4]))
                    {
                        Console.WriteLine("The input must not be empty. Please any key to return.");
                        Console.ReadLine();
                        return;
                    }
                    loop = false;
                    break;

                    case 6:
                    Console.Write("Modification canceled. Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
            }while (loop);
            Console.CursorVisible = true;
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
            File.WriteAllLines(userStockList, read);
            Console.CursorVisible = false;
            Console.WriteLine("Product details updated successfully. Press any key to continue...");
            Console.ReadKey();
        }

        public void RemoveStock()
        {
            Console.Clear();
            Prompt = "Remove Product";
            string[] options = { " Stock ID", " Product Name", " Exit"};
            StockMenu removeProduct = new StockMenu(Prompt, options, userStockList);
            //Make a list to manipulate the contents of the userFile
            List<string> lines = File.ReadAllLines (userStockList).ToList();
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
                        if (string.IsNullOrEmpty(search)) 
                        {
                            Console.Write("The input cannot be empty. Please enter any key to continue.");
                            Console.ReadKey();
                            return;
                        }

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
                        if (string.IsNullOrEmpty(search)) 
                        {
                            Console.Write("The input cannot be empty. Please enter any key to continue.");
                            Console.ReadKey();
                            return;
                        }
                        DisplayStock();
                        Console.Write($"Are you sure you want to delete {search}?(yes/no)");
                        confirmation = Console.ReadLine().Trim().ToLower();
                        if (string.IsNullOrEmpty(search)) 
                        {
                            Console.Write("The input cannot be empty. Please enter any key to continue.");
                            Console.ReadKey();
                            return;
                        }
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
                    File.WriteAllLines(userStockList, lines);
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


        private void Transaction()
        {
            Console.Clear();
            Prompt = "Transactions and Analytics";
            string[] options = { " Make a Transaction", " View Transaction History", " Exit"};
            StockMenu transMenu = new StockMenu(Prompt,options,userSales);
            bool loop = true;

            do 
            {
                int index = transMenu.Run();
                switch (index) 
                {
                    case 0:
                        MakeTransaction();
                        break;
                    case 1:
                        ViewTransactons();
                        break;
                    case 2:
                        loop = false;
                        break;
                }
            }while(loop);
        }

        public void MakeTransaction()
        { 
            Console.Clear();

            string[] stockList = File.ReadAllLines(userStockList);
            List<string[]> products = new List<string[]>();
            int productCount = 1;
            var table = new Table 
            {
                Border = TableBorder.Square,
                ShowRowSeparators = true, 
                Alignment = Justify.Center 
            }; 
            table.Title = new TableTitle("[Bold Yellow]Available Products:[/]"); 
            table.AddColumn("No.");
            table.AddColumn("Product Name"); 
            table.AddColumn("Price"); 
            table.AddColumn("Quantity"); 
            foreach (string line in stockList.Skip(1)) 
            {
                string[] parts = line.Split(',');
                parts[0] = productCount.ToString();
                products.Add(parts);
                table.AddRow(productCount.ToString(), parts[1], parts[2], parts[3]);
                productCount++;
            }
            AnsiConsole.Write(table);

            List<string[]> transaction = new List<string[]>();
            bool addproducts = true;
            do
            {
                Console.CursorVisible = true;
                Console.Write("\nEnter the product number to add to the transaction (type \"done\" to proceed or \"exit\" cancel): ");
                string input = Console.ReadLine().Trim().ToLower();
                if (input == "done")
                {
                    addproducts = false;
                }
                if (input == "exit")
                {
                    return;
                }
                else if (int.TryParse(input, out int ProductId) && ProductId > 0 && ProductId <= products.Count)
                {
                    var product = products[ProductId - 1];
                    Console.Write("Enter Quantity: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity <= int.Parse(product[3]))
                    {
                        product[3] = (int.Parse(product[3]) - quantity).ToString();
                        var transactionProduct = transaction.Find(p => p[0] == ProductId.ToString());
                        if (transactionProduct == null)
                        {
                            transaction.Add(new string[] { ProductId.ToString(), product[1], product[2], quantity.ToString() });
                        }
                        else
                        {
                            transactionProduct[3] = (int.Parse(transactionProduct[3]) + quantity).ToString();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity. Please try again.");
                    }
                }

            } while (addproducts);
            double money = 0;
            double total = 0;
            double change = 0;
            bool validCash = false;
            foreach (var product in transaction) 
            {
                total += double.Parse(product[2]) * int.Parse(product[3]); 
            }
            Console.WriteLine($"Total Sales : {total}");
            while (!validCash) 
            {
                Console.Write("Enter cash amount: ");
                if (double.TryParse(Console.ReadLine(), out money) && money >= total)
                {
                    validCash = true;
                }
                else 
                {
                    Console.WriteLine("Invalid cash amount. Please enter an amount equal to or greater than the total.");
                }
            }
            change = money - total;
            Console.Clear(); 
            table = new Table 
            {
                Border = TableBorder.Square,
                ShowRowSeparators = true, 
                Alignment = Justify.Center 
            };
            table.Title = new TableTitle("[Bold Yellow]R&G Soy Food Products[/]");
            table.AddColumn("Product Name"); 
            table.AddColumn("Price"); 
            table.AddColumn("Quantity"); 
            foreach (var product in transaction) 
            { 
                table.AddRow(product[1], product[2], product[3]); 
                
            } 
            table.AddRow(new Markup("[bold red]Total[/]"), new Markup($"[bold red]PHP {total}[/]"));
            table.AddRow(new Markup("[bold blue]Cash[/]"), new Markup($"[bold blue]PHP {money:0.00}[/]"));
            table.AddRow(new Markup("[bold green]Change[/]"), new Markup($"[bold green]PHP {change:0.00}[/]"));
            table.AddRow(new Markup($"Date Purchased: [Bold Yellow]{DateTime.Now}[/]"), new Markup(""));
            table.AddRow(new Markup("[Bold Yellow]This is your SALES INVOICE[/]"));
            AnsiConsole.Write(table); 

            // Save the transaction Date|Product Names| Product Quantities| Total Sales| Cash||Change
            string transactionRecord = $"{DateTime.Now}|{string.Join(',', transaction.Select(p => $"{p[1]}"))}|{string.Join(',', transaction.Select(p => $"{p[3]}"))}|{total}|{money}|{change}"; 
            File.AppendAllText(userSales, transactionRecord + Environment.NewLine); 

            // Update stock list with new quantities
            List<string> updatedStockList = new List<string>
            { 
                stockList[0] // Keep header
            }; 
            foreach (var product in products) 
            {
                if (int.Parse(product[3]) > 0) {
                    updatedStockList.Add($"{product[0]},{product[1]},{product[2]},{product[3]},{product[4]},{product[5]}");
                }
            } 
            File.WriteAllLines(userStockList, updatedStockList);
            Console.CursorVisible = false;
            Console.WriteLine("Transaction completed. Press any key to return to the menu.");
            Console.ReadKey();
        }
        public void ViewTransactons() 
        {
             Console.Clear();
            string[] transactionRecords = File.ReadAllLines(userSales);

            // Check if the userSales file exists
            if (!File.Exists(userSales) || transactionRecords.Length == 0)
            {
                Console.WriteLine("No transactions found.");
                Console.WriteLine("Press any key to return to the menu.");
                Console.ReadKey();
                return;
            }

            // Read the transaction records
            // Display transactions in a table
            var table = new Table
            {
                Border = TableBorder.Square,
                ShowRowSeparators = true,
                Alignment = Justify.Center
            };
            table.Expand();
            table.Title = new TableTitle("[Bold Yellow]Transaction History[/]");
            table.AddColumn("Date");
            table.AddColumn("Products");
            table.AddColumn("Quantity");
            table.AddColumn("Total");
            table.AddColumn("Cash");
            table.AddColumn("Change");
            string date, total, cash, change, product,quantity;
            string[] products;

            foreach (string record in transactionRecords)
            {
                products = record.Split('|');
                date = products[0];
                product = products[1].Replace(',', '\n');
                quantity = products[2].Replace(',', '\n');
                total = products[3];
                cash = products[4];
                change = products[5];  
                table.AddRow(date, product,quantity,total, cash, change);
            }

            AnsiConsole.Write(table);

            Console.WriteLine("Press any key to return to the menu.");
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

            string[] read = File.ReadAllLines(userStockList);
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