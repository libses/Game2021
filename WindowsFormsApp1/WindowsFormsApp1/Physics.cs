using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Physics
    {
        public double hard = 1;
        private readonly Level level;
        private const int g = 1;
        private readonly Block[,] map;
        readonly Random random = new Random();
        
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
                if (map[LB.X, LB.Y + entity.Acceleration.Y] == block || map[RB.X, RB.Y + entity.Acceleration.Y] == block)
                    yield return "down";
                if (map[LB.X - 1, LB.Y - 1] == block || map[LT.X - 1, LT.Y] == block)
                    yield return "left";
                if (map[RB.X + 1, RB.Y - 1] == block || map[RT.X + 1, RT.Y] == block)
                    yield return "right";
                if (map[LT.X + 1, LT.Y - 1] == block || map[RT.X - 1, RT.Y - 1] == block)
                    yield return "up";
            }
        }

        public void Iterate()
        {
            var player = level.Entities.Where(x => x is Player).FirstOrDefault();
            var e = level.Entities.Where(x => x is Enemy);
            foreach (var enemy in e)
            {
                var abc = (Enemy)enemy;
                if (random.NextDouble() >= 0.99) abc.Shoot((Player)player);
            }
            if (hard < 2.5)
            {
                hard *= 1.0001;
            }
            foreach (var spawner in level.Spawners)
            {
                bool ten = false;
                if (level.Entities.Count > 10*hard) ten = true;
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
                    level.Entities.Add(new Enemy(100, new Vector(spawner.location.X, spawner.location.Y),
                                        10, 10, Properties.Resources.EnemyStay,
                                        dictB, dictB, Properties.Resources.EnemyStay, map));
                }
            }
            
            var p = level.Entities;
            if (p.Count() > 0)
            {
                foreach (var guns in p)
                {
                    var gun = guns.CurrentGun;
                    foreach (var bullet in gun.bullets)
                    {
                        if (bullet.location.X > map.GetLength(0) * 20 || bullet.location.Y > map.GetLength(1) * 20 ||
                                    bullet.location.X < 0 || bullet.location.Y < 0) bullet.isDead = true;
                        foreach (var ent in level.Entities)
                        {
                            if (!(ent == bullet.owner))
                            {
                                if (bullet.location.X >= ent.Hitbox.LB.X && bullet.location.X <= ent.Hitbox.RB.X &&
                                    bullet.location.Y >= ent.Hitbox.LT.Y && bullet.location.Y <= ent.Hitbox.LB.Y)
                                {
                                    ent.HP -= bullet.damage;
                                    bullet.damage /= 2;
                                }
                            }
                            
                        } 
                    }
                    gun.bullets = gun.bullets.Where(x => x.isDead == false).ToList();
                }
            }
            foreach (var entity in level.Entities)
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
            var res = Screen.PrimaryScreen.Bounds;
            var xx = (level.mousePosition.X / (double)(res.Width)) * map.GetLength(0);
            var yy = ((level.mousePosition.Y - SystemInformation.BorderSize.Height * 2) / (double)(res.Height - SystemInformation.BorderSize.Height * 2)) * map.GetLength(1);
            var mouse = new Vector((int)(xx), (int)(yy));
            if (player != null)
            {
                var a = (player.Location.X - mouse.X) / (double)((player.Location - mouse).Length);
                var b = (player.Location.Y - mouse.Y);
                var angle = 0d;
                if (b >= 0)
                {
                    angle = Math.Acos(a);
                }
                else
                {
                    angle = -Math.Acos(a);
                }
                player.CurrentGun.angle = angle + Math.PI;
            }
        }
    }
}