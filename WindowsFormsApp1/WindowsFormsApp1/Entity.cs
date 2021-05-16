using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Entity : IEntity
    {
        public bool IsLeft; // надо подумать, что делать с таким колличеством bool-ов
        public bool IsRight;
        public bool IsJump;
        public bool IsFight;
        public bool IsFiring;
        public bool IsDowningGun;
        public bool IsUppingGun;
        public int Width;
        public int Height;
        private double frame = 0;
        public int HP  { get; set;}
        public Vector Location { get; set; }
        public Rectangle Hitbox { get; set; }
        public Vector Velocity {get; set;}
        public Vector Acceleration { get; set; }
        public Bitmap originalSprite;
        public Bitmap currentSprite;
        private Dictionary<string, Bitmap[]> animations; // name and animation
        public Pistol CurrentGun;

        public void ChangeLocation(Vector newLocation)
        {
            Location = newLocation;
        }

        public void ChangeVelocity(Vector newVelocity)
        {
            Velocity = newVelocity;
        }

        public void ChangeAcceleration(Vector newAcceleration)
        {
            Acceleration = newAcceleration;
        }

        public Entity (int HP, Vector location, int width, int height, Bitmap sprite, Dictionary<string, Bitmap[]> animation) 
        {
            this.HP = HP;
            Location = location;
            Hitbox = new Rectangle(width, height, location);
            originalSprite = sprite;
            currentSprite = sprite;
            Width = width;
            Height = height;
            animations = animation;
        }

        public void Invalidate()
        {
            Hitbox = new Rectangle(Width, Height, Location);
        }

        public void Run(int direction, Physics physics)
        {
            if (frame >= animations["run"].Length)
                frame = 0;
            if (direction != 0)
            {
                currentSprite = animations["run"][(int)frame];
                frame += 0.3;
            }
            physics.DoRun(this, direction);
        }

        public void Jump(Physics physics)
        {
            var acc = new Vector(Acceleration.X, Acceleration.Y - 20);
            ChangeAcceleration(acc);
            physics.DoGravity(this);
            Invalidate();
        }

        private void ReceiveDamage(int damage)
        {
            HP -= damage;
        }

        public void Fight(Entity entity, int damage)
        {
            var distance = Location - entity.Location;
            if (distance.Length < 20)
            {
                if (IsFight && this is Player)
                {
                    var player = (Player)this;
                    entity.ReceiveDamage(damage);
                    player.IsFight = false;
                }
                else
                {
                    entity.ReceiveDamage(damage);
                    if (frame >= animations["fight"].Length)
                        frame = 0;
                    currentSprite = animations["fight"][(int)frame];
                    frame++;
                }
            }
        }
    }
}