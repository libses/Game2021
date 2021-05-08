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
        public const int g = 1;
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
            var location = entity.Location;
            var velocity = entity.Velocity;
            entity.ChangeLocation(new Vector(entity.Location.X + entity.Velocity.X, entity.Location.Y + entity.Velocity.Y + g / 2));
            entity.ChangeVelocity(new Vector(0, velocity.Y + g));
        }

        public IEnumerable<string> CollideObstacle(IEntity entity, Block block)
        {
            var LB = entity.Hitbox.LB;
            var RB = entity.Hitbox.RB;
            var LT = entity.Hitbox.LT;
            var RT = entity.Hitbox.RT;
            if (map[LB.X + 4, LB.Y + entity.Velocity.Y] == block || map[RB.X - 4, RB.Y + entity.Velocity.Y] == block)
                yield return "down";
            if (map[LB.X - 5, LB.Y - 1] == block || map[LT.X - 5, LT.Y] == block)
                yield return "left";
            if (map[RB.X + 5, RB.Y - 1] == block || map[RT.X + 5, RT.Y] == block)
                yield return "right";
            if (map[LT.X, LT.Y - 1] == block || map[RT.X - 1, RT.Y - 1] == block) 
                yield return "up";
        }

        public void Iterate() 
        {
            foreach (var entity in level.entities)
            {
                var obstacles = CollideObstacle(entity, Block.Ground)
                    .Concat(CollideObstacle(entity, Block.Bound))
                    .ToList();
                if (entity.isRight && !obstacles.Contains("right"))
                {
                    DoRun(entity, 1);
                }
                if (entity.isLeft && !obstacles.Contains("left"))
                {
                    DoRun(entity, -1);
                }
                if (entity.isJump && !obstacles.Contains("up") && obstacles.Contains("down"))
                {
                    entity.Jump(this);
                }   
                if (!obstacles.Contains("down"))
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