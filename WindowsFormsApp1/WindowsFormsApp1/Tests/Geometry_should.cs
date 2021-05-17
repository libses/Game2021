using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace WindowsFormsApp1
{
    [TestFixture]
    class Geometry_should
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
    }
}