using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /*что имеет любое более-менее живое существо? 
     Здоровье, а также местоположение. Что-то ещё? Добавьте, если будут идеи.*/
    public class Entity
    {
        public int HP  { get; set;}
        public Vector location { get; set; }
        public Rectangle Hitbox { get; set; }
    }
}
