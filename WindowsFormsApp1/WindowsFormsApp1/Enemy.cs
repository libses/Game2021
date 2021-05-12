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
        public Enemy(int HP, Vector location, int width, int height, Bitmap sprite) : base(HP, location, width, height, sprite)
        {
            
        }

        public override void Fight(Entity entity)
        {
            entity.ReceiveDamage(1);
        }
    }
}
