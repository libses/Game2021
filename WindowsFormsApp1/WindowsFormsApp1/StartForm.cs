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
                BackColor = Color.FromArgb(244, 196, 148),
                Padding = new Padding(8),
                Font = new Font(SystemFonts.DefaultFont.FontFamily, 16, FontStyle.Bold)
            };
            menuPanel.Controls.Add(new Label
            {
                Text = "Choose level:",
                ForeColor = Color.FromArgb(151, 57, 75),
                AutoSize = true,
                Margin = new Padding(8)
            });
            menuPicture = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = Properties.Resources._1doom,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            Controls.Add(menuPanel);
            Controls.Add(menuPicture);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
            Text = "Ghost.Killer";
            DrawLevelSwitch(levels, menuPanel);
        }

        private static IEnumerable<Level> LoadLevels()
        {
            yield return Level.FromText(Properties.Resources.Map1, 1);
            yield return Level.FromText(Properties.Resources.Map2, 1);
            yield return Level.FromText(Properties.Resources.Map3, 1);
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
                    ActiveLinkColor = Color.FromArgb(105, 94, 101),
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
                linkLabel.LinkColor = linkLabel.Tag == level ? Color.Black : Color.Brown;
                linkLabel.Tag = levels[i];
                i++;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.SuspendLayout();
            // 
            // StartForm
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartForm";
            this.ResumeLayout(false);

        }
    }
}
