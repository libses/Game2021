using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Physics
    {
        public Level level;
        public const double g = 0.01;

        public Physics(Level lvl) 
        {
            level = lvl;
        }

        public void DoGravity(IEntity entity)
        {
            entity.ChangeLocation(new Vector(entity.Location.X, entity.Location.Y + entity.Velocity.Y + g / 2));
            entity.ChangeVelocity(new Vector(entity.Velocity.X, entity.Velocity.Y + g));
        }

        public bool Collide(IEntity entity)
        {
            var position = entity.Location;
            var x = Math.Round(position.X);
            var y = Math.Round(entity.Hitbox.LB.Y);
            if (Math.Abs(y - entity.Hitbox.LB.Y) < 0.16)
                return level.CurrentLevel[(int)x, (int)y] == Block.Ground;
            return false;
        }

        public void Iterate() 
        {
            foreach (var entity in level.entities)
            {
                if (!Collide(entity))
                {
                    DoGravity(entity);
                }
                else
                {
                    var veloX = entity.Velocity.X;
                    entity.ChangeVelocity(new Vector(veloX, 0));
                }
                entity.Invalidate();
            }
        }
    }
}