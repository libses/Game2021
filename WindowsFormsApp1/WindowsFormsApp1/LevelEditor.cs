﻿using System;
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
		Ground,
		Bound,
		Player,
		Enemy
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

		public Level(Level source)
        {
			Map = source.Map;
			entities = source.entities;
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
								var player = new Player(100, new Vector(x * 20 + 10, y * 20 + 10),
									10, 10, Properties.Resources.Player);
								player.currentGun = new Pistol(Properties.Resources.Agressor, player);
								//ЗАХАРДКОЖЕН СПЛИТТИНГ
								ents.Add(player);
								break;
                            }
						case 'E':
                            {
								ents.Add(new Enemy(100, new Vector(x * 20 + 10, y * 20 + 10),
									10, 10, Properties.Resources.Agressor));
								break;
							}
						case 'B':
							{
								for (int dx = 0; dx < splitting; dx++)
								{
									for (int dy = 0; dy < splitting; dy++)
										map[splitting * x + dx, splitting * y + dy] = Block.Bound;
								}
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

		public void Remove(Entity entity)
        {
			var ents = entities.ToList();
			ents.Remove(entity);
			entities = ents.ToArray();
        }
	}
}