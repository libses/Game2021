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

        public void Moving(Player player)
        {
            isJump = false;
            isLeft = false;
            isRight = false;
            var path = player.Location - Location;
            if (path.Length >= 20)
            {
                if (path.X > 0 && path.Y >= 0)
                    isRight = true;
                if (path.X < 0 && path.Y < 0)
                {
                    isJump = true;
                    isRight = true;
                }
                if (path.X > 0 && path.Y < 0)
                {
                    isJump = true;
                    isRight = true;
                }
                if (path.X < 0 && path.Y >= 0)
                    isLeft = true;
            }
        }
    }
}
