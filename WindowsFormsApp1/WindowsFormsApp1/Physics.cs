using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Physics
    {
        private Level level;
        private const int g = 1;
        private Block[,] map;
        
        public Physics(Level lvl) 
        {
            level = lvl;
            map = new IntedMap(lvl.Map, 20).intMap;
        }

        public void DoRun(Entity entity, int direction)
        {
            double runSpeed = 3;
            if (entity is Enemy)
            {
                runSpeed = 2;
            }
            if (true)
            { 
                var n = direction * runSpeed;
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
            if (map[LB.X - 1, LB.Y - 1] == block || map[LT.X - 1, LT.Y] == block)
                yield return "left";
            if (map[RB.X + 1, RB.Y - 1] == block || map[RT.X + 1, RT.Y] == block)
                yield return "right";
            if (map[LT.X + 1, LT.Y - 1] == block || map[RT.X - 1, RT.Y - 1] == block) 
                yield return "up";
        }

        public void Iterate() // need to refactoring
        {
            foreach (var entity in level.entities)
            {
                var obstacles = CollideObstacle(entity, Block.Ground)
                    .Concat(CollideObstacle(entity, Block.Bound))
                    .ToList();
                if (entity.IsFiring)
                {
                    entity.CurrentGun.Fire();
                }
                if (entity.CurrentGun != null)
                {
                    if (entity.IsDowningGun)
                    {
                        entity.CurrentGun.angle += 0.15;
                    }
                    if (entity.IsUppingGun)
                    {
                        entity.CurrentGun.angle -= 0.15;
                    }
                    foreach (var bullet in entity.CurrentGun.bullets)
                    {
                        if (!bullet.isDead)
                        {
                            bullet.Fly();
                        }
                    }
                }
                if (entity.IsRight && !obstacles.Contains("right"))
                {
                    entity.Run(1, this);
                }
                if (entity.IsLeft && !obstacles.Contains("left"))
                {
                    entity.Run(-1, this);
                }
                if (entity.IsJump && !obstacles.Contains("up") && obstacles.Contains("down"))
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
                    entity.currentSprite = entity.originalSprite;
                    entity.ChangeVelocity(new Vector(veloX, 0));
                }
                entity.Run(0, this);
                entity.Invalidate();
            }
        }
    }
}