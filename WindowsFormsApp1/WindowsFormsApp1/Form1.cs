﻿using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
        private Label label;

        public GameForm()
        {
            levels = LoadLevels().ToArray();
            physics = new Physics(levels[0]);
            painter = new Painter(levels);
            label = new Label() { Dock = DockStyle.Top, Font = new Font("Arial", 12) };
            scaledViewPanel = new ViewPanel(painter) { Dock = DockStyle.Fill };
            Controls.Add(scaledViewPanel);
            Controls.Add(label);
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
                player = levels[0].entities.Where(x => x is Player).FirstOrDefault();
                enemies = levels[0].entities.Where(x => x is Enemy).ToArray();
                if (player != null) // null - Game Over
                {
                    EnemyMoving();
                    Fighting();
                    label.Text = "HP: " + player.HP;
                    Die();
                    physics.Iterate();
                    Refresh();
                }
            };
            timer.Start();
            //here the music goes
            #region
            var debug1 = Environment.CurrentDirectory.Split('\\').ToList();
            debug1.RemoveAt(debug1.Count - 1);
            debug1.RemoveAt(debug1.Count - 1);
            string path = "";
            foreach (var character in debug1) { path = path + character + '\\'; }
            var resPath = Path.Combine(path, @"Images\sounds\Monkeys-Spinning-Monkeys.wav");
            System.Media.SoundPlayer playerM = new System.Media.SoundPlayer(resPath);
            playerM.Play();
            #endregion
        }

        bool press;
        private void FormKeyPress(object sender, KeyEventArgs e)
        {
            if (player != null)
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
        }

        private void FormKeyUp(object sender, KeyEventArgs e)
        {
            if (player != null)
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
        }

        private void EnemyMoving()
        {
            foreach (var enemy in enemies)
            {
                enemy.isJump = false;
                enemy.isLeft = false;
                enemy.isRight = false;
                var path = player.Location - enemy.Location;
                if (path.Length >= 20)
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
            foreach(var enemy in enemies)
            {
                var distance = player.Location - enemy.Location;
                if (distance.Length < 20)
                {
                    if(player.isFight)
                    {
                        player.Fight(enemy);
                        Console.WriteLine(enemy.HP);
                        player.isFight = false;
                    }
                    enemy.Fight(player);
                }
            }
        }

        public void Die()
        {
            var level = levels[0];
            var died = new List<Entity>();
            foreach(var entity in level.entities)
            {
                if (entity.HP <= 0)
                    died.Add(entity);
            }

            foreach(var entity in died)
            {
                level.Remove(entity);
            }
        }

        private static IEnumerable<Level> LoadLevels()
        {
            yield return Level.FromText(Properties.Resources.Map1, 1);
            yield return Level.FromText(Properties.Resources.Map2, 1);
        }
    }
}
