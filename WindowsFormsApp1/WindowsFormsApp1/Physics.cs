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
        public const double g = 1;
        public Block[,] map;
        
        public Physics(Level lvl) 
        {
            level = lvl;
            map = new IntedMap(lvl.Map, 20).intMap;
        }

        public void DoRun(IEntity entity, int direction)
        {
            if (true)
            {
                var n = direction*5;
                entity.ChangeVelocity(new Vector((int)(entity.Velocity.X * 0.6 + n), entity.Velocity.Y));
                entity.ChangeLocation(new Vector(entity.Location.X + entity.Velocity.X, entity.Location.Y));
            }
        }
        public void DoGravity(IEntity entity)
        {
            entity.ChangeLocation(new Vector(entity.Location.X + entity.Velocity.X, (int)(entity.Location.Y + entity.Velocity.Y + g / 2)));
            entity.ChangeVelocity(new Vector(entity.Velocity.X, (int)(entity.Velocity.Y + g)));
        }

        public bool Collide(IEntity entity)
        {
            var LB = entity.Hitbox.LB;
            var RB = entity.Hitbox.RB;
            return map[LB.X, LB.Y + 1] == Block.Ground || map[RB.X, RB.Y+1] == Block.Ground;
        }

        public void Iterate() 
        {
            foreach (var entity in level.entities)
            {
                if (entity.isRight)
                {
                    DoRun(entity, 1);
                }
                if (entity.isLeft)
                {
                    DoRun(entity, -1);
                }
                if (entity.isJump)
                {
                    entity.Jump(this);
                }
                if (!Collide(entity))
                {
                    DoGravity(entity);
                }
                else
                {
                    var veloX = entity.Velocity.X;
                    entity.ChangeVelocity(new Vector(veloX, 0));
                }
                DoRun(entity, 0);
                entity.Invalidate();
            }
        }
    }
}