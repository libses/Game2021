using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class Coin
    {
        public Vector Location;
        public Bitmap Sprite;

        public Coin(Vector location, Bitmap sprite)
        {
            Location = location;
            this.Sprite = sprite;
        }
    }
}
