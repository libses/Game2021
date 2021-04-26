using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WindowsFormsApp1
{
	public enum Block
	{
		Empty,
		Ground
	}

	public class Level
	{
		public readonly Block[,] Map;
		public readonly Point Start;
		public Entity[] entities;

		private Level(Block[,] level, Entity[] ents)
		{
			Map = level;
			entities = ents;
		}

		public static Level FromText(string text, int splitting)
		{
			var lines = text.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			return FromLines(lines, splitting);
		}

		public static Level FromLines(string[] lines, int splitting)
		{
			var ents = new List<Entity>();
			var map = new Block[lines[0].Length * splitting, lines.Length * splitting];
			for (var y = 0; y < lines.Length; y++)
			{
				for (var x = 0; x < lines[0].Length; x++)
				{
					switch (lines[y][x])
					{
						case 'G':
							{
								for (int dx = 0; dx < splitting; dx++)
								{
									for (int dy = 0; dy < splitting;  dy++)
										map[splitting * x + dx, splitting * y + dy] = Block.Ground;
								}
								break;
							}
						case 'P':
                            {
								ents.Add(new Player(100, new Vector(x * splitting, y * splitting), 
									Properties.Resources.Player.Width / 50, Properties.Resources.Player.Height / 75, Properties.Resources.Player));
								//sadovnichek: я создал тестовое изображение персонажа и закинул в Resources. Так вроде бы лучше.
								break;
                            }
						case 'E':
                            {
								ents.Add(new Enemy(100, new Vector(x * splitting, y * splitting),
									Properties.Resources.Agressor.Width / 50, Properties.Resources.Agressor.Height / 71, Properties.Resources.Agressor));
								break;
							}
						default:
							{
								for (int dx = 0; dx < splitting; dx++)
								{
									for (int dy = 0; dy < splitting; dy++)
										map[splitting * x + dx, splitting * y + dy] = Block.Empty;
								}
								break;
							}
					}
				}
			}
			return new Level(map, ents.ToArray());
		}

		public bool InBounds(Point point)
        {
			return point.X >= 0 && point.X <= Map.GetLength(1) && point.Y >= 0 && point.Y <= Map.GetLength(0);
        }
	}
}