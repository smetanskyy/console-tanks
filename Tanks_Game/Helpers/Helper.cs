using System;
using System.Threading;
using static System.Console;

namespace Tanks_Game.Helpers
{
    abstract class Helper
    {
        // вивід головного меню
        public static void ShowMenu()
        {
            WriteLine("      MENU:");
            WriteLine("1 - Start game");
            WriteLine("2 - Show rules");
            WriteLine("3 - Select a map");
            WriteLine("0 - Exit");
            Write("\nMake your choice: >> ");
        }

        // вивід меню з картами
        public static void ShowMapsMenu()
        {
            WriteLine("      MAPS::");
            WriteLine("1 - Map1");
            WriteLine("2 - Map2 <<Tank>>");
            WriteLine("3 - Map3");
            WriteLine("4 - Map4");
            WriteLine("5 - Map5");
            WriteLine("6 - Map6 << I love C# >>");
            WriteLine("7 - Map7 ");
            WriteLine("8 - Map8 << Trident >>");
            WriteLine("9 - Map9 << Empty >>");
            Write("\nMake your choice: >> ");
        }

        // вивід логотипа
        public static void Logo()
        {
            Clear();
            CursorVisible = false;
            string logo = "\n\tCONSOLE TANKS";
            foreach (char item in logo)
            {
                Write(item);
                Thread.Sleep(300);
            }
            Thread.Sleep(300);
            Clear();
        }

        // вивід правил ігри
        public static void ShowRules()
        {
            Clear();
            WriteLine("CONSOLE TANKS by SSI Inc.\n");
            WriteLine("The rules are very simple. You must destroy all" +
                      "\nenemy tanks and protect the base.");
            WriteLine("Legend:");
            ForegroundColor = ConsoleColor.DarkGreen;
            Write("\tW ");
            ResetColor();
            WriteLine(" - user tank");

            ForegroundColor = ConsoleColor.Red;
            Write("\tU ");
            ResetColor();
            WriteLine(" - enemy tank");

            ForegroundColor = ConsoleColor.Green;
            Write("\tY ");
            ResetColor();
            WriteLine(" - tree");

            ForegroundColor = ConsoleColor.DarkRed;
            Write("\t[]");
            ResetColor();
            WriteLine(" - brick");

            ForegroundColor = ConsoleColor.Blue;
            Write("\t{}");
            ResetColor();
            WriteLine(" - river");

            ForegroundColor = ConsoleColor.Cyan;
            Write("\t$ ");
            ResetColor();
            WriteLine(" - base");

            ForegroundColor = ConsoleColor.White;
            Write("\t* ");
            ResetColor();
            WriteLine(" - enemy bullet");

            ForegroundColor = ConsoleColor.White;
            Write("\t+ ");
            ResetColor();
            WriteLine(" - user bullet");

            WriteLine("\nMovement (PRESS KEY):");
            WriteLine("\t'w' - move up");
            WriteLine("\t's' - move down");
            WriteLine("\t'a' - move left");
            WriteLine("\t'd' - move right");
            WriteLine("\t'SPACE' - shoot\n");
            WriteLine("\t'q' - exit\n");
            ResetColor();
            WriteLine("Good luck!\n");

            WriteLine("Copyright 2019");
        }

        public static void IllusionOfLoading()
        {
            // псевдозагрузка
            Clear();
            for (int i = 1; i <= 10; i++)
            {
                WriteLine("\n\tLOADING . . . "); 
                Write("\t[ ");
                for (int j = 1; j <= 10; j++)
                {
                    if (j > i)
                        Write("  ");
                    else
                        Write("##");
                }
                Write($" {i * 10}% ]");
                Thread.Sleep(200);
                Clear();
            }
        }
    }
}