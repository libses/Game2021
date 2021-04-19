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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
            Text = "Game2021";
            var timer = new Timer();
            timer.Interval = 17;
            timer.Tick += (sender, args) =>
            {
                physics.Iterate();
                Refresh();
            };
            timer.Start();
            
        }
        private void FormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                physics.Jump();
            }
            if (e.KeyCode == Keys.D)
            {
                for (int i = 0; i < 3; i++)
                {
                    foreach (var entity in levels[0].entities)
                    {
                        var x = entity.Location.X;
                        var y = entity.Location.Y;
                        entity.ChangeLocation(new Vector(x + 0.03, y));
                        entity.ChangeVelocity(new Vector(entity.Velocity.X + 0.01, entity.Velocity.Y));
                        //переписать на velocity. работает неадекватно + медленно.
                    }
                }
            }
            if (e.KeyCode == Keys.A)
            {
                for (int i = 0; i < 3; i++)
                {
                    foreach (var entity in levels[0].entities)
                    {
                        var x = entity.Location.X;
                        var y = entity.Location.Y;
                        entity.ChangeLocation(new Vector(x - 0.03, y));
                        entity.ChangeVelocity(new Vector(entity.Velocity.X - 0.01, entity.Velocity.Y));
                    }
                }
            }
        }
        public GameForm()
        {
            
            levels = LoadLevels().ToArray();
            physics = new Physics(levels[0]);
            painter = new Painter(levels);
            scaledViewPanel = new ViewPanel(painter) { Dock = DockStyle.Fill };
            Controls.Add(scaledViewPanel);
            KeyDown += FormKeyDown;
        }

        private static IEnumerable<Level> LoadLevels()
        {
            yield return Level.FromText(Properties.Resources.Map1, 1);
        }
    }
}
