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
        public Rectangle(double width, double heigth, Vector center) {
            RB = center + new Vector(width, heigth); //Как и везде, ось y направлена вниз
            LB = center + new Vector(-width, heigth);
            LT = center + new Vector(-width, -heigth);
            RT = center + new Vector(width, -heigth);
        }
    }
    class Segment
    {
        public Vector Start;
        public Vector End;
    }

    public struct Vector
    {
        public double X;
        public double Y;
        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        public Vector(double x, double y)
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
            return new Vector(vector.X * t, vector.Y * t);
        }
        public static Vector operator *(double t, Vector vector) => vector * t;

        public static double operator *(Vector first, Vector second)
        {
            return first.X * second.X + first.Y * second.Y;
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
                result = result + vector;
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
    }

    [TestFixture]
    class Tests
    {
        [Test]
        public void DoubleVectorIsEqual()
        {
            var vector = new Vector(3999945646633, 0);
            vector.X = vector.X * vector.X;
            vector.X = Math.Sqrt(vector.X);
            Assert.AreEqual(new Vector(3999945646633, 0), vector);
        }
        [Test]
        public void IsTwoVectorsEqual()
        {
            var vector1 = new Vector(1, 1);
            var vector2 = new Vector(1, 1);
            Assert.AreEqual(vector1, vector2);
        }
        [Test]
        public void VectorsPlus()
        {
            var vector1 = new Vector(1, 1);
            var vector2 = new Vector(1, 1);
            Assert.AreEqual(vector2 + vector1, new Vector(2, 2));
        }
        [Test] 
        public void VectorMinus()
        {
            var vector1 = new Vector(1, 1);
            var vector2 = new Vector(1, 1);
            Assert.AreEqual(vector2 - vector1, new Vector(0, 0));
        }
        [Test]
        public void VectorOnDouble()
        {
            var vector1 = new Vector(1, 1);
            Assert.AreEqual(vector1*2, new Vector(2, 2));
        }
        [Test] 
        public void VectorOnVectorRightAngle()
        {
            var vector1 = new Vector(1, 0);
            var vector2 = new Vector(0, 1);
            Assert.AreEqual(vector2 * vector1, 0);
        }
        [Test]
        public void VectorLength()
        {
            var vector = new Vector(3, 4);
            Assert.AreEqual(vector.Length, 5, 0.0001);
        }
        [Test]
        public void RectangleCorrect()
        {
            var center = new Vector(0,0);
            var width = 1;
            var heigth = 1;
            var rect = new Rectangle(width, heigth, center);
            Assert.AreEqual(rect.RT, new Vector(1, -1));
            Assert.AreEqual(rect.LB, new Vector(-1, 1));
            Assert.AreEqual(rect.LT, new Vector(-1, -1));
            Assert.AreEqual(rect.RB, new Vector(1, 1));
        }
        [Test]
        public void SumIsNice()
        {
            var res = Vector.SumAllVectors(new Vector(1, 1), new Vector(-1, -1), new Vector(0, 0), new Vector(2, 2));
            Assert.AreEqual(res, new Vector(2, 2));
        }
        [Test] 
        public void Ort()
        {
            var vector = new Vector(3, 0);
            Assert.AreEqual(vector.GetOrt(), new Vector(1, 0));
            var vector1 = new Vector(3, 4);
            Assert.AreEqual(new Vector(0.6, 0.8), vector1.GetOrt());
        }
    }
}