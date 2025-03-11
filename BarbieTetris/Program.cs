using System;
using System.Collections.Generic;
using System.Threading;

class TetrisGame
{
    static int width = 10;
    static int height = 20;
    static int[,] field = new int[height, width];
    static List<int[,]> figures = new List<int[,]>()
    {
        new int[,] { {1, 1, 1, 1} }, // Линия
        new int[,] { {1, 1}, {1, 1} }, // Квадрат
        new int[,] { {0, 1, 0}, {1, 1, 1} }, // T
        new int[,] { {1, 1, 0}, {0, 1, 1} }, // Z
        new int[,] { {0, 1, 1}, {1, 1, 0} }  // S
    };

    static int[,] currentFigure;
    static int x, y;
    static bool gameOver = false;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "🎀 Barbie Tetris 🎀";
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.BackgroundColor = ConsoleColor.White;
        Console.Clear();

        while (!gameOver)
        {
            SpawnFigure();
            while (true)
            {
                DrawField();
                HandleInput();
                Thread.Sleep(300);

                if (Collides())
                {
                    y--; 
                    MergeFigure();
                    ClearLines();
                    CheckGameOver();
                    break;
                }
                y++;
            }
        }
        Console.SetCursorPosition(0, height);
        Console.WriteLine("🎀 Game Over! Press any key to restart 🎀");
        Console.ReadKey();
        Console.Clear();
        Main();
    }

    static void SpawnFigure()
    {
        Random rand = new Random();
        currentFigure = figures[rand.Next(figures.Count)];
        x = width / 2 - currentFigure.GetLength(1) / 2;
        y = 0;

        if (Collides())
        {
            gameOver = true;
        }
    }

    static bool Collides()
    {
        for (int i = 0; i < currentFigure.GetLength(0); i++)
        {
            for (int j = 0; j < currentFigure.GetLength(1); j++)
            {
                if (currentFigure[i, j] == 1)
                {
                    int newX = x + j;
                    int newY = y + i;
                    if (newY >= height || newX < 0 || newX >= width || (newY >= 0 && field[newY, newX] == 1))
                        return true;
                }
            }
        }
        return false;
    }

    static void MergeFigure()
    {
        for (int i = 0; i < currentFigure.GetLength(0); i++)
        {
            for (int j = 0; j < currentFigure.GetLength(1); j++)
            {
                if (currentFigure[i, j] == 1)
                {
                    int newY = y + i;
                    int newX = x + j;
                    if (newY >= 0 && newY < height && newX >= 0 && newX < width)
                    {
                        field[newY, newX] = 1;
                    }
                }
            }
        }
    }

    static void ClearLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            bool full = true;
            for (int j = 0; j < width; j++)
            {
                if (field[i, j] == 0)
                {
                    full = false;
                    break;
                }
            }
            if (full)
            {
                for (int k = i; k > 0; k--)
                {
                    for (int j = 0; j < width; j++)
                    {
                        field[k, j] = field[k - 1, j];
                    }
                }
                for (int j = 0; j < width; j++)
                {
                    field[0, j] = 0;
                }
                i++;
            }
        }
    }

    static void CheckGameOver()
    {
        for (int i = 0; i < width; i++)
        {
            if (field[1, i] == 1) 
            {
                gameOver = true;
                return;
            }
        }
    }

    static void HandleInput()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    x--;
                    if (Collides()) x++;
                    break;
                case ConsoleKey.RightArrow:
                    x++;
                    if (Collides()) x--;
                    break;
                case ConsoleKey.DownArrow:
                    y++;
                    if (Collides()) y--;
                    break;
                case ConsoleKey.UpArrow:
                    Rotate();
                    if (Collides()) Rotate(); 
                    break;
            }
        }
    }

    static void Rotate()
    {
        int[,] rotated = new int[currentFigure.GetLength(1), currentFigure.GetLength(0)];
        for (int i = 0; i < currentFigure.GetLength(0); i++)
            for (int j = 0; j < currentFigure.GetLength(1); j++)
                rotated[j, currentFigure.GetLength(0) - 1 - i] = currentFigure[i, j];
        currentFigure = rotated;
    }

    static void DrawField()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                bool isFigureBlock = false;
                for (int fi = 0; fi < currentFigure.GetLength(0); fi++)
                {
                    for (int fj = 0; fj < currentFigure.GetLength(1); fj++)
                    {
                        if (currentFigure[fi, fj] == 1 && i == y + fi && j == x + fj)
                        {
                            isFigureBlock = true;
                            break;
                        }
                    }
                }
                Console.Write(isFigureBlock || field[i, j] == 1 ? "■ " : ". ");
            }
            Console.WriteLine();
        }
    }
}
