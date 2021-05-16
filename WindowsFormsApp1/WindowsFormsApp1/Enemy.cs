using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Enemy : Entity
    {
        private Random random = new Random(DateTime.Now.Millisecond);
        private Timer timer = new Timer();
        private int action;

        public Enemy(int HP, Vector location, int width, int height, Bitmap sprite, Dictionary<string, Bitmap[]> animations) : base(HP, location, width, height, sprite, animations)
        {
            timer.Interval = 1000;
            timer.Start();
        }

        public void Moving(Player player)
        {
            IsJump = false;
            IsLeft = false;
            IsRight = false;
            var path = player.Location - Location;
            if (path.Length >= 20 && path.Length < 200)
            {
                if (path.X > 0 && path.Y >= 0)
                    IsRight = true;
                if (path.X < 0 && path.Y < 0)
                {
                    IsJump = true;
                    IsRight = true;
                }
                if (path.X > 0 && path.Y < 0)
                {
                    IsJump = true;
                    IsRight = true;
                }
                if (path.X < 0 && path.Y >= 0)
                    IsLeft = true;
            }
            else if (path.Length >= 200)
                OrdinaryMove();
        }

        private void OrdinaryMove()
        {
            timer.Tick += (sender, args) => { action = (random.Next(0, 4) + GetHashCode()) % 4; };
            if (action == 0)
                IsLeft = true;
            if (action == 1)
                IsRight = true;
            if (action == 2)
            {
                IsJump = false;
                IsLeft = false;
                IsRight = false;
                currentSprite = originalSprite;
            }
            if (action == 3)
                IsJump = true;
        }
    }
}
