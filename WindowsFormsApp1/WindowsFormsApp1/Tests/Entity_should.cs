using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace WindowsFormsApp1
{
    [TestFixture]
    class Entity_should
    {
        [Test]
        public void IsOneEntityExists()
        {
            var map = new string[] { "BBB",
                                     "BPB",
                                     "B#B",
                                     "BGB"};
            var level = Level.FromLines(map, 1);
            Assert.IsTrue(level.Entities.Count != 0);
        }

        [Test]
        public void IsCorrectIdentifyEntity()
        {
            var map = new string[] { "BBBB",
                                     "BP#B",
                                     "B#EB",
                                     "BGGB"};
            var level = Level.FromLines(map, 1);
            Assert.IsTrue(level.Entities.Count == 2);
            Assert.IsTrue(level.Entities.Where(x => x is Player).FirstOrDefault() != null);
            Assert.IsTrue(level.Entities.Where(x => x is Enemy).FirstOrDefault() != null);
        }

        [Test]
        public void IsAnimationWorks()
        {
            var map = new string[] { "BBBB",
                                     "B##B",
                                     "BP#B",
                                     "BGGB"};
            var level = Level.FromLines(map, 1);
            var form = new GameForm(level);
            Application.Run(form);
            var player = form.player;
            player.Run(1, form.physics);
            Assert.IsTrue(player.currentSprite != player.originalSprite);
        }

        [Test]
        public void BresenhamAlgorithm1()
        {
            var map = new string[] { "BBBBB",
                                     "B###B",
                                     "BP#EB",
                                     "BGGGB"};
            var level = Level.FromLines(map, 1);
            var enemy = (Enemy)level.Entities.Where(x => x is Enemy).FirstOrDefault();
            var player = (Player)level.Entities.Where(x => x is Player).FirstOrDefault();
            var path = enemy.BresenhamAlgorithm(enemy.Location, player.Location, level.Map);
            Assert.IsFalse(path.Contains(Block.Ground));
        }

        [Test]
        public void BresenhamAlgorithm2()
        {
            var map = new string[] { "BBBBB",
                                     "B###B",
                                     "BPGEB",
                                     "BGGGB"};
            var level = Level.FromLines(map, 1);
            var enemy = (Enemy)level.Entities.Where(x => x is Enemy).FirstOrDefault();
            var player = (Player)level.Entities.Where(x => x is Player).FirstOrDefault();
            var path = enemy.BresenhamAlgorithm(enemy.Location, player.Location, level.Map);
            Assert.IsTrue(path.Contains(Block.Ground));
        }

        [Test]
        public void BresenhamAlgorithm3()
        {
            var map = new string[] { "BBBBBBBBBB",
                                     "B#######EB",
                                     "BP###GGGGB",
                                     "BGGGGGGGGB"};
            var level = Level.FromLines(map, 1);
            var enemy = (Enemy)level.Entities.Where(x => x is Enemy).FirstOrDefault();
            var player = (Player)level.Entities.Where(x => x is Player).FirstOrDefault();
            var path = enemy.BresenhamAlgorithm(enemy.Location, player.Location, level.Map);
            Assert.IsFalse(path.Contains(Block.Ground));
        }

        [Test]
        public void EnemyFollowsPlayer()
        {
            var map = new string[] { "BBBBBBBBBB",
                                     "B#######EB",
                                     "BP###GGGGB",
                                     "BGGGGGGGGB"};
            var level = Level.FromLines(map, 1);
            var enemy = (Enemy)level.Entities.Where(x => x is Enemy).FirstOrDefault();
            var player = (Player)level.Entities.Where(x => x is Player).FirstOrDefault();
            var form = new GameForm(level);
            Application.Run(form);
            var distance = player.Location - enemy.Location;
            Assert.IsTrue(distance.Length < 20);
        }

        [Test]
        public void EnemyAttacksPlayer()
        {
            var map = new string[] { "BBBBBBBBBB",
                                     "B########B",
                                     "BPE##GGGGB",
                                     "BGGGGGGGGB"};
            var level = Level.FromLines(map, 1);
            var enemy = (Enemy)level.Entities.Where(x => x is Enemy).FirstOrDefault();
            var player = (Player)level.Entities.Where(x => x is Player).FirstOrDefault();
            var form = new GameForm(level);
            Application.Run(form);
            Assert.AreEqual(player.HP, 0);
        }
    }
}