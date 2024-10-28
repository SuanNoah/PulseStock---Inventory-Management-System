using PulseStock___Inventory_Management_System.Classes.Interface;
using PulseStock___Inventory_Management_System.Classes.Parent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PulseStock___Inventory_Management_System.Classes.Functions
{
    internal class Home : Data, IHomeMethods
    {
        public Home() { }

        public void start()
        {
            Console.Title = "PulseStock - Home";
            HomeMenu();
        }
        public void HomeMenu()
        {
             Prompt = "Welcome to PulseStock - Inventory System!";
            string[] Options = { "Start", "About", "Exit" };

            Menu HomeMenu = new Menu(Prompt, Options);
            bool loop = true;
            do
            {
                int index = HomeMenu.Run();
                Console.CursorVisible = false;

                switch (index)
                {
                    case 0:
                        Start();
                        break;

                    case 1:
                        About();
                        break;

                    case 2:
                        Exit();
                        loop = false;
                        break;

                    default:
                        Console.WriteLine();
                        break;
                }
            } while (loop);
        }
        public void Start()
        {
            Console.WriteLine("");
            AccountManager accountManager = new AccountManager();
            accountManager.AccountStart();
        }
        public void About()
        {
            Console.WriteLine("");
        }

        public void Exit()
        {
            Console.WriteLine("Thanks for using");
        }



    }
}