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
		Ground,
		Empty,
		Player,
		Demon,
		Zombie
	}

	public class Level
	{
		public readonly Block[,] CurrentLevel;
		public readonly Point Start;
		public Entity[] entities;


		private Level(Block[,] level)
		{
			CurrentLevel = level;
		}

		public static Level FromText(string text, int splitting)
		{
			var lines = text.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			return FromLines(lines, splitting);
		}

		public static Level FromLines(string[] lines, int splitting)
		{
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
			return new Level(map);
		}
	}
}