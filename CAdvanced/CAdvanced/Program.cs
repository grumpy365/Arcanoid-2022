using System;
using System.Threading;

namespace Arcanoid
{
    class Program
    {
        static void Main(string[] args)
        {
            //ARCANOID (FUNCTIONAL)

            //Console font - Point 8x9

            //Greeting window
            const int STARTHEIGHT = 20;
            const int STARTWIDTH = 70;

            Console.WindowHeight = STARTHEIGHT;
            Console.WindowWidth = STARTWIDTH;
            string GreetingStr = "Press any key to start!";
            Console.SetCursorPosition(STARTWIDTH / 2 - GreetingStr.Length / 2, STARTHEIGHT / 2); //center of screen
            Console.WriteLine(GreetingStr);
            Console.ReadKey(true);

            //Game window
            const int WIDTH = 60;
            const int HEIGHT = 50;
            int score = 0;
            char graphicSymbol = 'X';

            Console.WindowWidth = WIDTH;
            Console.WindowHeight = HEIGHT + 2;

            //render
            bool gameOver = false;

            int ballX = WIDTH / 2;
            int ballY = HEIGHT / 2;
            bool UpRight = true;
            bool UpLeft = false;
            bool DownRight = false;
            bool DownLeft = false;
            char ballSymbol = 'O';

            int centerPlatformX = WIDTH / 2;
            int centerPlatformY = HEIGHT - 4;
            char platformSymbol = '-';

            Random random = new Random();
            bool isRestarted = true;
            int[] obstaclesX = new int[40];
            int[] obstaclesY = new int[40];
            int ofset = 3;


            while (!gameOver)
            {
                Thread.Sleep(1);

                Console.SetCursorPosition(0, HEIGHT + 1);
                Console.Write("SCORE: " + score);
                //game field
                if (Console.KeyAvailable)
                {
                    Thread.Sleep(1);
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case (ConsoleKey.A):
                        {
                            if(centerPlatformX > 4)
                                centerPlatformX--;
                            break;
                        }
                        case (ConsoleKey.D):
                        {
                            if(centerPlatformX < WIDTH - 5)
                                centerPlatformX++;
                            break;
                        }
                    }
                }
                if (isRestarted)
                {
                    for (int obstacle = 0; obstacle < obstaclesX.Length; obstacle++)
                    {
                        obstaclesX[obstacle] = random.Next(5, 55);
                    }
                    for (int obstacle = 0; obstacle < obstaclesY.Length; obstacle++)
                    {
                        obstaclesY[obstacle] = random.Next(5, 13);
                    }
                    isRestarted = false;
                }

                for (int i = 0; i < obstaclesX.Length; i++)
                {
                    Console.SetCursorPosition(obstaclesX[i], obstaclesY[i]);
                    if (obstaclesX[i] != 0)
                        Console.Write("#");
                }

                if (DownRight && ballX == WIDTH - 3)
                {
                    DownRight = false;
                    DownLeft = true;
                }
                if (DownRight && ballY == HEIGHT - 2)
                {
                    Restart();
                }
                if(DownRight &&  (ballX == centerPlatformX ||
                            ballX == centerPlatformX + 1 ||
                            ballX == centerPlatformX + 2 ||
                            ballX == centerPlatformX - 1 ||
                            ballX == centerPlatformX - 2) && ballY == centerPlatformY - 1 )
                {
                    DownRight = false;
                    UpRight = true;
                }
                if(DownLeft && ballX == 2)
                {
                    DownLeft = false;
                    DownRight = true;
                }
                if (DownLeft && ballY == HEIGHT - 3)
                {
                    Restart();
                }
                if (DownLeft && (ballX == centerPlatformX ||
                            ballX == centerPlatformX + 1 ||
                            ballX == centerPlatformX + 2 ||
                            ballX == centerPlatformX - 1 ||
                            ballX == centerPlatformX - 2) && ballY == centerPlatformY - 1)
                {
                    DownLeft = false;
                    UpLeft = true;
                }
                if (UpRight && ballX == WIDTH - 3)
                {
                    UpRight = false;
                    UpLeft = true;
                }
                if(UpRight && ballY == 3)
                {
                    UpRight = false;
                    DownRight = true;
                }
                if(UpLeft && ballX == 2)
                {
                    UpLeft = false;
                    UpRight = true;
                }
                if(UpLeft && ballY == 3)
                {
                    UpLeft = false;
                    DownLeft = true;
                }
                for(int obstacle = 0; obstacle < obstaclesX.Length; obstacle++)
                {
                    if(ballX == obstaclesX[obstacle] && ballY == obstaclesY[obstacle] && UpRight)
                    {
                        UpRight = false;
                        DownRight = true;
                        obstaclesX[obstacle] = 0;
                        score++;
                    }
                    if (ballX == obstaclesX[obstacle] && ballY == obstaclesY[obstacle] && UpLeft)
                    {
                        UpLeft = false;
                        DownLeft = true;
                        obstaclesX[obstacle] = 0;
                        score++;
                    }
                    if (ballX == obstaclesX[obstacle] && ballY == obstaclesY[obstacle] && DownRight)
                    {
                        DownRight = false;
                        UpRight = true; 
                        obstaclesX[obstacle] = 0;
                        score++;
                    }
                    if (ballX == obstaclesX[obstacle] && ballY == obstaclesY[obstacle] && DownLeft)
                    {
                        DownLeft = false;
                        UpLeft = true;
                        obstaclesX[obstacle] = 0;
                        score++;
                    }
                }


                if (DownRight)
                {
                    ballY++;
                    ballX++;
                }
                if (DownLeft)
                {
                    ballY++;
                    ballX--;
                }
                if (UpRight)
                {
                    ballY--;
                    ballX++;
                }
                if (UpLeft)
                {
                    ballY--;
                    ballX--;
                }


                Console.SetCursorPosition(0, 0);
                for (int raw = 0; raw < HEIGHT; raw++)
                {
                    for (int col = 0; col < WIDTH; col++)
                    {
                        if (raw == 0 || raw == HEIGHT - 1 || col == 0 || col == WIDTH - 1
                            || raw == 1 || raw == HEIGHT - 2 || col == 1 || col == WIDTH - 2)
                        {
                            Console.Write(graphicSymbol);
                        }
                        else if (col == ballX && raw == ballY)
                        {
                            Console.Write(ballSymbol);
                        }
                        else if (col == centerPlatformX - 2 && raw == centerPlatformY ||
                            col == centerPlatformX - 1 && raw == centerPlatformY ||
                            col == centerPlatformX && raw == centerPlatformY ||
                            col == centerPlatformX + 1 && raw == centerPlatformY ||
                            col == centerPlatformX + 2 && raw == centerPlatformY)
                        {
                            Console.Write(platformSymbol);
                        }
                        else
                        {
                            Console.Write(" ");
                        }  
                    }
                    Console.WriteLine();
                }
            }

            void Restart()
            {
                string gameOverText = "Game Over!!!";
                string waitFor = "Press any key to restart or 'E' to exit";
                Console.SetCursorPosition(WIDTH / 2 - gameOverText.Length / 2, HEIGHT / 2 - 1);
                Console.WriteLine(gameOverText);
                Console.SetCursorPosition(WIDTH / 2 - waitFor.Length / 2, HEIGHT / 2);
                Console.WriteLine(waitFor);
                ConsoleKeyInfo key = Console.ReadKey(false);
                if (key.Key == ConsoleKey.E)
                {
                    gameOver = true;
                    return;
                }
                ballX = WIDTH / 2;
                ballY = HEIGHT / 2;
                DownLeft = false;
                DownRight = false;
                UpRight = true;
                isRestarted = true;
                score = 0;
            }

        }
        
    }
}
