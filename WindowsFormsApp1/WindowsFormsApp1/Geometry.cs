using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace WindowsFormsApp1
{

    class Segment
    {
        public Vector Start;
        public Vector End;
    }

    public struct Vector
    {
        public float X;
        public float Y;
        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y);
            }
        }

        public Vector(float x, float y)
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
        public static Vector operator *(Vector vector, float t)
        {
            return new Vector(vector.X * t, vector.Y * t);
        }
        public static Vector operator *(float t, Vector vector) => vector * t;

        public static float operator *(Vector first, Vector second)
        {
            return first.X * second.X + first.Y * second.Y;
        }
    }

    [TestFixture]
    class Tests
    {
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
        public void VectorOnFloat()
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
    }
}