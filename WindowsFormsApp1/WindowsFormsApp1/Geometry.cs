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
    }
}