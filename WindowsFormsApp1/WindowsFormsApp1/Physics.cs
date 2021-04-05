using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Physics
    {
        public Level level;
        public Physics(Level lvl) {
            level = lvl;
        }
        public const double g = 9.8;
        public void Iterate() {
            
        }
    }
}
