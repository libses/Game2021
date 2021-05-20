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
        public Entity owner;
        private Pistol gun;
        public int damage = 8;
        readonly int kostil = 10;
        public bool isDead;
        public Vector location;
        public Vector velocity;
        public Vector kostilVelocity;
        public float angle; // ПОФИКСИТЬ УДАЛЕНИЕ ПУЛЬ!!!!!!!!
        public Bullet(Entity entity)
        {
            isDead = false;
            location = entity.Location;
            gun = entity.CurrentGun;
            var xVel = Math.Cos(gun.angle) * kostil * 6;
            var yVel = Math.Sin(gun.angle) * kostil * 6;
            velocity = new Vector((int)xVel, (int)yVel);
            owner = entity;
        }
        public void Fly()
        {
            if (owner.IsLeftOriented)
                kostilVelocity = new Vector((int)Math.Round((double)velocity.X / kostil), (int)Math.Round((double)velocity.Y / kostil));
            else
                kostilVelocity = -1 * new Vector((int)Math.Round((double)velocity.X / kostil), (int)Math.Round((double)velocity.Y / kostil));
            location += kostilVelocity;
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
