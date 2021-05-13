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
                var n = direction*3;
                entity.ChangeVelocity(new Vector((int)(entity.Velocity.X * 0.6 + n), entity.Velocity.Y));
                entity.ChangeLocation(new Vector(entity.Location.X + entity.Velocity.X, entity.Location.Y));
            }
        }
        public void DoGravity(IEntity entity)
        {
            entity.ChangeAcceleration(new Vector(entity.Acceleration.X, entity.Acceleration.Y + g));
            entity.ChangeVelocity(entity.Velocity + entity.Acceleration);
            entity.ChangeLocation(new Vector(entity.Location.X + entity.Velocity.X, entity.Location.Y + entity.Velocity.Y));
        }

        public IEnumerable<string> CollideObstacle(IEntity entity, Block block)
        {
            var LB = entity.Hitbox.LB;
            var RB = entity.Hitbox.RB;
            var LT = entity.Hitbox.LT;
            var RT = entity.Hitbox.RT;
            if (map[LB.X + 4, LB.Y + entity.Acceleration.Y] == block || map[RB.X - 4, RB.Y + entity.Acceleration.Y] == block)
                yield return "down";
            if (map[LB.X - 5, LB.Y - 1] == block || map[LT.X - 5, LT.Y] == block)
                yield return "left";
            if (map[RB.X + 5, RB.Y - 1] == block || map[RT.X + 5, RT.Y] == block)
                yield return "right";
            if (map[LT.X, LT.Y - 1] == block || map[RT.X - 1, RT.Y - 1] == block || map[RB.X, RB.Y - 1] == block) 
                yield return "up";
        }

        public void Iterate() 
        {
            foreach (var entity in level.entities)
            {
                var obstacles = CollideObstacle(entity, Block.Ground)
                    .Concat(CollideObstacle(entity, Block.Bound))
                    .ToList();
                if (entity.isFiring)
                {
                    entity.currentGun.Fire();
                }
                if (entity.currentGun != null)
                {
                    if (entity.isDowningGun)
                    {
                        entity.currentGun.angle += 0.15;
                    }
                    if (entity.isUppingGun)
                    {
                        entity.currentGun.angle -= 0.15;
                    }
                    foreach (var bullet in entity.currentGun.bullets)
                    {
                        if (!bullet.isDead)
                        {
                            bullet.Fly();
                        }
                    }
                }
                if (entity.isRight && !obstacles.Contains("right"))
                {
                    DoRun(entity, 1);
                }
                if (entity.isLeft && !obstacles.Contains("left"))
                {
                    DoRun(entity, -1);
                }
                //if (obstacles.Contains("up") && obstacles.Contains("down"))
                //{
                //    entity.ChangeLocation(new Vector(entity.Location.X, entity.Location.Y - 1));
                //}
                if (entity.isJump && !obstacles.Contains("up") && obstacles.Contains("down"))
                {
                    entity.Jump(this);
                }   
                if (!obstacles.Contains("down"))
                {
                    DoGravity(entity);
                }
                if (obstacles.Contains("down"))
                {
                    entity.ChangeAcceleration(new Vector(entity.Acceleration.X, 0));
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