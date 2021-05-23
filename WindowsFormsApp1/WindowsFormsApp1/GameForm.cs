using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using WMPLib;

namespace WindowsFormsApp1
{
    public class GameForm : Form
    {
        private readonly Painter painter;
        private readonly ViewPanel scaledViewPanel;
        public Physics physics;
        private readonly Level currentLevel;
        public Entity player;
        private Entity[] enemies;
        private readonly System.Windows.Forms.Timer timer;
        private readonly Label TextLabel;
        private int initialCoins;
        private DispatcherTimer timeToClose = new DispatcherTimer();
        private SoundPlayer music = new SoundPlayer(Properties.Resources.music);

        public GameForm(Level newLevel)
        {
            currentLevel = newLevel;
            physics = new Physics(currentLevel);
            painter = new Painter(currentLevel);
            TextLabel = new Label() { Dock = DockStyle.Top, Font = new Font("Arial", 15), Size = new Size(0,30) };
            scaledViewPanel = new ViewPanel(painter) { Dock = DockStyle.Fill };
            timer = new System.Windows.Forms.Timer();
            timeToClose.Interval = new TimeSpan(0, 0, 10);
            Controls.Add(scaledViewPanel);
            Controls.Add(TextLabel);
            KeyUp += FormKeyUp;
            KeyDown += FormKeyPress;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            music.Stop();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Text = "Game2021";
            Console.WriteLine(Properties.Resources.music.ToString());
            timer.Interval = 15;
            initialCoins = currentLevel.Coins.Count;
            timer.Tick += (sender, args) =>
            {
                MMouseMove();
                player = currentLevel.Entities.Where(x => x is Player).FirstOrDefault();
                enemies = currentLevel.Entities.Where(x => x is Enemy).ToArray();
                if(player == null)
                {
                    timeToClose.Start();
                    TextLabel.Text = "Game over:(";
                    music.Stop();
                    timeToClose.Tick += (s, a) => Close();
                }
                else if(player.Score == initialCoins && initialCoins != 0)
                {
                    timeToClose.Start();
                    TextLabel.Text = "You win!";
                    music.Stop();
                    timeToClose.Tick += (s, a) => Close();
                }
                else
                {
                    EnemyMoving();
                    Fighting();
                    TextLabel.Text = "HP: " + player.HP + "  Score: " + player.Score;
                    player.RemoveEntities(currentLevel);
                    physics.Iterate();
                    Refresh();
                }
            };
            timer.Start();
            music.PlayLooping();
        }

        bool pressSpace;
        public bool pressD;
        bool pressA;
        bool pressS;
        public bool pressE;
        bool pressR;
        bool pressF;

        private void MMouseMove()
        {
            var pos = Cursor.Position;
            var vector = new Vector(pos.X, pos.Y);
            currentLevel.mousePosition = vector;
        }

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
                    player.IsLeftOriented = true;
                }
                if (e.KeyData == Keys.A && !pressA)
                {
                    pressA = true;
                    player.IsLeft = true;
                    player.IsLeftOriented = false;
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
                if (e.KeyData == Keys.F && !pressR)
                {
                    player.CurrentGun.angle += 0.78539816339744830961566084581988;
                }
                if (e.KeyData == Keys.R && !pressF)
                {
                    player.CurrentGun.angle -= 0.78539816339744830961566084581988;
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
    }
}
