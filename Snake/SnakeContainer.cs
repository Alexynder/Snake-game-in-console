using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public struct Point
    {
        public int X;
        public int Y;
        public Point(int y,int x)
        {
            X = x;
            Y = y;
        }
    }
    public class SnakeContainer:IEnumerable
    {
        public Point newTailSpot = new Point();
        public int length { get { return snakeBlocks.Count; } }
        public void AddTail(Point tail)
        {
            snakeBlocks.Add(tail);
        }
        public Point this[int index]
        {
            get { return snakeBlocks[index]; }
            set { snakeBlocks[index] = value; }
        }
        List<Point> snakeBlocks;
        public SnakeContainer()
        {
            snakeBlocks = new List<Point>();
            snakeBlocks.Add( new Point(7, 11));
            snakeBlocks.Add( new Point(7, 10));
            snakeBlocks.Add( new Point(7, 9));
        }

        public IEnumerator GetEnumerator()
        {
            return snakeBlocks.GetEnumerator();
        }
    }
}
