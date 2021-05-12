using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /*что имеет любое более-менее живое существо? 
     Здоровье, а также местоположение. Что-то ещё? Добавьте, если будут идеи.
    Хитбокс не должен быть отдельно изменяемым свойством. Должен быть отдельный конструктор для хитбокса, куда
    вводится только ширина и высота объекта, а хитбокс является свойством, дающим крайние грани от локации.*/
    public interface IEntity
    {
        Vector Acceleration { get; set; }
        Vector Location { get; set; }
        Rectangle Hitbox { get; set; }
        Vector Velocity { get; set; }
        void ChangeLocation(Vector newLocaton);
        void ChangeVelocity(Vector newVelocity);
        void ChangeAcceleration(Vector newAcceleration);
    }

    public abstract class Entity : IEntity
    {
        public bool isLeft;
        public bool isRight;
        public bool isJump;
        public bool isFight;
        public bool isFiring;
        public bool isDowningGun;
        public bool isUppingGun;
        public int Width;
        public int Height;
        public int HP  { get; set;}
        public Vector Location { get; set; }
        public Rectangle Hitbox { get; set; }
        public Vector Velocity {get; set;}
        public Vector Acceleration { get; set; }
        public Bitmap Sprite;
        public Pistol currentGun;

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

        public Entity (int HP, Vector location, int width, int height, Bitmap sprite) 
        {
            this.HP = HP;
            Location = location;
            Hitbox = new Rectangle(width, height, location);
            Sprite = sprite;
            Width = width;
            Height = height;
        }

        public void Invalidate()
        {
            Hitbox = new Rectangle(Width, Height, Location);
        }
        public void Run(int direction, Physics physics)
        {
            physics.DoRun(this, direction);
        }
        public void Move(int dx)
        {
            for (int i = 0; i < 5; i++)
            {
                var x = Location.X;
                var y = Location.Y;
                ChangeLocation(new Vector(x + dx, y));
                ChangeVelocity(new Vector(Velocity.X + dx, Velocity.Y));
            }
        }

        public void Jump(Physics physics)
        {
            var acc = new Vector(Acceleration.X, Acceleration.Y - 10);
            var p = new Vector(Velocity.X, Velocity.Y - 30);
            ChangeAcceleration(acc);
            physics.DoGravity(this);
            Invalidate();
        }

        public void ReceiveDamage(int damage)
        {
            HP = 100;
            //HP -= damage;
        }

        public abstract void Fight(Entity entity);
    }
}