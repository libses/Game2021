using System.Drawing;

namespace WindowsFormsApp1
{
    public class Player : Entity
    {
        public Player(int HP, Vector location, int width, int height, Bitmap sprite) : base(HP, location, width, height, sprite)
        {

        }

        public override void Fight(Entity entity)
        {
            entity.ReceiveDamage(20);
        }
    }
}
