using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class PathFinder
    {
		public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Level level, Point start, Point player)
		{
			var queue = new Queue<SinglyLinkedList<Point>>();
			var used = new HashSet<Point>();
			var directions = new Point[] { new Point(0, 1), new Point(1, 0), new Point(0, -1), new Point(-1, 0) };
			queue.Enqueue(new SinglyLinkedList<Point>(start));
			used.Add(start);
			while (queue.Count != 0)
			{
				var currentPath = queue.Dequeue();
				var currentPoint = currentPath.Value;
				if (player == currentPoint)
					yield return currentPath;
				foreach (var dir in directions)
				{
					var target = new Point(currentPoint.X + dir.X, currentPoint.Y + dir.Y);
					if (!level.InBounds(target)) continue;
					if (level.Map[target.X, target.Y] == Block.Ground) continue;
					if (!used.Contains(target))
					{
						queue.Enqueue(new SinglyLinkedList<Point>(target, currentPath));
						used.Add(target);
					}
				}
			}
		}
	}
}
