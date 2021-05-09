using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class GameForm : Form
    {
        private readonly Painter painter;
        private readonly ViewPanel scaledViewPanel;
        private Physics physics;
        private Level[] levels;
        private Entity player;
        private Entity[] enemies;
        private Timer timer = new Timer();

        public GameForm()
        {
            levels = LoadLevels().ToArray();
            physics = new Physics(levels[0]);
            player = levels[0].entities.Where(x => x is Player).FirstOrDefault();
            enemies = levels[0].entities.Where(x => x is Enemy).ToArray();
            painter = new Painter(levels);
            scaledViewPanel = new ViewPanel(painter) { Dock = DockStyle.Fill };
            Controls.Add(scaledViewPanel);
            KeyUp += FormKeyUp;
            KeyDown += FormKeyPress;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
            Text = "Game2021";
            timer.Interval = 15;
            timer.Tick += (sender, args) =>
            {
                EnemyMoving();
                Fighting();
                Die();
                physics.Iterate();
                Refresh();
            };
            timer.Start();
        }

        bool press;
        private void FormKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space && !press)
            {
                press = true;
                player.isJump = true;
            }
            if (e.KeyData == Keys.D && !press)
            {
                press = true;
                player.isRight = true;
            }
            if (e.KeyData == Keys.A && !press)
            {
                press = true;
                player.isLeft = true;
            }
            if (e.KeyData == Keys.S && !press)
            {
                press = true;
                player.isFight = true;
            }
        }

        private void FormKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && press)
            {
                press = false;
                player.isJump = false;
            }
            if (e.KeyCode == Keys.D && press)
            {
                press = false;
                player.isRight = false;
            }
            if (e.KeyCode == Keys.A && press)
            {
                press = false;
                player.isLeft = false;
            }
            if (e.KeyData == Keys.S && press)
            {
                press = false;
                player.isFight = false;
            }
        }

        private void EnemyMoving()
        {
            foreach (var enemy in enemies)
            {
                enemy.isJump = false;
                enemy.isLeft = false;
                enemy.isRight = false;
                var path = player.Location - enemy.Location;
                if (path.Length >= 40)
                {
                    if (path.X > 0 && path.Y >= 0)
                        enemy.isRight = true;
                    if (path.X < 0 && path.Y < 0)
                    {
                        enemy.isJump = true;
                        enemy.isRight = true;
                    }
                    if (path.X > 0 && path.Y < 0)
                    {
                        enemy.isJump = true;
                        enemy.isRight = true;
                    }
                    if (path.X < 0 && path.Y >= 0)
                        enemy.isLeft = true;
                }
            }
        }

        public void Fighting()
        {
            if(player.isFight == true)
            {
                foreach(var enemy in enemies)
                {
                    var distance = enemy.Location - player.Location;
                    if(distance.Length < 10)
                    {
                        player.Fight(enemy);
                        Console.WriteLine(enemy.HP);
                    }
                }
            }
        }

        public void Die()
        {
            var level = levels[0];
            var died = new List<Entity>();
            foreach(var e in level.entities)
            {
                if (e.HP <= 0)
                    died.Add(e);
            }
            foreach(var d in died)
            {
                level.Remove(d);
            }
        }

        private static IEnumerable<Level> LoadLevels()
        {
            yield return Level.FromText(Properties.Resources.Map1, 1);
            yield return Level.FromText(Properties.Resources.Map2, 1);
        }
    }
}
