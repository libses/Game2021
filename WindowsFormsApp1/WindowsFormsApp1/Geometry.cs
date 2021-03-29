using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Vector : Point
    {
        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    class Segment
    {
        Point Start;
        Point End;
    }

    class Point
    {
        public float X;
        public float Y;
    }
}