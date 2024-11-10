using PulseStock___Inventory_Management_System.Classes.Interface;
using PulseStock___Inventory_Management_System.Classes.Parent;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseStock___Inventory_Management_System.Classes.Functions
{
    internal class Menu : Data, IMenu
    {
        
        public Menu()
        {
        }

        public Menu(string prompt, string[] options)
        {
            Prompt = prompt;
            Options = options;
            Index = 0;
        }
        public void DisplayOptions()
        {
            /* using the spectre console the program will display the options inside a panel
             and a table for the header and prompt that tells the user to navigate the options using 
            arrow keys or ws key
             */
            var header = new Table();
            header.Expand();
            header.AddColumn(new TableColumn(Prompt).Centered());
            header.AddRow("Use Arrow up and Arrow Down or W and D keys to navigate.");
            header.Alignment(Justify.Center);
            var panelContent = new List<string>(); 
            for (int i = 0; i < Options.Length; i++)
            { 
                string currentOption = Options[i]; 
                string selector = i == Index ? "[bold yellow]>[/]" : " "; 
                string option_selector = $"{selector}{currentOption}"; 
                panelContent.Add(option_selector);
            }
            var panel = new Panel(string.Join("\n", panelContent));
            panel.Padding = new Padding(3, 3);
            panel.Header = new PanelHeader("Options");
            panel.Border = BoxBorder.Rounded;
            panel = panel.BorderColor(Color.White);
            AnsiConsole.Write(header);
            AnsiConsole.Write(panel); 
        }

        public int Run()
        {
            ConsoleKey keypressed;
            do
            {
                Console.Clear();
                Console.CursorVisible = false;
                DisplayOptions();
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