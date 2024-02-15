using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ConsoleApp10
{
    public class Program
    {
        static void Main()
        {
            MazeLoader mazeLoader = new MazeLoader();
            Maze maze = mazeLoader.LoadMaze("maze.txt");
            ConsoleKeyInfo keyInfo;
            Console.WriteLine("Лабиринт:");
            PrintMaze(maze);
            do
            {
                keyInfo = Console.ReadKey();
                maze.MovePacMan(keyInfo.Key);
                maze.MoveRandomGhost();
                Console.Clear();
                Console.WriteLine("Лабиринт:");
                PrintMaze(maze);
                Thread.Sleep(250);

                if (maze.CheckForGameOver() == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game Over");
                    Thread.Sleep(3000);
                    return;
                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }


        public static void PrintMaze(Maze maze)
        {
            for (int y = 0; y != maze.Height; y++)
            {
                for (int x = 0; x != maze.Width; x++)
                {
                    if (maze[y, x] is null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(maze[y, x].GetSymbol());
                    }
                }
                Console.WriteLine(" ");
            }

        }
    }

    public class MazeLoader
    {
        public Maze LoadMaze(String FileName)
        {
            string[] masiv = File.ReadAllLines(FileName);
            Maze maze = new Maze(masiv.Length, masiv.Length);
            for (int y = 0; y != masiv.GetLength(0); y++)
            {
                for (int x = 0; x != masiv.GetLength(0); x++)
                {
                    switch (masiv[y][x])
                    {
                        case 'p':
                            maze[y, x] = new PcMan();
                            break;
                        case '*':
                            maze[y, x] = new Point();
                            break;
                        case '|':
                            maze[y, x] = new Wall();
                            break;
                        case 'G':
                            maze[y, x] = new RandomGhost();
                            break;
                    }
                }
            }
            return maze;
        }
    }
    public class PcMan : GameObject
    {
        public override char GetSymbol()
        {
            return 'p';
        }
    }
    public class GameObject
    {
        protected int x, y;
        public virtual char GetSymbol()
        {
            return 'x';
        }
    }

    public class Point : GameObject
    {
        public override char GetSymbol()
        {
            return '*';
        }
    }

    public class Ghost : GameObject
    {
        public override char GetSymbol()
        {
            return 'G';
        }
    }

    public class SmartGhost : Ghost
    {

    }

    public class RandomGhost : Ghost
    {

    }

    public class Wall : GameObject
    {
        public override char GetSymbol()
        {
            return '|';
        }
    }

    public class Maze
    {
        private GameObject[,] maze;
        public int Width { get => maze.GetLength(1); }
        public int Height { get => maze.GetLength(0); }
        public GameObject this[int y, int x] { get => maze[y, x]; set => maze[y, x] = value; }
        public Maze(int height, int width)
        {
            maze = new GameObject[height, width];

        }

        public bool CheckForGameOver()
        {
            foreach (GameObject obj in maze)
            {
                if (obj is PcMan)
                {
                    return false;
                }
            }
            return true;
        }

        private bool MoveObject(int startY, int startX, int endY, int endX)
        {
            if (IsWithinBounds(endY, endX) && this[endY, endX] is not Wall)
            {
                this[endY, endX] = this[startY, startX];
                this[startY, startX] = null;

                return true;
            }
            
            return false;
        }
        private bool IsWithinBounds(int y, int x)
        {
            return y >= 0 && x >= 0 && y < Height && x < Width;
        }
        public void MovePacMan(ConsoleKey key)
        {
            int pcx = -1;
            int pcy = -1;
            for (int y = 0; y != Height; y++)
            {
                for (int x = 0; x != Width; x++)
                {
                    if (this[y, x] is PcMan)
                    {
                        pcy = y;
                        pcx = x;
                        break;
                    }
                }
            }
            switch (key)
            {
                case ConsoleKey.W:
                    MoveObject(pcy, pcx, pcy - 1, pcx);
                    break;
                case ConsoleKey.A:
                    MoveObject(pcy, pcx, pcy, pcx - 1); 
                    break;
                case ConsoleKey.S:
                    MoveObject(pcy, pcx, pcy + 1, pcx); 
                    break;
                case ConsoleKey.D:
                    MoveObject(pcy, pcx, pcy, pcx + 1); 
                    break;
            }




        }
        public void MoveRandomGhost()
        {
            for (int y = 0; y != Height; y++)
            {
                for (int x = 0; x != Width; x++)
                {
                    if (this[y, x] is RandomGhost)
                    {
                        int rgx = x;
                        int rgy = y;

                        bool flag = true;
                        Random randomizer = new Random();

                        while (flag)
                        {
                            // 0 - вверх, 1 - вниз, 2 - влево, 3 - вправо
                            int direction = randomizer.Next(4);

                            switch (direction)
                            {
                                case 0:
                                    if (MoveObject(rgy, rgx, rgy - 1, rgx) == true)
                                        flag = false;
                                    break;
                                case 1:
                                    if (MoveObject(rgy, rgx, rgy, rgx - 1) == true)
                                        flag = false;
                                    break;
                                case 2:
                                    if(MoveObject(rgy, rgx, rgy + 1, rgx) == true)
                                        flag = false;
                                    break;
                                case 3:
                                    if (MoveObject(rgy, rgx, rgy, rgx + 1) == true)
                                        flag = false;
                                    break;
                            }


                        }
                        
                        break;
                    }
                }
            }
           
        }
    }
}
