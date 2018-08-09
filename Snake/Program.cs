using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake
{
    class Program
    {
        static bool play = true;
        static int oldWX = 30;
        static int oldWY = 30;
        static int gameX = 30, gameY = 30;
        delegate void DirectionKeyHandler(direction dir);
        static event DirectionKeyHandler DirectionEvent;
        static int delay = 100;
        static void Main(string[] args)
        {
            if (args.Length!=0)
            {
                gameX = Convert.ToInt32(args[0]);
                gameY = Convert.ToInt32(args[1]);
            }
            oldWX = Console.WindowWidth;
            oldWY = Console.WindowHeight;
            SnakeGameField game = new SnakeGameField(gameX,gameY);
            Console.SetWindowSize(gameX * 2, gameY);
            Thread keyReaderThread = new Thread(new ThreadStart(KeyReader));
            keyReaderThread.Name = "KeyRegister";
            keyReaderThread.Start();
            DirectionEvent += game.SetDirection;
            game.GameOverEvent += (gameSender,score) => {
                Console.SetWindowSize(oldWX, oldWY);
                Console.Clear();
                Console.WriteLine("Game over! Your final score is {0}.",score);
                play = false;
                        };
            Console.Clear();
            game.DrawGame();
            while (game.Continues)
            {
                Tick(game, delay);
            }
        }

        static void Tick(SnakeGameField game, int miliseconds)
        {
            game.MoveSnake();
            Thread.Sleep(miliseconds);
        }
        static void KeyReader()
        {
            while (play)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey k = Console.ReadKey(true).Key;
                    switch (k)
                    {
                        case ConsoleKey.Spacebar:
                            {
                                delay -= 25;
                                if (delay < 25)
                                    delay = 25;
                                break;
                            }
                        case ConsoleKey.Enter:
                            {
                                delay += 25;
                                if (delay > 600)
                                    delay = 600;
                                break;
                            }
                        case ConsoleKey.UpArrow:
                            {
                                DirectionEvent?.Invoke(direction.up);
                                break;
                            }
                        case ConsoleKey.LeftArrow:
                            {
                                DirectionEvent?.Invoke(direction.left);
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                DirectionEvent?.Invoke(direction.down);
                                break;
                            }
                        case ConsoleKey.RightArrow:
                            {
                                DirectionEvent?.Invoke(direction.right);
                                break;
                            }
                        case ConsoleKey.Escape:
                            {

                                break;
                            }
                    }
                }
            }
        }
    }
}
