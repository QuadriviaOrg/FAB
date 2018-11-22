using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Quadrivia.FunctionalLibrary;
using System.Collections.Immutable;

namespace Quadrivia.FAB
{
    [TestClass]
    public class GameScenarioTests
    {
        private FRandom  Random = null;
        private ImmutableHashSet<Location>  noMisses = ImmutableHashSet.Create<Location>();
        [TestInitialize] 
        public void TestInitialize()
        {
            Random = FRandom.SeedDefault();
        }

        [TestCleanup] 
        public void TestCleanUp()
        {
            Random = null;
        }

        [TestMethod]
        public void TestHitWithMissile()
        {
            var ships = Battleships.TrainingGame();
            var board = new GameBoard(10, ships, "", noMisses);       
            board = Battleships.fireMissile(new Location(8, 1), board);
            Assert.AreEqual("Hit a Battleship at (8,1).", board.Messages);
            var battleship = FList.Head(FList.Filter(s => s.Name == Battleships.Battleship, board.Ships));
            Assert.AreEqual(1, battleship.Hits.Count());
            Assert.IsFalse(Battleships.isSunk(battleship));
            Assert.AreEqual(0, board.Misses.Count());
        }

        [TestMethod]
        public void RepeatedHitOnSameSquareDoesNotIncreaseHitCount()
        {
            var ships = Battleships.TrainingGame();
            var board = new GameBoard(10, ships, "", noMisses);
            var loc = new Location(8, 1);
            board = Battleships.fireMissile(loc, board);
            Assert.AreEqual("Hit a Battleship at (8,1).", board.Messages);
            var battleship = FList.Head(FList.Filter(s => s.Name == Battleships.Battleship, board.Ships));
            Assert.AreEqual(1, battleship.Hits.Count());
            Assert.IsFalse(Battleships.isSunk(battleship));
            board = Battleships.fireMissile(loc, board);
            Assert.AreEqual("Hit a Battleship at (8,1).", board.Messages);
             battleship = FList.Head(FList.Filter(s => s.Name == Battleships.Battleship, board.Ships));
            Assert.AreEqual(1, battleship.Hits.Count());
            board = Battleships.fireMissile(loc, board);
            board = Battleships.fireMissile(loc, board);
            board = Battleships.fireMissile(loc, board);
            battleship = FList.Head(FList.Filter(s => s.Name == Battleships.Battleship, board.Ships));
            Assert.AreEqual(1, battleship.Hits.Count());
            Assert.IsFalse(Battleships.isSunk(battleship));
        }

        [TestMethod]
        public void TestMissWithMissile()
        {
            var ships = Battleships.TrainingGame();
            var board = new GameBoard(10, ships, "", noMisses);
            board = Battleships.fireMissile(new Location(7, 1), board);
            Assert.AreEqual(1, board.Misses.Count());
            Assert.AreEqual("Sorry, (7,1) is a miss.", board.Messages);
            board = Battleships.fireMissile(new Location(6, 1), board);
            Assert.AreEqual(2, board.Misses.Count());
            Assert.AreEqual("Sorry, (6,1) is a miss.", board.Messages);

        }

        [TestMethod]
        public void TestBombWithHits()
        {
            var ships = Battleships.TrainingGame();
            var board = new GameBoard(10, ships, "", noMisses);
            board = Battleships.fireBomb(new Location(7, 2), board);
            var expected = "Sorry, (6,1) is a miss.Sorry, (6,2) is a miss.Sorry, (6,3) is a miss." +
               "Sorry, (7,1) is a miss.Sorry, (7,2) is a miss.Sorry, (7,3) is a miss." +
               "Hit a Battleship at (8,1).Hit a Battleship at (8,2).Hit a Battleship at (8,3).";
            Assert.AreEqual(expected, board.Messages);
            var battleship = FList.Head(FList.Filter(s => s.Name == Battleships.Battleship, board.Ships));
            Assert.AreEqual(3, battleship.Hits.Count());
            Assert.IsFalse(Battleships.isSunk(battleship));
        }

        [TestMethod]
        public void TestSunk()
        {
            var ships = Ships.SmallTestGame();
            var board = new GameBoard(10, ships, "", noMisses);
            board = Battleships.fireMissile(new Location(4,5), board);
            Assert.AreEqual("Hit a Frigate at (4,5).", board.Messages);
            var frigate = FList.Head(FList.Filter(s => s.Name == Battleships.Frigate, board.Ships));
            Assert.AreEqual(1, frigate.Hits.Count());
            Assert.IsFalse(Battleships.isSunk(frigate));
            board = Battleships.fireMissile(new Location(4, 6), board);
            frigate = FList.Head(FList.Filter(s => s.Name == Battleships.Frigate, board.Ships));
            Assert.AreEqual(2, frigate.Hits.Count());
            Assert.IsTrue(Battleships.isSunk(frigate));
            Assert.AreEqual("Frigate sunk!", board.Messages);
        }

        [TestMethod]
        public void TestGameOver()
        {
            var ships = Ships.SmallTestGame();
            var board = new GameBoard(10, ships, "", noMisses);
            board = Battleships.fireMissile(new Location(4, 5), board);
            Assert.AreEqual("Hit a Frigate at (4,5).", board.Messages);
            board = Battleships.fireMissile(new Location(4, 6), board);
            Assert.AreEqual("Frigate sunk!", board.Messages);
            board = Battleships.fireMissile(new Location(2, 3), board);
            Assert.AreEqual("Minesweeper sunk!All ships sunk!", board.Messages);
        }

        //Uses the (mock) RandomRandomGenerator. It is not
        //testing the randomness, but that the outcomes are correct
        [TestMethod]
        public void TestRandomPlacement()
        {
            var unplacedShips = Ships.UnplacedShips4();
            var board = Battleships.createBoardWithShipsPlacedRandomly(10, unplacedShips, Random);
            var ships = board.Ships;
            var s0 = FList.Head(ships);
            var s1 = FList.Head(FList.Tail(ships));
            var s2 = FList.Head(FList.Tail(FList.Tail(ships)));
            var s3 = FList.Head(FList.Tail(FList.Tail(FList.Tail(ships))));

            //2 1 0 7 6 0

            Assert.IsTrue(Battleships.occupies(s0, new Location(5, 1)));
            Assert.IsTrue(Battleships.occupies(s0, new Location(6, 1)));

            Assert.IsTrue(Battleships.occupies(s1, new Location(2, 8)));
            Assert.IsTrue(Battleships.occupies(s1, new Location(3, 8)));
        }
    }
}
