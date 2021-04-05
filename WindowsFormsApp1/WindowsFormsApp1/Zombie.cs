using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Zombie : Enemy
    {
        public Zombie(int HP, Vector location, double width, double height) : base(HP, location, width, height)
        {
        }
    }
}
