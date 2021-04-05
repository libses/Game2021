using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


		private Level(Block[,] level, Point start)
		{
			CurrentLevel = level;
			Start = start;
		}

		public static Level FromText(string text)
		{
			var lines = text.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			return FromLines(lines);
		}

		public static Level FromLines(string[] lines)
		{
			var map = new Block[lines[0].Length, lines.Length];
			var start = Point.Empty;
			for (var y = 0; y < lines.Length; y++)
			{
				for (var x = 0; x < lines[0].Length; x++)
				{
					switch (lines[y][x])
					{
						case 'G':
							map[x, y] = Block.Ground;
							break;
						default:
							map[x, y] = Block.Empty;
							break;
					}
				}
			}
			return new Level(map, start);
		}

		//public static Block[,] GetPointVersion(string[] lines)
  //      {
		//	var map = new Block[lines[0].Length, lines.Length];
		//}

	}
}