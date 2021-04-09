using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Physics
    {
        public void DoGravity(IEntity entity)
        {
            entity.ChangeLocation(new Vector(entity.Location.X, entity.Location.Y + entity.Velocity.Y + g / 2));
            entity.ChangeVelocity(new Vector(entity.Velocity.X, entity.Velocity.Y + g));
        }
        public Level level;
        public Physics(Level lvl) {
            level = lvl;
        }
        public const double g = 0.01;
        public void Iterate() {
            foreach (var entity in level.entities)
            {
                DoGravity(entity);
                entity.Invalidate();
            }

        }
    }
}