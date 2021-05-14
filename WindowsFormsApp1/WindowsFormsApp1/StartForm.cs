using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class StartForm : Form
    {
        private Level[] levels;
        private FlowLayoutPanel menuPanel;
        private PictureBox menuPicture;

        public StartForm()
        {
            menuPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.YellowGreen,
                Padding = new Padding(8),
                Font = new Font(SystemFonts.DefaultFont.FontFamily, 16, FontStyle.Bold)
            };
            menuPanel.Controls.Add(new Label
            {
                Text = "Choose level:",
                ForeColor = Color.FloralWhite,
                AutoSize = true,
                Margin = new Padding(8)
            });
            menuPicture = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = Properties.Resources.logo
            };
            Controls.Add(menuPanel);
            Controls.Add(menuPicture);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
            Text = "Game2021";
            DrawLevelSwitch(levels, menuPanel);
        }

        private static IEnumerable<Level> LoadLevels()
        {
            yield return Level.FromText(Properties.Resources.Map1, 1);
            yield return Level.FromText(Properties.Resources.Map2, 1);
        }

        private void DrawLevelSwitch(Level[] levels, FlowLayoutPanel menuPanel)
        {
            var linkLabels = new List<LinkLabel>();
            levels = LoadLevels().ToArray();
            for (var i = 0; i < levels.Length; i++)
            {
                var level = levels[i];
                var link = new LinkLabel
                {
                    Text = $"Level {i + 1}",
                    ActiveLinkColor = Color.Beige,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = true,
                    Margin = new Padding(32, 16, 8, 16),
                };
                link.LinkClicked += (sender, args) =>
                {
                    UpdateLinks(level, linkLabels);
                    ChangeLevel((Level)link.Tag);
                    Show();
                };
                menuPanel.Controls.Add(link);
                linkLabels.Add(link);
            }
            UpdateLinks(levels[0], linkLabels);
        }

        private void ChangeLevel(Level newLevel)
        {
            Hide();
            var game = new GameForm(newLevel);
            game.ShowDialog();
        }

        private void UpdateLinks(Level level, List<LinkLabel> linkLabels)
        {
            levels = LoadLevels().ToArray();
            int i = 0;
            foreach (var linkLabel in linkLabels)
            {
                linkLabel.LinkColor = linkLabel.Tag == level ? Color.LimeGreen : Color.Brown;
                linkLabel.Tag = levels[i];
                i++;
            }
        }
    }
}
