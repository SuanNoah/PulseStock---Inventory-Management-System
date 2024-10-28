using PulseStock___Inventory_Management_System.Classes.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseStock___Inventory_Management_System.Classes.Functions
{
    internal class StockMenu : StocksManagement,IMenu
    {
        private string Prompt;
        private string[] Options;
        private int index;
        private string userFile;
        public StockMenu(string prompt, string[] options, string userFile) :base(userFile)
        {
            Prompt = prompt;
            Options = options;
            index = 0;
            this.userFile = userFile;
        }

        public void DisplayOptions()
        {
            Console.WriteLine(Prompt);
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                string selector = i == index ? "> " : "  ";
                string option_selector = $"{selector} {currentOption}";

                if (i == index)
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
                DisplayStock(); 
                ConsoleKeyInfo keyinfo = Console.ReadKey(true);
                keypressed = keyinfo.Key;

                if (keypressed == ConsoleKey.UpArrow || keypressed == ConsoleKey.W)
                {
                    index -= 1;
                    if (index == -1)
                    {
                        index = Options.Length - 1;
                    }
                }

                else if (keypressed == ConsoleKey.DownArrow || keypressed == ConsoleKey.S)
                {
                    index += 1;
                    if (index == Options.Length)
                    {
                        index = 0;
                    }
                }
            } while (keypressed != ConsoleKey.Enter);
            return index;
        }
    }
}
