﻿using NUnit.Framework.Constraints;
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
        private Random random = new Random(new DateTime().Millisecond);
        private PathFinder PathFinder;

        public GameForm()
        {
            levels = LoadLevels().ToArray();
            physics = new Physics(levels[0]);
            player = levels[0].entities.Where(x => x is Player).FirstOrDefault();
            enemies = levels[0].entities.Where(x => x is Enemy).ToArray();
            painter = new Painter(levels);
            PathFinder = new PathFinder();
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
            var timer = new Timer();
            timer.Interval = 15;
            timer.Tick += (sender, args) =>
            {
                EnemyMoving();
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
        }

        private void EnemyMoving()
        {
            foreach (var enemy in enemies)
            {
                if(Math.Abs(enemy.Location.X - player.Location.X) > 20 || Math.Abs(enemy.Location.Y - player.Location.Y) > 20)
                {
                    var path = player.Location - enemy.Location;
                    if (path.X > 0 && path.Y > 0)
                        enemy.Run(1, physics);
                    if (path.X < 0 && path.Y < 0)
                    {
                        enemy.Jump(physics);
                        enemy.Run(-1, physics);
                    }
                    if (path.X > 0 && path.Y < 0)
                    {
                        enemy.Jump(physics);
                        enemy.Run(1, physics);
                    }
                    if (path.X < 0 && path.Y > 0)
                        enemy.Run(-1, physics);
                    if (path.X == 0 && path.Y > 0)
                        enemy.Run(-1, physics);
                    if (path.X == 0 && path.Y < 0)
                        enemy.Jump(physics);
                    if (path.X < 0 && path.Y == 0)
                        enemy.Run(-1, physics);
                    if (path.X > 0 && path.Y == 0)
                        enemy.Run(1, physics);
                }
            }
        }

        private static IEnumerable<Level> LoadLevels()
        {
            yield return Level.FromText(Properties.Resources.Map1, 1);
        }
    }
}
