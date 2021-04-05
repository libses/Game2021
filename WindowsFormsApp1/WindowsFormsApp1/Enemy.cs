using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Enemy : Entity
    {
        public Enemy(int HP, Vector location, double width, double height, Image sprite) : base(HP, location, width, height, sprite)
        {

        }
    }
}
