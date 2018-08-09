using System;
using System.Collections.Generic;
using Snake;

public struct partOfField
{
    public pointInField PointIndField;
    public Point coord;
}
public enum pointInField : byte
{
    empty, snakeHead, snakeTail, food
}
public enum direction : byte
{
    up, left, down, right
}

public class SnakeGameField
{
    public delegate void GameOverHandler(object sender, int score);
    public event GameOverHandler GameOverEvent;
    public int scoreBonus = 10;
    int score = 0;
    bool changedDirectioin = false;
    bool foodEaten = true;
    bool needToAddTail = false;
    ConsoleColor foodColor = ConsoleColor.Red;
    private bool continues = true;
    Point food;
    public bool Continues { get { return continues; } }
    direction Direction = direction.right;
    partOfField[,] gameField;
    public ConsoleColor snakeColore = ConsoleColor.Green;
    SnakeContainer snake;
    int sizeX;
    int sizeY;
    Point nextHeadPos
    {
        get
        {
            Point pos;
            switch (Direction)
            {
                case direction.up:
                    {
                        pos = new Point(snake[0].Y - 1, snake[0].X);
                        break;
                    }
                case direction.left:
                    {
                        pos = new Point(snake[0].Y, snake[0].X - 1);
                        break;
                    }
                case direction.down:
                    {
                        pos = new Point(snake[0].Y + 1, snake[0].X);
                        break;
                    }
                default:
                    {
                        pos = new Point(snake[0].Y, snake[0].X + 1);
                        break;
                    }
            }
            checkPos(ref pos);
            return pos;
        }
    }
    public SnakeGameField() : this(20, 15)
    {
    }
    public SnakeGameField(int x, int y)
    {
        sizeX = x;
        sizeY = y;
        gameField = new partOfField[y, x];
        snake = new SnakeContainer();
        gameField[snake[0].Y, snake[0].X].PointIndField = pointInField.snakeHead;
        foreach (Point p in snake)
        {
            gameField[p.Y, p.X].PointIndField = pointInField.snakeTail;
        }
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                gameField[i, j].coord = new Point(i, j);
            }
        }
    }
    public void SetDirection(direction newDirection)
    {
        if (!changedDirectioin)
        {
            if ((Direction == direction.down && newDirection != direction.up) ||
                (Direction == direction.up && newDirection != direction.down) ||
                (Direction == direction.left && newDirection != direction.right) ||
                (Direction == direction.right && newDirection != direction.left))
                Direction = newDirection;
            changedDirectioin = true;
        }
    }
    void checkPos(ref Point pointToCheck)
    {
        if (pointToCheck.X > sizeX - 1)
            pointToCheck.X = 0;
        if (pointToCheck.X < 0)
            pointToCheck.X = sizeX - 1;
        if (pointToCheck.Y > sizeY - 1)
            pointToCheck.Y = 0;
        if (pointToCheck.Y < 0)
            pointToCheck.Y = sizeY - 1;
    }
    public void MoveSnake()
    {
        changedDirectioin = true;
        CheckNextPos();
        ReprintSnake();
        if (needToAddTail)
            snake.newTailSpot = new Point(snake[snake.length - 1].Y, snake[snake.length - 1].X);
        for (int i = snake.length - 1; i > 0; i--)
        {
            snake[i] = snake[i - 1];
        }
        if (needToAddTail)
        {
            snake.AddTail(new Point(snake.newTailSpot.Y, snake.newTailSpot.X));
            needToAddTail = false;
        }
        snake[0] = nextHeadPos;
        if (snake[0].X == food.X && snake[0].Y == food.Y)
        {
            foodEaten = true;
            score += scoreBonus;
            needToAddTail = true;
        }
        if (foodEaten)
            createFood();
        changedDirectioin = false;
    }
    private void CheckNextPos()
    {
        Point next = nextHeadPos;
        foreach (Point p in snake)
        {
            if (p.X == next.X && p.Y == next.Y)
            {
                continues = false;
                GameOverEvent(this, score);
            }
        }
    }

    private void PrintFood()
    {
        Console.ForegroundColor = foodColor;
        Console.SetCursorPosition(food.X * 2, food.Y);
        Console.Write("ff");
        Console.ForegroundColor = ConsoleColor.Black;
    }

    private void ReprintSnake()
    {
        gameField[snake[0].Y, snake[0].X].PointIndField = pointInField.snakeTail;
        gameField[nextHeadPos.Y, nextHeadPos.X].PointIndField = pointInField.snakeHead;
        Console.ForegroundColor = snakeColore;
        Console.SetCursorPosition(nextHeadPos.X * 2, nextHeadPos.Y);
        Console.Write("oo");
        Console.ForegroundColor = ConsoleColor.Gray;
        gameField[snake[snake.length - 1].Y, snake[snake.length - 1].X].PointIndField = pointInField.empty;
        Console.SetCursorPosition(snake[snake.length - 1].X * 2, snake[snake.length - 1].Y);
        Console.Write("--");
        Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
    }
    public void DrawGame()
    {
        ConsoleColor gray = ConsoleColor.Gray;
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                switch (gameField[i, j].PointIndField)
                {
                    case pointInField.empty:
                        {
                            Console.Write("--");
                            break;
                        }
                    case pointInField.snakeHead:
                        {
                            Console.ForegroundColor = snakeColore;
                            Console.Write("oo");
                            Console.ForegroundColor = gray;
                            break;
                        }

                    case pointInField.snakeTail:
                        {
                            Console.ForegroundColor = snakeColore;
                            Console.Write("oo");
                            Console.ForegroundColor = gray;
                            break;
                        }
                }
            }
            Console.Write("\n");
        }
    }
    void createFood()
    {
        Random rnd = new Random();
        List<Point> listOfEmptySpots = new List<Point>();
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
                if (gameField[i, j].PointIndField == pointInField.empty)
                    listOfEmptySpots.Add(gameField[i, j].coord);
        }
        food = listOfEmptySpots[rnd.Next(listOfEmptySpots.Count)];
        PrintFood();
        foodEaten = false;
    }

}
