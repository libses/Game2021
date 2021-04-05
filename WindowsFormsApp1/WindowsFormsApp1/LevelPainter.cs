using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
	public class Painter
	{
		private Level currentLevel;
		private Bitmap mapImage;

		public SizeF Size => new SizeF(currentLevel.CurrentLevel.GetLength(0), currentLevel.CurrentLevel.GetLength(1));
		public Size LevelSize => new Size(currentLevel.CurrentLevel.GetLength(0), currentLevel.CurrentLevel.GetLength(1));

        public Painter(Level[] level)
        {
			currentLevel = level[0];
			CreateMap();
        }

        private void CreateMap()
        {
            var blockWidth = Properties.Resources.Ground.Width;
            var blockHeight = Properties.Resources.Ground.Height;
            mapImage = new Bitmap(LevelSize.Width * blockWidth, LevelSize.Height * blockHeight);
			using (var graphics = Graphics.FromImage(mapImage))
			{
				for (var x = 0; x < currentLevel.CurrentLevel.GetLength(0); x++)
				{
					for (var y = 0; y < currentLevel.CurrentLevel.GetLength(1); y++)
					{
						if (currentLevel.CurrentLevel[x, y] == Block.Ground)
						{
							var image = Properties.Resources.Ground;
							graphics.DrawImage(image,
								new RectangleF(x * blockWidth, y * blockHeight, blockWidth, blockHeight));
						}
					}
				}
			}
		}

		private void DrawLevel(Graphics graphics)
        {
			graphics.DrawImage(mapImage, new RectangleF(0, 0, LevelSize.Width, LevelSize.Height));
		}

		public void Paint(Graphics g)
        {
			g.SmoothingMode = SmoothingMode.AntiAlias;
			DrawLevel(g);
		}
    }
}