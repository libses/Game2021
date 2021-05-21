using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace WindowsFormsApp1
{
    [TestFixture]
    class Physics_should
    {
        [Test]
        public void CollideObstacles()
        {
            var map = new string[] { "BBB",
                                     "B#B",
                                     "BPB",
                                     "BGB"};
            var level = Level.FromLines(map, 1);
            var physics = new Physics(level);
            var player = level.Entities.Where(x => x is Player).FirstOrDefault();
            Assert.IsTrue(physics.CollideObstacle(player, Block.Ground).Contains("down"));
            Assert.IsTrue(physics.CollideObstacle(player, Block.Bound).Contains("left"));
            Assert.IsTrue(physics.CollideObstacle(player, Block.Bound).Contains("right"));
        }

        [Test]
        public void DoGravity()
        {
            var map = new string[] { "BBBBB",
                                     "B#P#B",
                                     "B###B",
                                     "BGGGB"};
            var level = Level.FromLines(map, 1);
            var physics = new Physics(level);
            var player = level.Entities.Where(x => x is Player).FirstOrDefault();
            Assert.IsFalse(physics.CollideObstacle(player, Block.Ground).Contains("down"));
            var form = new GameForm(level);
            Application.Run(form);
            form.Close();
            Assert.IsTrue(physics.CollideObstacle(player, Block.Ground).Contains("down"));

        }

        [Test]
        public void DoRun()
        {
            var map = new string[] { "BBBB",
                                     "B##B",
                                     "BP#B",
                                     "BGGB"};
            var level = Level.FromLines(map, 1);
            var physics = new Physics(level);
            var player = level.Entities.Where(x => x is Player).FirstOrDefault();
            var form = new GameForm(level);
            var start = player.Location;
            Application.Run(form);
            player.IsRight = true;
            form.Close();
            Assert.AreEqual(player.Location, start + new Vector(20,0));
        }
    }
}