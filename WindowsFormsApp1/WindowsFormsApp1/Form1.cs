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
        private Entity enemy;
        private Random random = new Random(new DateTime().Millisecond);

        public GameForm()
        {
            levels = LoadLevels().ToArray();
            physics = new Physics(levels[0]);
            player = levels[0].entities.Where(x => x is Player).FirstOrDefault();
            enemy = levels[0].entities.Where(x => x is Enemy).FirstOrDefault();
            painter = new Painter(levels);
            scaledViewPanel = new ViewPanel(painter) { Dock = DockStyle.Fill };
            Controls.Add(scaledViewPanel);
            KeyDown += FormKeyDown;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
            Text = "Game2021";
            var timer = new Timer();
            timer.Interval = 1;
            timer.Tick += (sender, args) =>
            {
                physics.Iterate();
                EnemyMoving();
                Refresh();
            };
            timer.Start();
        }

        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                player.Jump(physics);
            }
            if (e.KeyCode == Keys.D)
            {
                player.Move(0.1);
            }
            if (e.KeyCode == Keys.A)
            {
                player.Move(-0.1);
            }
        }

        private void EnemyMoving()
        {
            var action = random.Next(0, 10);
            if(enemy != null)
            {
                if (action == 0)
                    enemy.Move(0.1);
                if (action == 1)
                    enemy.Move(-0.1);
                if (action == 2)
                    enemy.Jump(physics);
            }
        }

        private static IEnumerable<Level> LoadLevels()
        {
            yield return Level.FromText(Properties.Resources.Map1, 1);
        }
    }
}
