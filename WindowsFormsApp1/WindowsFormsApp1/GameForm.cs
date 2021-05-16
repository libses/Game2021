﻿using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class GameForm : Form
    {
        private readonly Painter painter;
        private readonly ViewPanel scaledViewPanel;
        private readonly Physics physics;
        private readonly Level currentLevel;
        private Entity player;
        private Entity[] enemies;
        private readonly Timer timer;
        private readonly Label HPlabel;
        private readonly SoundPlayer music;

        public GameForm(Level newLevel)
        {
            currentLevel = newLevel;
            music = new SoundPlayer(Properties.Resources.Monkeys_Spinning_Monkeys1); // рандомная музыка на уровень?
            physics = new Physics(currentLevel);
            painter = new Painter(currentLevel);
            HPlabel = new Label() { Dock = DockStyle.Top, Font = new Font("Arial", 12) };
            scaledViewPanel = new ViewPanel(painter) { Dock = DockStyle.Fill };
            timer = new Timer();
            Controls.Add(scaledViewPanel);
            Controls.Add(HPlabel);
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
                player = currentLevel.entities.Where(x => x is Player).FirstOrDefault();
                enemies = currentLevel.entities.Where(x => x is Enemy).ToArray();
                if (player != null) // null - Game Over
                {
                    EnemyMoving();
                    Fighting();
                    HPlabel.Text = "HP: " + player.HP;
                    RemoveDied();
                    physics.Iterate();
                    Refresh();
                }
                else
                    Close();
            };
            timer.Start();
            music.Play();
        }

        bool pressSpace;
        bool pressD;
        bool pressA;
        bool pressS;
        bool pressE;
        bool pressR;
        bool pressF;
        private void FormKeyPress(object sender, KeyEventArgs e)
        {
            if (player != null)
            {
                if (e.KeyData == Keys.Space && !pressSpace)
                {
                    pressSpace = true;
                    player.IsJump = true;
                }
                if (e.KeyData == Keys.D && !pressD)
                {
                    pressD = true;
                    player.IsRight = true;
                }
                if (e.KeyData == Keys.A && !pressA)
                {
                    pressA = true;
                    player.IsLeft = true;
                }
                if (e.KeyData == Keys.S && !pressS)
                {
                    pressS = true;
                    player.IsFight = true;
                }
                if (e.KeyData == Keys.E && !pressE)
                {
                    pressE = true;
                    player.IsFiring = true;
                }
                if (e.KeyData == Keys.R && !pressR)
                {
                    pressR = true;
                    player.IsUppingGun = true;
                }
                if (e.KeyData == Keys.F && !pressF)
                {
                    pressF = true;
                    player.IsDowningGun = true;
                }
            }
        }

        private void FormKeyUp(object sender, KeyEventArgs e)
        {
            if (player != null)
            {
                if (e.KeyCode == Keys.Space && pressSpace)
                {
                    pressSpace = false;
                    player.IsJump = false;
                }
                if (e.KeyCode == Keys.D && pressD)
                {
                    pressD = false;
                    player.IsRight = false;
                }
                if (e.KeyCode == Keys.A && pressA)
                {
                    pressA = false;
                    player.IsLeft = false;
                }
                if (e.KeyData == Keys.S && pressS)
                {
                    pressS = false;
                    player.IsFight = false;
                }
                if (e.KeyData == Keys.E && pressE)
                {
                    pressE = false;
                    player.IsFiring = false;
                }
                if (e.KeyData == Keys.R && pressR)
                {
                    pressR = false;
                    player.IsUppingGun = false;
                }
                if (e.KeyData == Keys.F && pressF)
                {
                    pressF = false;
                    player.IsDowningGun = false;
                }
            }
        }

        private void EnemyMoving()
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Moving((Player)player);
            }
        }

        private void Fighting()
        {
            foreach(var enemy in enemies)
            {
                enemy.Fight(player, 1);
                if (player.IsFight)
                    player.Fight(enemy, 20);
            }
        }

        private void RemoveDied()
        {
            var died = new List<Entity>();
            foreach(var entity in currentLevel.entities)
            {
                if (entity.HP <= 0)
                    died.Add(entity);
            }

            foreach(var entity in died)
            {
                currentLevel.Remove(entity);
            }
        }
    }
}
