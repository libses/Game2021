using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Demon : Enemy
    {
        public Demon(int HP, Vector location, int width, int height, Bitmap sprite) : base(HP, location, width, height, sprite)
        {
        }
    }
}
