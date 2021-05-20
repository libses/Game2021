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
		public Level currentLevel;
		public Bitmap mapImage;
		private Bitmap rotatedGun = Properties.Resources.gun;

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
			rotationAngle = rotationAngle * 57.3f;
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
			foreach (var coin in currentLevel.Coins)
			{
				graphics.DrawImage(coin.Sprite, ((float)coin.Location.X - 10) / 20, ((float)coin.Location.Y - 10) / 20,
					1, 1);
			}

			foreach (var ent in currentLevel.Entities)
            {
				graphics.DrawImage(ent.currentSprite, ((float)ent.Location.X-10)/20, ((float)ent.Location.Y-10)/20, 
					1, 1);
				if (!ent.IsLeftOriented)
				{
					ent.CurrentGun.angle -= Math.PI;
					ent.CurrentGun.sprite = Properties.Resources.gunv;
					if (ent.CurrentGun != null && !double.IsNaN(ent.CurrentGun.angle))
					{
						if (ent.CurrentGun.angle >= - Math.PI / 2 && ent.CurrentGun.angle <= Math.PI / 2)
						{
							
							if (ent.CurrentGun.angle == 0)
								rotatedGun = Properties.Resources.gunv;
							else
								rotatedGun = RotateImage(Properties.Resources.gunv, (float)ent.CurrentGun.angle);

							graphics.DrawImage(rotatedGun, ((float)ent.Location.X - 15) / 20, ((float)ent.Location.Y - 12) / 20,
								1, 1);
						}
						else
						{
							graphics.DrawImage(rotatedGun, ((float)ent.Location.X - 15) / 20, ((float)ent.Location.Y - 12) / 20,
								1, 1);
                            if (ent.CurrentGun.angle <= -Math.PI / 2)
                                ent.CurrentGun.angle = -Math.PI / 2;
                            else if (ent.CurrentGun.angle >= Math.PI / 2)
                                ent.CurrentGun.angle = Math.PI / 2;
                        }
						foreach (var bullet in ent.CurrentGun.bullets)
						{
							graphics.DrawImage(Properties.Resources.bullet2, ((float)bullet.location.X - 15) / 20, ((float)bullet.location.Y - 12) / 20,
							1, 1);
						}
					}
				}

				else
				{
					if (ent.CurrentGun != null && !double.IsNaN(ent.CurrentGun.angle))
					{
						ent.CurrentGun.sprite = Properties.Resources.gun;
						if ((ent.CurrentGun.angle <= Math.PI / 2 && ent.CurrentGun.angle >= 0) 
							|| (ent.CurrentGun.angle <= 2 * Math.PI && ent.CurrentGun.angle >= 1.5 * Math.PI))
						{
							if (ent.CurrentGun.angle == 0)
								rotatedGun = Properties.Resources.gun;
							else
								rotatedGun = RotateImage(Properties.Resources.gun, (float)ent.CurrentGun.angle);
							
							graphics.DrawImage(rotatedGun, ((float)ent.Location.X - 5) / 20, ((float)ent.Location.Y - 15) / 20,
							1, 1);
						}
						else
						{
							graphics.DrawImage(rotatedGun, ((float)ent.Location.X - 5) / 20, ((float)ent.Location.Y - 15) / 20,
								1, 1);
							if (ent.CurrentGun.angle > Math.PI / 2)
								ent.CurrentGun.angle = Math.PI / 2;
							if (ent.CurrentGun.angle >= 1.5 * Math.PI)
								ent.CurrentGun.angle = 1.5 * Math.PI;
						}

						foreach (var bullet in ent.CurrentGun.bullets)
						{
							graphics.DrawImage(Properties.Resources.bullet2, ((float)bullet.location.X - 5) / 20, ((float)bullet.location.Y - 15) / 20,
							1, 1);
						}
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