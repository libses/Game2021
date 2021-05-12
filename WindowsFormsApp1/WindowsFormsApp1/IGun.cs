using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public interface IGun
    {
        void Fire();
        Bitmap sprite { get; set; }
        Entity owner { get; set; }
        double angle { get; set; }
        List<Bullet> bullets { get; set; }
    }
}
