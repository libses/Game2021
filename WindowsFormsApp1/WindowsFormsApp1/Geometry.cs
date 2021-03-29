using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace WindowsFormsApp1
{
    class Vector : Point
    {
        public Vector(float x, float y) : base(x, y)
        {
            X = x;
            Y = y;
        }
    }

    class Segment
    {
        public Point Start;
        public Point End;
    }

    class Point
    {
        public float X;
        public float Y;

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    [TestFixture]
    class Tests
    {
        [Test]
        public void InitializationTest()
        {
            var p = new Point(1,1);
            var v = new Vector(1, 1);
            Assert.IsTrue(p is Point);
            Assert.IsTrue(v is Vector);
        }
    }
}