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
        private Block[,] Map;

        public Enemy(int HP, Vector location, int width, int height, Bitmap sprite, 
            Dictionary<string, Bitmap[]> animation, Dictionary<string, Bitmap[]> animationV, Bitmap spriteV, Block[,] map) : base(HP, location, width, height, sprite, animation, animationV, spriteV)
        {
            timer.Interval = 1000;
            timer.Start();
            CurrentGun = new Pistol(Properties.Resources.gun, this);
            Map = map;
        }

        public void Shoot(Player player)
        {
            var r = (player.Location.X - Location.X) / (double)((player.Location - Location).Length);
            var angle = Math.Acos(r);
            CurrentGun.angle = angle;
            CurrentGun.Fire();
        }

        public void Moving(Player player)
        {
            IsJump = false;
            IsLeft = false;
            IsRight = false;
            var path = player.Location - Location;
            var pathDerscrete = BresenhamAlgorithm(Location, player.Location, Map);
            bool hasGround = true;
            //тут чето сломалось
            if (path.Length >= 20 && path.Length < 200 && !hasGround) //для отладки
            {
                //if (random.NextDouble() < 0.7)
                //{
                //    if (path.X > 0 && path.Y >= 0)
                //        IsRight = true;
                //    if (path.X < 0 && path.Y < 0)
                //    {
                //        IsJump = true;
                //        IsRight = true;
                //    }
                //    if (path.X > 0 && path.Y < 0)
                //    {
                //        IsJump = true;
                //        IsRight = true;
                //    }
                //    if (path.X < 0 && path.Y >= 0)
                //        IsLeft = true;
                //}
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
            else if (path.Length >= 200 || hasGround)
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
        
        public IEnumerable<Block> BresenhamAlgorithm(Vector start, Vector end, Block[,] map)
        {
            start = (start - new Vector(10, 10)) * 0.05;
            end = (end - new Vector(10, 10)) * 0.05;
            var x0 = start.X;
            var x1 = end.X;
            var y0 = start.Y;
            var y1 = end.Y;
            var dx = Math.Abs(x1 - x0);
            var sx = x0 < x1 ? 1 : -1;
            var dy = -Math.Abs(y1 - y0);
            var sy = y0 < y1 ? 1 : -1;
            var err = dx + dy;  /* error value e_xy */
            while (true)
            {
                if(x0 > 0 && y0 > 0)
                    if (map.GetLength(0) < x0 - 1 && map.GetLength(1) < y0 - 1)
                    {
                        yield return map[x0, y0];
                    }
                if (x0 == x1 && y0 == y1)
                    break;
                var e2 = 2 * err;
                if (e2 >= dy)/* e_xy+e_x > 0 */
                {
                    err += dy;
                    x0 += sx;
                }
                if (e2 <= dx)/* e_xy+e_y < 0 */
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }
    }
}
