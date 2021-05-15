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
            isJump = false;
            isLeft = false;
            isRight = false;
            var path = player.Location - Location;
            if (path.Length >= 20 && path.Length < 200)
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
            else if (path.Length >= 200)
                OrdinaryMove();
        }

        private void OrdinaryMove()
        {
            timer.Tick += (sender, args) => { action = (random.Next(0, 4) + GetHashCode()) % 4; };
            if (action == 0)
                isLeft = true;
            if (action == 1)
                isRight = true;
            if (action == 2)
            {
                isJump = false;
                isLeft = false;
                isRight = false;
            }
            if (action == 3)
                isJump = true;
        }
    }
}
