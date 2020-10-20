using System.Threading;
using Tanks_Game.Models;
using static System.Console;
using static Tanks_Game.Helpers.Helper;

namespace Tanks_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            // задаємо розмір консолі
            SetWindowSize(100, 30);
            // для вибору в головному меню
            char choice;
            // для вибору карти (за замовчуванням перша карта)
            char choiceMap = '1'; 
            
            Game game = new Game();
            // показ логотипа
            Logo();
            
            CursorVisible = false;
            string mapPath = null;
            do
            {
                mapPath = "map";
                Clear();
                ShowMenu();
                choice = ReadKey().KeyChar;
                switch (choice)
                {
                    case '0':
                        Clear();
                        WriteLine("Goodbye! See you later!");
                        Thread.Sleep(1000);
                        break;
                    case '1':
                        mapPath += choiceMap.ToString();
                        mapPath += ".txt";
                        game.StartGame(mapPath);
                        break;
                    case '2':
                        ShowRules();
                        break;
                    case '3':
                        Clear();
                        ShowMapsMenu();
                        choiceMap = ReadKey().KeyChar;
                        switch (choiceMap)
                        {
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                break;
                            default:
                                choiceMap = '1';
                                break;
                        }
                        mapPath += choiceMap.ToString();
                        mapPath += ".txt";
                        Clear();
                        IllusionOfLoading();
                        WriteLine($"\tEXEMPLE: MAP << {choiceMap} >>");
                        game.LoadTheMap(mapPath);
                        game.DisplayMap();
                        break;
                    default:
                        WriteLine("\nIncorrect choice! Try again!");
                        break;
                }

                // після закінчення ігри вийти з програми
                if (choice == '1')
                {
                    Thread.Sleep(1000);
                    break;
                }
                if (choice != '0')
                {
                    WriteLine("\nPress any key to continue . . . ");
                    ReadKey();
                }
            } while (choice != '0');
        }
    }
}