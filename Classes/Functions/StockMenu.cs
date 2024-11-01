using PulseStock___Inventory_Management_System.Classes.Interface;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseStock___Inventory_Management_System.Classes.Functions
{
    internal class StockMenu : StocksManagement,IMenu
    {
       
        public StockMenu(string prompt, string[] options, string userFile) :base(userFile)
        {
            Prompt = prompt;
            Options = options;
            Index = 0;
            userFile = userFile;
        }

        public void DisplayOptions()
        {
            var Header = new Table();
            Header.Alignment(Justify.Center);
            Header.Border = TableBorder.Square;
            Header.AddColumn(Prompt);
            Header.AddRow("Use Arrow up and Arrow Down or W and D keys to navigate.");
            AnsiConsole.Write(Header);
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                string selector = i == Index ? "> " : "  ";
                string option_selector = $"{selector} {currentOption}";

                if (i == Index)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;//Visually pleasing
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(option_selector);
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(option_selector);
                }
            }
            Console.ResetColor();
        }

        public int Run()
        {
            ConsoleKey keypressed;
            do
            {
                Console.Clear();
                Console.CursorVisible = false;
                DisplayOptions();
                DisplayStock();  //Dispays the stock list
                ConsoleKeyInfo keyinfo = Console.ReadKey(true);
                keypressed = keyinfo.Key;

                if (keypressed == ConsoleKey.UpArrow || keypressed == ConsoleKey.W)
                {
                    Index -= 1;
                    if (Index == -1)
                    {
                        Index = Options.Length - 1;
                    }
                }

                else if (keypressed == ConsoleKey.DownArrow || keypressed == ConsoleKey.S)
                {
                    Index += 1;
                    if (Index == Options.Length)
                    {
                        Index = 0;
                    }
                }
            } while (keypressed != ConsoleKey.Enter);
            return (int)Index;
        }
    }
}
