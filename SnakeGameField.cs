using System;

public class SnakeGameField
{
    public enum pointInField
    {
        empty,snakeHead,snakeTail,food
    }
    public enum direction
    {
        up,left,down,right
    }
    direction Direction = direction.left;
    pointInField[,] gameField;
    public SnakeGameField()
    {
        gameField = new pointInField[15, 20];
        gameField[7, 11] = pointInField.snakeHead;
        gameField[7, 10] = pointInField.snakeTail;
        gameField[7, 9] = pointInField.snakeTail;
    }
}
