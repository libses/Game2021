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

        public Painter(Level level)
        {
			currentLevel = level;
			CreateMap();
        }

        private void CreateMap()
        {
            var blockWidth = Properties.Resources.Ground.Width;
            var blockHeight = Properties.Resources.Ground.Height;
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
		public static Bitmap RotateImage(Bitmap img, float rotationAngle)
		{
			rotationAngle = rotationAngle * 56;
			//create an empty Bitmap image
			Bitmap bmp = new Bitmap(img.Width, img.Height);

			//turn the Bitmap into a Graphics object
			Graphics gfx = Graphics.FromImage(bmp);

			//now we set the rotation point to the center of our image
			gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

			//now rotate the image
			gfx.RotateTransform(rotationAngle);

			gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

			//set the InterpolationMode to HighQualityBicubic so to ensure a high
			//quality image once it is transformed to the specified size
			gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

			//now draw our new image onto the graphics object
			gfx.DrawImage(img, new Point(0, 0));

			//dispose of our Graphics object
			gfx.Dispose();

			//return the image
			return bmp;
		}
		private void DrawLevel(Graphics graphics)
        {
			//хардкодинг
			foreach (var ent in currentLevel.entities)
            {
				graphics.DrawImage(ent.Sprite, ((float)ent.Location.X-10)/20, ((float)ent.Location.Y-10)/20, 
					1, 1);
				if (ent.currentGun != null)
                {
					Bitmap rotatedGun;
					if (ent.currentGun.angle == 0)
                    {
						rotatedGun = Properties.Resources.gun;
                    } else
                    {
						rotatedGun = RotateImage(Properties.Resources.gun, (float)ent.currentGun.angle);
					}
					graphics.DrawImage(rotatedGun, ((float)ent.Location.X - 10) / 20, ((float)ent.Location.Y - 10) / 20,
						1, 1);
					foreach (var bullet in ent.currentGun.bullets)
					{
						graphics.DrawImage(Properties.Resources.bullet2, ((float)bullet.location.X - 10) / 20, ((float)bullet.location.Y - 10) / 20,
						1, 1);
					}
				}
			}
		}

		public void Paint(Graphics g)
        {
			g.SmoothingMode = SmoothingMode.HighSpeed;
			g.DrawImage(mapImage, new RectangleF(0, 0, LevelSize.Width, LevelSize.Height));
			DrawLevel(g);
		}

		public void ChangeLevel(Level newMap)
		{
			currentLevel = newMap;
			CreateMap();
		}
	}
}