using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /*что имеет любое более-менее живое существо? 
     Здоровье, а также местоположение. Что-то ещё? Добавьте, если будут идеи.
    Хитбокс не должен быть отдельно изменяемым свойством. Должен быть отдельный конструктор для хитбокса, куда
    вводится только ширина и высота объекта, а хитбокс является свойством, дающим крайние грани от локации.*/
    public class Entity
    {
        public int HP  { get; set;}
        public Vector Location { get; set; }
        public Rectangle Hitbox { get; set; }
        public Vector Velocity {get; set;}
        public Entity (int HP, Vector location, double width, double height) {
            this.HP = HP;
            this.Location = location;
            Hitbox = new Rectangle(width, height, location);
        }
    }
}
