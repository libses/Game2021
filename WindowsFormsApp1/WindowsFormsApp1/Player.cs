using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class Player : Entity
    {
        public Player(int HP, Vector location, int width, int height, 
            Bitmap sprite, Dictionary<string, Bitmap[]> animation, Dictionary<string, Bitmap[]> animationV, Bitmap spriteV) : base(HP, location, width, height, sprite, animation, animationV, spriteV)
        {

        }
    }
}