using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Tanks_Game.Interface;
using static System.Console;

namespace Tanks_Game.Models
{
    // клас реалізовує інтерфейс
    class Game : IDisplay
    {
        // розміри карти
        public const int ROW = 25;
        public const int COLUMN = 100;
        // час затримки для більш зручного сприйнятя юзером
        const int sleepTime = 350;
        // усі кулі будуть заноситись у список
        public static List<Bullet> bullets;
        // список ворогів
        List<Enemy> enemies;
        // власне танк юзера
        public UserTank userTank;
        // лічильник для послідовності дій
        static int counter = 0;
        // кількість ворогів
        const int countOfEnemies = 15;
        // кількість "мертвих" ворогів
        static int counterOfDeadEnemies = 0;
        // вказівник для карти
        public static char[,] map;
        // здоровя юзера
        public static int health = 100;
        // чи ще ігра триває
        public static bool isGame = true;
        // чи ще юзер не виграв
        public static bool isWin = false;

        // загрузка карти з файла
        public char[,] LoadTheMap(string mapPath)
        {
            // відкриваємо текстовий файл і "загружаємо" карту
            using (FileStream file = new FileStream(mapPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                map = new char[ROW, COLUMN];
                byte[] readBytes = new byte[(int)file.Length];
                file.Read(readBytes, 0, readBytes.Length);
                char[] tempMap = Encoding.Default.GetChars(readBytes);

                int index = 0;
                for (int i = 0; i < ROW; i++)
                {
                    for (int j = 0; j < COLUMN; j++)
                    {
                        if (tempMap[index] == '\n')
                            ++index;
                        map[i, j] = tempMap[index];
                        ++index;
                    }
                }
            }
            return map;
        }
        // вивід на екран карти
        public void DisplayMap()
        {
            try
            {
                for (int i = 0; i < ROW; i++)
                {
                    for (int j = 0; j < COLUMN; j++)
                    {
                        if (map[i, j] == '#')
                            ForegroundColor = ConsoleColor.DarkMagenta;
                        else if (map[i, j] == 'U')
                            ForegroundColor = ConsoleColor.Red;
                        else if (map[i, j] == '*' || map[i, j] == '+')
                            ForegroundColor = ConsoleColor.White;
                        else if (map[i, j] == '$')
                            ForegroundColor = ConsoleColor.Cyan;
                        else if (map[i, j] == 'Y')
                            ForegroundColor = ConsoleColor.Green;
                        else if (map[i, j] == '[' || map[i, j] == ']')
                            ForegroundColor = ConsoleColor.DarkRed;
                        else if (map[i, j] == '{' || map[i, j] == '}')
                            ForegroundColor = ConsoleColor.Blue;
                        else if (map[i, j] == 'W')
                            ForegroundColor = ConsoleColor.DarkGreen;
                        else
                            ResetColor();
                        Write(map[i, j]);
                        ResetColor();
                    }
                    WriteLine();
                }
            }
            catch (Exception exc)
            {
                Clear();
                WriteLine(exc.Message);
            }
        }
        // встановлюємо ворогів
        void SetEnemies()
        {
            enemies = new List<Enemy>(countOfEnemies);
            for (int i = 0; i < countOfEnemies; i++)
            {
                // надаємо ворогам 3 різні координати
                if (i % 3 == 0)
                    enemies.Add(new Enemy(1, 2));
                if (i % 3 == 1)
                    enemies.Add(new Enemy(1, COLUMN / 2 - 1));
                if (i % 3 == 2)
                    enemies.Add(new Enemy(1, COLUMN - 4));
            }
            // записуємо трьох на карту
            for (int i = 0; i < 3; i++)
            {
                enemies[i].OverwritingMap();
            }
        }
        // опис ходів для противника
        void MoveEnemies()
        {
            // будуть ходити через раз, надаємо користувачу перевагу (буде скоріший на 1 хід)
            if (counter % 2 != 0)
                return;

            // рух тільки 3-троьох перших
            int amountForThree = 0;
            for (int i = 0; i < enemies.Count && amountForThree < 3; i++)
            {
                if (enemies[i].IsAlive == true)
                {
                    ++amountForThree;
                    enemies[i].Move();
                }
            }

        }
        // опис пострілів для противників
        public void ShootEnemies()
        {
            // кожний 5-ий ігровий хід вистріл
            if (counter != 5)
                return;

            // для 3-троьох перших
            int amountForThree = 0;
            for (int i = 0; i < enemies.Count && amountForThree < 3; i++)
            {
                if (enemies[i].IsAlive == true)
                {
                    ++amountForThree;
                    enemies[i].Shoot();
                }
            }
        }
        // перевірка на виграш
        bool CheckIsWin()
        {
            counterOfDeadEnemies = 0;
            foreach (Enemy item in enemies)
            {
                if (item.IsAlive == false)
                    ++counterOfDeadEnemies;
            }
            if (counterOfDeadEnemies == countOfEnemies)
            {
                isWin = true;
                return true;
            }
            else
                return false;
        }
        // рух і оновлення в списку для куль
        void MoveBullets()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].IsAlive == false)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                Game.bullets[i].Move();
            }
        }

        // лічильник рухів в ігрі
        void ChangeCounter()
        {
            Game.counter++;
            if (Game.counter == 10)
                Game.counter = 0;
        }
        // власне сама ігра
        public void StartGame(string mapPath)
        {
            // обнулення ігри
            Game.bullets = new List<Bullet>();
            Game.counter = 0;
            Game.counterOfDeadEnemies = 0;
            Game.health = 100;
            Game.isGame = true;
            Game.isWin = false;
            userTank = new UserTank();
            Clear();
            Helpers.Helper.IllusionOfLoading();
            WriteLine($"\tHEALTH << {health} >>\t\t\t\t\tCOUNT OF ENEMIES << {countOfEnemies - counterOfDeadEnemies} / {countOfEnemies} >>");
            LoadTheMap(mapPath);
            DisplayMap();
            WriteLine("PRESS ANY KEY TO START . . . GOOD LUCK ! ! !");
            ReadKey();
            SetEnemies();

            do
            {
                // кожний раз очищаємо ігру
                Clear();
                // статистика ігри
                WriteLine($"\tHEALTH << {health} >>\t\t\t\t\tCOUNT OF ENEMIES << {countOfEnemies - counterOfDeadEnemies} / {countOfEnemies} >>");
                DisplayMap();               // вивід карти на екран
                MoveEnemies();              // рух ворожих танків
                ShootEnemies();             // вистріл ворогів
                userTank.MoveOrShoot();     // хід користувача
                MoveBullets();              // переміщення куль
                ChangeCounter();            // обновлюємо лічильник
                Thread.Sleep(sleepTime);    // затримка для кращого сприйнятя користувачем

                // перевірка на виграш
                if (CheckIsWin())
                    break;
                // перевірка на програш
            } while (Game.isGame);
            Clear();
            // підсумок ігри
            if (Game.isWin)
                WriteLine("CONGRATULATION! YOU ARE WIN!");
            else
                WriteLine("TRY AGAIN! YOU ARE LOSE!");
        }
    }
}