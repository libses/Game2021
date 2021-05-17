﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Physics
    {
        public double hard = 1;
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
                runSpeed = 1*hard;
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
            if (LB.X > 6 && LB.Y > 6 && RB.X > 6 && RB.Y > 6 && LT.X > 6 && LT.Y > 6 && RT.X > 6 && RT.Y > 6)
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
            var player = level.entities.Where(x => x is Player).FirstOrDefault();
            var e = level.entities.Where(x => x is Enemy);
            foreach (var enemy in e)
            {
                var abc = (Enemy)enemy;
                if (random.NextDouble() > 0.98) abc.Shoot((Player)player);
            }
            if (hard < 2.5)
            {
                hard = hard * 1.0001;
            }
            foreach (var spawner in level.spawners)
            {
                bool ten = false;
                if (level.entities.Count > 10*hard) ten = true;
                if (random.NextDouble() > 0.99 && !ten)
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
                    level.entities.Add(new Enemy(100, new Vector(spawner.location.X, spawner.location.Y),
                                        10, 10, Properties.Resources.EnemyStay,
                                        dictB, dictB, Properties.Resources.EnemyStay));
                }
            }
            
            var p = level.entities;
            if (p.Count() > 0)
            {
                foreach (var guns in p)
                {
                    var gun = guns.CurrentGun;
                    foreach (var bullet in gun.bullets)
                    {
                        foreach (var ent in level.entities)
                        {
                            if (!(ent == bullet.owner))
                            {
                                if (bullet.location.X >= ent.Hitbox.LB.X && bullet.location.X <= ent.Hitbox.RB.X &&
                                    bullet.location.Y >= ent.Hitbox.LT.Y && bullet.location.Y <= ent.Hitbox.LB.Y)
                                {
                                    ent.HP = ent.HP - bullet.damage;
                                    bullet.damage = bullet.damage / 2;
                                }
                                if (bullet.location.X > map.GetLength(0) * 20 || bullet.location.Y > map.GetLength(1) * 20 ||
                                    bullet.location.X < 0 || bullet.location.Y < 0) bullet.isDead = true;
                            }
                        }
                        gun.bullets = gun.bullets.Where(x => x.isDead == false).ToList();
                    }
                }
            }
            foreach (var entity in level.entities)
            {
                if (entity.tiredness > 0) entity.tiredness--;
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
                if (entity.IsJump && !obstacles.Contains("up") && obstacles.Contains("down") && entity.tiredness == 0)
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