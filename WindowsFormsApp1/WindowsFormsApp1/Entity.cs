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
        Vector Location { get; set; }
        Rectangle Hitbox { get; set; }
        Vector Velocity { get; set; }
        void ChangeLocation(Vector newLocaton);
        void ChangeVelocity(Vector newVelocity);
    }
    public class Entity : IEntity
    {
        public double Width;
        public double Height;
        public int HP  { get; set;}
        public Vector Location { get; set; }
        public void ChangeLocation(Vector newLocation)
        {
            Location = newLocation;
        }
        public void ChangeVelocity(Vector newVelocity)
        {
            Velocity = newVelocity;
        }
        public Rectangle Hitbox { get; set; }
        public Vector Velocity {get; set;}
        public Bitmap Sprite;
        public Entity (int HP, Vector location, double width, double height, Bitmap sprite) 
        {
            this.HP = HP;
            this.Location = location;
            Hitbox = new Rectangle(width, height, location);
            Sprite = sprite;
            Width = width;
            Height = height;
        }
        public void Invalidate()
        {
            Hitbox = new Rectangle(Width, Height, Location);
        }
        public void Jump()
        {

        }
    }
}
