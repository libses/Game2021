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
}
