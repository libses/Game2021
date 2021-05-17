using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}