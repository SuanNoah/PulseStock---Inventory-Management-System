using PulseStock___Inventory_Management_System.Classes.Interface;
using PulseStock___Inventory_Management_System.Classes.Parent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PulseStock___Inventory_Management_System.Classes.Functions
{
    internal class AccountManager : Data, IAccountManipulation
    {
        
        public AccountManager()
        {
            //Creates the Directory/Folder for accounts and Creates a file for the account list
            filename = Path.Combine(folder, filename);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (!File.Exists(filename))
            {
                using (File.Create(filename)) { }
            }
        }
        public void AccountStart()
        {
            Console.Title = "PulseStock - Account Manager";
            RunMenu();
        }

        private void RunMenu()
        {
            Prompt = "        Log in or Sign up";
            string[] options = {"Log in", "Sign up", "Go Back"};

            Menu accountMenu = new Menu(Prompt, options);
            bool loop = true;
            do
            {
                int index = accountMenu.Run();
                switch (index)
                {
                    case 0:
                        LogIn();
                        break;

                    case 1:
                        CreateUser();
                        break;

                    case 2:
                        goBack();
                        loop = false;
                        break;
                }
            }while (loop);
        }
        public void CreateUser()
        {
            Console.CursorVisible = true;
            input[0] = InputNotNull("\nEnter username: ");
            if (input[0] == "error") { return; }
            input[1] = InputNotNull("Enter password: ", true);
            if (input[1] == "error") { return; }//The returned string will be used to break the input and return to the menu safely

            if (CheckUser(input[0]))
            {
                Console.WriteLine("\nUser already exist. Please try a different username. Press any key to continue.");
                Console.ReadKey();
                return;
            }
            //Writing input[0] as the username and input[1] as password while being separated using comma(,)
            using (StreamWriter writeuser = File.AppendText(filename))
            {
                writeuser.WriteLine(input[0] + "," + input[1]);
            }
            //creates a directory and a file specified for the user, it will be in located at Debug/net8.0/Accounts/Username/Username_StockList
            string userDirectory = Path.Combine(folder, input[0]);
            Directory.CreateDirectory(userDirectory);
            string userFile = Path.Combine(userDirectory, input[0] + "_StockList");
            using (StreamWriter writeHeader = File.AppendText(userFile))
            {
                writeHeader.WriteLine("Stock ID,Product Name,Price,Date MFG,Date EXP");//Writes the header for stock details
            }
            Console.Write($"\nAccount for {input[0]} has been created successfuly. Press any key to continue.");
            Console.ReadKey();
            return;
        }
        public void LogIn()
        {
            string[] lines = File.ReadAllLines(filename);
            Console.CursorVisible = true;
            if (lines.Length == 0)//Checks if file has accounts
            {
                Console.Write("No accounts were found in the database, please try creating one. Press any key to continue.");
                Console.ReadKey();
                return;
            }
            Username = InputNotNull("\nEnter username: ");
            if (Username == "error") { return; }
            Password = InputNotNull("Enter password: ", true);
            if( Password == "error") { return; }//The returned string will be used to break the input and return to the menu safely

            for (int i = 0; i < lines.Length; i++)
            {
                string[] field = lines[i].Split(',');
                if (field[0] == Username && field[1] == Password)
                {
                    Console.Write($"\n{Username} logged in successfully. Press any key to continue.");
                    Console.ReadKey();
                    //After loggin in the program should open the specified user directory and then open the specified stocks list for the user
                    string userDirectory = Path.Combine (folder, Username);
                    string userFile = Path.Combine(userDirectory, Username + "_StockList");
                    StocksManagement manage = new StocksManagement(userFile);
                    manage.StocksStart();
                    return;
                }
            }
            Console.Write("\nInvalid username or password! Please try again\nPress any key to continue.");
            Console.ReadKey();
            return;
        }
        public bool CheckUser(string username)//checks the username if it is already in the file
        {
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                if (line.Split(',')[0].Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        public string InputNotNull(string prompt, bool password = false)//Custom Line Reader to determine if user input is not null
        {
            string? input;
            do
            {
                Console.Write(prompt);
                input = password ? ReadPassword() : Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.Write("\nThere was no input, please try again. Press any key to continue.");
                    Console.ReadKey();
                    return "error";//Returns a string message after the user inputs an empty string
                }
            } while (string.IsNullOrEmpty(input));
            return input;
        }
        public string ReadPassword()//changing the character inputed by the user to protect password
        {
            string password = string.Empty;
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key != ConsoleKey.Backspace && keyinfo.Key != ConsoleKey.Enter)
                {
                    //Changing the input to asterisk
                    password += keyinfo.KeyChar;
                    Console.Write("*");

                }
                else if (keyinfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    //Erases the input
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (keyinfo.Key != ConsoleKey.Enter);
            return password;
        }

        private void goBack() //go back to main menu/home
        {
            Console.WriteLine("You are now going back to the main menu. Press any key to continue.");
            Console.ReadKey();
        }
        public void DeleteUser() //Deletes the user
        {
            throw new NotImplementedException();
        }

    }
}