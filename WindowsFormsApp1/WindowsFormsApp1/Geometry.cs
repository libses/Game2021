using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace WindowsFormsApp1
{
    public class Rectangle
    {
        public Vector LT { get; set; }
        public Vector RT { get; set; }
        public Vector LB { get; set; }
        public Vector RB { get; set; }
        public Rectangle(Vector LT, Vector RT, Vector LB, Vector RB)
        {
            this.LT = LT;
            this.RT = RT;
            this.LB = LB;
            this.RB = RB;
        }
        public Rectangle(int width, int heigth, Vector center) {
            RB = center + new Vector(width, heigth); //Как и везде, ось y направлена вниз
            LB = center + new Vector(-width, heigth);
            LT = center + new Vector(-width, -heigth);
            RT = center + new Vector(width, -heigth);
        }
    }
    public class Segment
    {
        public Vector Start;
        public Vector End;
        public Segment(Vector start, Vector end)
        {
            Start = start;
            End = end;
        }

        public override bool Equals(object other1)
        {
            var other = (Segment)other1;
            return Start.Equals(other.Start) && End.Equals(other.End);
        }
        public override int GetHashCode()
        {
            return Start.GetHashCode() + End.GetHashCode();
        }
    }

    public struct Vector
    {
        public int X;
        public int Y;
        public int Length
        {
            get
            {
                return (int)Math.Sqrt(X * X + Y * Y);
            }
        }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }
        public static Vector operator -(Vector first, Vector second)
        {
            return new Vector(first.X - second.X, first.Y - second.Y);
        }
        public override string ToString()
        {
            return "x:" + X + " y:" + Y;
        }
        public static Vector operator *(Vector vector, double t)
        {
            return new Vector((int)(vector.X * t), (int)(vector.Y * t));
        }
        public static Vector operator *(double t, Vector vector) => vector * t;

        public static int operator *(Vector first, Vector second)
        {
            return (int)(first.X * second.X + first.Y * second.Y);
        }
        public double GetAngleWith(Vector other)
        {
            return Math.Acos((this * other) / (this.Length * other.Length));
        }
        public static Vector SumAllVectors(params Vector[] vectors)
        {
            var result = new Vector(0, 0);
            foreach (var vector in vectors)
            {
                result += vector;
            }
            return result;
        }
        public Vector GetOrt()
        {
            var length = this.Length;
            if (length == 0)
            {
                return new Vector(0, 0);
            }
            return this * (1 / length);
        }
        public override bool Equals(object other1)
        {
            var other = (Vector)other1;
            return (this.X - other.X < 0.0001) && (this.Y - other.Y < 0.0001);
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() + 2000*Y.GetHashCode();
        }
    }
}