using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quadrivia.FAB
{
    [TestClass]
    public class ShipFunctionsTests
    {
        [TestMethod]
        public void Horizontal1()
        {
            var ship = new Ship("", 5, new Location(3, 4), Orientations.Horizontal);
            Assert.IsTrue(Battleships.horizontal(ship));
        }

        [TestMethod]
        public void Horizontal2()
        {
            var ship = new Ship("", 5, new Location(3, 4), Orientations.Vertical);
            Assert.IsFalse(Battleships.horizontal(ship));
        }

        [TestMethod]
        public void Vertical1()
        {
            var ship = new Ship("", 5, new Location(3, 4), Orientations.Horizontal);
            Assert.IsFalse(Battleships.vertical(ship));
        }
        [TestMethod]
        public void Vertical2()
        {
            var ship = new Ship("", 5, new Location(3, 4), Orientations.Vertical);
            Assert.IsTrue(Battleships.vertical(ship));
        }

        [TestMethod]
        public void Extent()
        {
            var ship = new Ship("", 5, new Location(3, 1), Orientations.Horizontal);
            var extent = Battleships.extent(ship);
            Assert.AreEqual(3, extent.Item1);
            Assert.AreEqual(7, extent.Item2);

            ship = new Ship("", 4, new Location(2, 3), Orientations.Vertical);
            extent = Battleships.extent(ship);
            Assert.AreEqual(3, extent.Item1);
            Assert.AreEqual(6, extent.Item2);
        }
        [TestMethod]
        public void Covers1()
        {
            Assert.IsTrue(Battleships.covers(Tuple.Create(3, 7),4));
        }
        [TestMethod]
        public void Covers2()
        {
            Assert.IsTrue(Battleships.covers(Tuple.Create(3, 7),3));
        }
        [TestMethod]
        public void Covers3()
        {
            Assert.IsTrue(Battleships.covers(Tuple.Create(3, 7),7));
        }
        [TestMethod]
        public void Covers4()
        {
            Assert.IsFalse(Battleships.covers(Tuple.Create(3, 7),2));
        }
        [TestMethod]
        public void Covers5()
        {
            Assert.IsFalse(Battleships.covers(Tuple.Create(3, 7),8));
        }
        [TestMethod]
        public void Overlaps1()
        {
            Assert.IsTrue(Battleships.overlaps(Tuple.Create(3, 7),Tuple.Create(4, 9)));
        }
        [TestMethod]
        public void Overlaps2()
        {
            Assert.IsTrue(Battleships.overlaps(Tuple.Create(3, 7), Tuple.Create(2, 5)));
        }
        [TestMethod]
        public void Overlaps3()
        {
            Assert.IsTrue(Battleships.overlaps(Tuple.Create(3, 7), Tuple.Create(4, 5)));
        }
        [TestMethod]
        public void Overlaps4()
        {
            Assert.IsTrue(Battleships.overlaps(Tuple.Create(3, 7), Tuple.Create(3, 7)));
        }
        [TestMethod]
        public void Overlaps5()
        {
            Assert.IsTrue(Battleships.overlaps(Tuple.Create(3, 7), Tuple.Create(1, 8)));
        }
        [TestMethod]
        public void Overlaps6()
        {
            Assert.IsTrue(Battleships.overlaps(Tuple.Create(3, 7), Tuple.Create(7, 8)));
        }
        [TestMethod]
        public void Overlaps7()
        {
            Assert.IsTrue(Battleships.overlaps(Tuple.Create(3, 7), Tuple.Create(4, 4)));
        }
        [TestMethod]
        public void Overlaps8()
        {
            Assert.IsFalse(Battleships.overlaps(Tuple.Create(3, 7), Tuple.Create(8, 9)));
        }
        [TestMethod]
        public void Overlaps9()
        {
            Assert.IsFalse(Battleships.overlaps(Tuple.Create(3, 7), Tuple.Create(1, 2)));
        }
        [TestMethod]
        public void Overlaps10()
        {
            Assert.IsFalse(Battleships.overlaps(Tuple.Create(1, 1),Tuple.Create(2, 2)));
        }
        [TestMethod]
        public void Intersects1()
        {
            var ship1 = new Ship("", 5, new Location(2, 3), Orientations.Horizontal);
            var ship2 = new Ship("", 3, new Location(5, 3), Orientations.Horizontal);
            Assert.IsTrue(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects2()
        {
            var ship1 = new Ship("", 5, new Location(2, 3), Orientations.Horizontal);
            var ship2 = new Ship("", 3, new Location(6, 3), Orientations.Horizontal);
            Assert.IsTrue(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects3()
        {
            var ship1 = new Ship("", 5, new Location(2, 3), Orientations.Horizontal);
            var ship2 = new Ship("", 3, new Location(7, 3), Orientations.Horizontal);
            Assert.IsFalse(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects4()
        {
            var ship1 = new Ship("", 3, new Location(2, 3), Orientations.Horizontal);
            var ship2 = new Ship("", 3, new Location(2, 4), Orientations.Horizontal);
            Assert.IsFalse(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects5()
        {
            var ship1 = new Ship("", 5, new Location(3, 2), Orientations.Vertical);
            var ship2 = new Ship("", 3, new Location(3, 5), Orientations.Vertical);
            Assert.IsTrue(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects6()
        {
            var ship1 = new Ship("", 5, new Location(3, 2), Orientations.Vertical);
            var ship2 = new Ship("", 3, new Location(3, 6), Orientations.Vertical);
            Assert.IsTrue(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects7()
        {
            var ship1 = new Ship("", 5, new Location(3, 2), Orientations.Vertical);
            var ship2 = new Ship("", 3, new Location(3, 7), Orientations.Vertical);
            Assert.IsFalse(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects8()
        {
            var ship1 = new Ship("", 3, new Location(3, 2), Orientations.Vertical);
            var ship2 = new Ship("", 3, new Location(4, 2), Orientations.Vertical);
            Assert.IsFalse(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects9()
        {
            var ship1 = new Ship("", 5, new Location(1, 8), Orientations.Horizontal);
            var ship2 = new Ship("", 5, new Location(3, 5), Orientations.Vertical);
            Assert.IsTrue(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects10()
        {
            var ship1 = new Ship("", 5, new Location(1, 1), Orientations.Horizontal);
            var ship2 = new Ship("", 3, new Location(1, 1), Orientations.Vertical);
            Assert.IsTrue(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects11()
        {
            var ship1 = new Ship("", 5, new Location(3, 1), Orientations.Vertical);
            var ship2 = new Ship("", 3, new Location(1, 5), Orientations.Horizontal);
            Assert.IsTrue(Battleships.intersects(ship1, ship2));
        }
        [TestMethod]
        public void Intersects12()
        {
            var ship1 = new Ship("", 5, new Location(3, 1), Orientations.Vertical);
            var ship2 = new Ship("", 3, new Location(1, 6), Orientations.Vertical);
            Assert.IsFalse(Battleships.intersects(ship1, ship2));
        }
    }
}
