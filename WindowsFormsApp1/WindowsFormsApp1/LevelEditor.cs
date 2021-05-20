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
		Ground,
		Bound,
		Player,
		Enemy,
		Coin
	}

	public class Level
	{
		public Vector mousePosition;
		public readonly Block[,] Map;
		public List<Entity> Entities;
		public List<Spawner> Spawners;
		public List<Coin> Coins;

		private Level(Block[,] level, List<Entity> ents, List<Spawner> spwn, List<Coin> coins)
		{
			Map = level;
			Entities = ents;
			Spawners = spwn;
			Coins = coins;
		}

		public Level(Level source)
        {
			Map = source.Map;
			Entities = source.Entities;
        }

		public static Level FromText(string text, int splitting)
		{
			var lines = text.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			return FromLines(lines, splitting);
		}

		public static Level FromLines(string[] lines, int splitting)
		{
			var spwn = new List<Spawner>();
			var ents = new List<Entity>();
			var coins = new List<Coin>();
			var map = new Block[lines[0].Length * splitting, lines.Length * splitting];
			for (var y = 0; y < lines.Length; y++)
			{
				for (var x = 0; x < lines[0].Length; x++)
				{
					switch (lines[y][x])
					{
						case 'S':
                            {
								var v = new Vector(x * 20 + 10, y * 20 + 10);
								var s = new Spawner(v);
								spwn.Add(s);
								break;
                            }
						case 'C':
							{
								var coin = new Coin(new Vector(x * 20 + 10, y * 20 + 10), Properties.Resources.coin);
								coins.Add(coin);
								break;
							}
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
								var player = new Player(200, new Vector(x * 20 + 10, y * 20 + 10),
									10, 10, Properties.Resources.PlayerStay,
									new Dictionary<string, Bitmap[]>()
									{
										{ "run", new Bitmap[]
											{
												Properties.Resources.PlayerRun1,
												Properties.Resources.PlayerRun2,
												Properties.Resources.PlayerRun3,
												Properties.Resources.PlayerRun4,
												Properties.Resources.PlayerStay
											}
										}
									}
									,
									new Dictionary<string, Bitmap[]>()
									{
										{ "run", new Bitmap[]
											{
												Properties.Resources.PlayerRun1v,
												Properties.Resources.PlayerRun2v,
												Properties.Resources.PlayerRun3v,
												Properties.Resources.PlayerRun4v,
											}
										}
									}, Properties.Resources.PlayerStayv
										);
								player.CurrentGun = new Pistol(Properties.Resources.EnemyStay, player);
								//ЗАХАРДКОЖЕН СПЛИТТИНГ
								ents.Add(player);
								break;
                            }
						case 'E':
                            {
								var dictB = new Dictionary<string, Bitmap[]>()
									{
										{ "run", new Bitmap[]
											{
												Properties.Resources.EnemyRun1,
												Properties.Resources.EnemyRun2,
												Properties.Resources.EnemyRun3,
												Properties.Resources.EnemyRun4,
												Properties.Resources.EnemyRun5,
												Properties.Resources.EnemyRun6,
												Properties.Resources.EnemyStay
											}
										},
										{ "fight", new Bitmap[]
											{
												Properties.Resources.EnemyFight1,
												Properties.Resources.EnemyFight2,
												Properties.Resources.EnemyFight3,
												Properties.Resources.EnemyFight4,
												Properties.Resources.EnemyFight5,
												Properties.Resources.EnemyStay
											}
										}
									};
								ents.Add(new Enemy(100, new Vector(x * 20 + 10, y * 20 + 10),
									10, 10, Properties.Resources.EnemyStay, 
									dictB, dictB, Properties.Resources.EnemyStay, map));
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
			return new Level(map, ents.ToList(), spwn, coins);
		}

		public bool InBounds(Point point)
        {
			return point.X >= 0 && point.X <= Map.GetLength(1) && point.Y >= 0 && point.Y <= Map.GetLength(0);
        }

		public void Remove(Entity entity)
        {
			var ents = Entities.ToList();
			ents.Remove(entity);
			Entities = ents.ToList();
        }
	}
}