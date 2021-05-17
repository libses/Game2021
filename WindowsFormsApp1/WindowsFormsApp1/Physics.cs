using System;
using System.Collections.Generic;
using System.Drawing;
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
        Random random = new Random();
        
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
            if (LB.X > 4 && LB.Y > 4 && RB.X > 4 && RB.Y > 4 && LT.X > 4 && LT.Y > 4 && RT.X > 4 && RT.Y > 4)
            {
                if (map[LB.X + 4, LB.Y + entity.Acceleration.Y] == block || map[RB.X - 4, RB.Y + entity.Acceleration.Y] == block)
                    yield return "down";
                if (map[LB.X - 1, LB.Y - 1] == block || map[LT.X - 1, LT.Y] == block)
                    yield return "left";
                if (map[RB.X + 1, RB.Y - 1] == block || map[RT.X + 1, RT.Y] == block)
                    yield return "right";
                if (map[LT.X + 1, LT.Y - 1] == block || map[RT.X - 1, RT.Y - 1] == block)
                    yield return "up";
            }
        }

        public void Iterate() // need to refactoring
        {
            if (random.NextDouble() > 0.95)
            {
                var dictB = new Dictionary<string, Bitmap[]>()
									{
										{ "run", new Bitmap[]
											{
												Properties.Resources.EnemyRun1,
												Properties.Resources.EnemyRun2,
												Properties.Resources.EnemyRun3,
												Properties.Resources.EnemyRun4,
												Properties.Resources.EnemyRun5,
												Properties.Resources.EnemyRun6,
												Properties.Resources.EnemyStay
											}
										},
										{ "fight", new Bitmap[]
											{
												Properties.Resources.EnemyFight1,
												Properties.Resources.EnemyFight2,
												Properties.Resources.EnemyFight3,
												Properties.Resources.EnemyFight4,
												Properties.Resources.EnemyFight5,
												Properties.Resources.EnemyStay
											}
										}
									};
                level.entities.Add(new Enemy(100, new Vector(random.Next(0, level.Map.GetLength(0) *19), random.Next(0, level.Map.GetLength(1) * 19)),
                                    10, 10, Properties.Resources.EnemyStay,
                                    dictB, dictB, Properties.Resources.EnemyStay));
            }
            var p = level.entities.Where(x => x is Player);
            if (p.Count() > 0)
            {
                var gun = p.ToList()[0].CurrentGun;
                foreach (var bullet in gun.bullets)
                {
                    foreach (var ent in level.entities)
                    {
                        if (!(ent is Player))
                        {
                            if (bullet.location.X >= ent.Hitbox.LB.X && bullet.location.X <= ent.Hitbox.RB.X &&
                                bullet.location.Y >= ent.Hitbox.LT.Y && bullet.location.Y <= ent.Hitbox.LB.Y)
                            {
                                ent.HP = ent.HP - 4;
                            }
                        }
                    }

                }
            }
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
                        entity.CurrentGun.angle += 1;
                    }
                    if (entity.IsUppingGun)
                    {
                        entity.CurrentGun.angle -= 1;
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