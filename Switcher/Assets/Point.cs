using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Point
    {
        public int X;
        public int Y;

        public Point() { }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public Point Move(int direction)
        {
            var newPoint = new Point(this);
            switch (direction)
            {
                case 0:
                    newPoint.X++;
                    break;
                case 1:
                    newPoint.Y--;
                    break;
                case 2:
                    newPoint.X--;
                    break;
                case 3:
                    newPoint.Y++;
                    break;
            }
            return newPoint;
        }
        
        public int GetDirection(Point target)
        {
            if (X < target.X) return 0;
            else if (X > target.X) return 2;
            if (Y < target.Y) return 3;
            else if (Y > target.Y) return 1;

            return 0;
        }

        public List<Point> GetNeighbors(int width, int height)
        {
            var list = new List<Point>();

            for(var i = 0; i < 4; i++)
            {
                var point = Move(i);
                if(point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height)
                {
                    list.Add(point);
                }
            }

            return list;
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Point p = obj as Point;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == p.X) && (Y == p.Y);
        }

        public bool Equals(Point p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == p.X) && (Y == p.Y);
        }
    }
}
