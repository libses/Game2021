using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{//переписать, чтобы каждый раз не рисовать левел, а только entities. Медленно работает!
	public class Painter
	{
		//public bool isDrawn = false;
		public Level currentLevel;
		public Bitmap mapImage;

		public SizeF Size => new SizeF(currentLevel.Map.GetLength(0), currentLevel.Map.GetLength(1));
		public Size LevelSize => new Size(currentLevel.Map.GetLength(0), currentLevel.Map.GetLength(1));

        public Painter(Level[] level)
        {
			currentLevel = level[0];
			CreateMap();
        }

        private void CreateMap()
        {
            var blockWidth = Properties.Resources.Ground.Width;
            var blockHeight = Properties.Resources.Ground.Height;
			//здеь
            mapImage = new Bitmap(LevelSize.Width * blockWidth, LevelSize.Height * blockHeight);
			using (var graphics = Graphics.FromImage(mapImage))
			{
				for (var x = 0; x < currentLevel.Map.GetLength(0); x++)
				{
					for (var y = 0; y < currentLevel.Map.GetLength(1); y++)
					{
						if (currentLevel.Map[x, y] == Block.Ground)
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
			//хардкодинг
			foreach (var ent in currentLevel.entities)
            {
				graphics.DrawImage(ent.Sprite, ((float)ent.Location.X-10)/20, ((float)ent.Location.Y-10)/20, 
					1, 1);
			}
		}

		public void Paint(Graphics g)
        {
			g.SmoothingMode = SmoothingMode.HighSpeed;
			g.DrawImage(mapImage, new RectangleF(0, 0, LevelSize.Width, LevelSize.Height));
			DrawLevel(g);
		}
    }
}