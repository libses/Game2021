using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class Player : Entity
    {
        public Player(int HP, Vector location, int width, int height, Bitmap sprite, Dictionary<string, Bitmap[]> animations) : base(HP, location, width, height, sprite, animations)
        {
        }
    }
}