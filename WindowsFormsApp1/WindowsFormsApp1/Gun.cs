using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Bullet
    {
        int kostil = 100;
        public bool isDead;
        public Vector location;
        public Vector velocity;
        public Vector kostilVelocity;
        public float angle;
        public Bullet(Entity entity)
        {
            isDead = false;
            location = entity.Location;
            var gun = entity.currentGun;
            var xVel = Math.Cos(gun.angle) * 6 * kostil;
            var yVel = Math.Sin(gun.angle) * 6 * kostil;
            velocity = new Vector((int)xVel, (int)yVel);
        }
        public void Fly()
        {
            kostilVelocity = new Vector((int)Math.Round((double)velocity.X / kostil), (int)Math.Round((double)velocity.Y / kostil));
            location = location + kostilVelocity;
        }
    }
    public class Pistol : IGun
    {
        public Entity owner { get; set; }
        public double angle { get; set; }
        public Bitmap sprite { get; set; }
        public List<Bullet> bullets { get; set; }
        public void Fire()
        {
            var bullet = new Bullet(owner);
            bullets.Add(bullet);
        }
        public Pistol(Bitmap sprite, Entity owner)
        {
            bullets = new List<Bullet>();
            angle = 0;
            this.owner = owner;
            this.sprite = sprite;
        }
    }
}
