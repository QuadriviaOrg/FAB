using Quadrivia.FunctionalLibrary;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Quadrivia.FAB
{
    public static class Battleships
    {
        private const string AllSunkMsg = "All ships sunk!";
        private const string MissMsg = "Sorry, ({0},{1}) is a miss.";
        private const string PlacingMsg = "Computer placing the {0}\n";

        public static bool allSunk(FList<Ship> ships)
        {
            return !FList.Any(ship => !isSunk(ship), ships);
        }

        public static GameBoard checkSquareAndRecordOutcome(GameBoard board, Location loc, bool aggregate = false)
        {
            return HitSomething(board, loc) ?
                allSunk(ShipsAfterFiring(board, loc)) ?
                     new GameBoard(board.Size, ShipsAfterFiring(board, loc), MessagesAfterFiring(board, loc) + AllSunkMsg, board.Misses)
                     : new GameBoard(board.Size, ShipsAfterFiring(board, loc), AggregatedMessages(board, aggregate, MessagesAfterFiring(board, loc)), board.Misses)
                : new GameBoard(board.Size, ShipsAfterFiring(board, loc), AddMissMsg(board, loc, aggregate, MessagesAfterFiring(board, loc)), board.Misses.Add(loc));
        }

        private static bool HitSomething(GameBoard board, Location loc)
        {
            return FList.FoldL((a, b) => a | b, false, FList.Map(s => fireAt(s, loc).Item2, board.Ships));
        }

        private static string MessagesAfterFiring(GameBoard board, Location loc)
        {
            return FList.FoldL((a, b) => a + b, "",FList.Map(s => fireAt(s, loc).Item3, board.Ships));
        }

        private static FList<Ship> ShipsAfterFiring(GameBoard board, Location loc)
        {
            return FList.Map(s => fireAt(s, loc).Item1, board.Ships);
        }

        private static string AddMissMsg(GameBoard board, Location loc, bool aggregate, string newMessages)
        {
            return AggregatedMessages(board, aggregate, newMessages) + string.Format(MissMsg, loc.Col, loc.Row);
        }

        private static string AggregatedMessages(GameBoard board, bool aggregateMessages, string newMessages)
        {
            return aggregateMessages ? board.Messages + newMessages : newMessages;
        }

        public static GameBoard checkSquaresAndRecordOutcome(GameBoard board, FList<Location> locs)
        {
            return FList.Length(locs) == 1 ?
                checkSquareAndRecordOutcome(board, FList.Head(locs), true)
                : checkSquaresAndRecordOutcome(checkSquareAndRecordOutcome(board, FList.Head(locs), true), FList.RemoveFirst(FList.Head(locs), locs));
        }

        public static bool isValidPosition(int boardSize, FList<Ship> existingShips, Ship shipToBePlaced)
        {
            return !shipWouldFitWithinBoard(boardSize, shipToBePlaced) ?
                 false
                 : !FList.Any(s => intersects(s, shipToBePlaced), existingShips);
        }

        public static FList<Location> locationsThatShipWouldOccupy(Location loc, Orientations orient, int locsToAdd)
        {
            return locsToAdd == 0 ?
                 FList.Empty<Location>()
                 : orient == Orientations.Horizontal ?
                         locationsThatShipWouldOccupy(Add(loc, 1, 0), orient, locsToAdd - 1)
                         : FList.Prepend(loc,locationsThatShipWouldOccupy(Add(loc,0, 1), orient, locsToAdd - 1));
        }

        public static bool shipWouldFitWithinBoard(int boardSize, Ship ship)
        {
            return (ship.Orientation == Orientations.Horizontal
                        && ship.Location.Col + ship.Size <= boardSize)
                    ||
                    (ship.Orientation == Orientations.Vertical
                        && ship.Location.Row + ship.Size <= boardSize);
        }

        public static SquareValues readSquare(GameBoard board, Location loc)
        {
            return FList.Any(s => isHitInLocation(s,loc), board.Ships) ?
                SquareValues.Hit
                : board.Misses.Contains(loc) ?
                    SquareValues.Miss
                    : SquareValues.Empty;
        }

        public static GameBoard createBoardWithShipsPlacedRandomly(int boardSize, FList<Ship> shipsToBePlaced, FRandom random)
        {
            return new GameBoard(
                boardSize,
                FList.Map(r => r.Item1,locateShipsRandomly(boardSize, shipsToBePlaced, FList.Empty<Ship>(), random)),
                FList.FoldL((r, s) => r + s, "", FList.Map(r => r.Item2,locateShipsRandomly(boardSize, shipsToBePlaced, FList.Empty<Ship>(), random))),
                ImmutableHashSet.Create<Location>()
            );
        }

        public static FList<Tuple<Ship, string>> locateShipsRandomly(int boardSize, FList<Ship> shipsToBePlaced, FList<Ship> shipsAlreadyPlaced, FRandom random)
        {
            return FList.Length(shipsToBePlaced)== 0 ?
                FList.Empty<Tuple<Ship, string>>()
                : FList.New(
                    Tuple.Create(
                        locateShipRandomly(boardSize, shipsAlreadyPlaced, FList.Head(shipsToBePlaced), random).Item1,
                        locateShipRandomly(boardSize, shipsAlreadyPlaced, FList.Head(shipsToBePlaced), random).Item2
                    ),
                    locateShipsRandomly(
                        boardSize,
                        FList.RemoveFirst(FList.Head(shipsToBePlaced), shipsToBePlaced),
                        FList.Prepend(
                            locateShipRandomly(boardSize, shipsAlreadyPlaced, FList.Head(shipsToBePlaced), random).Item1, shipsAlreadyPlaced),
                        locateShipRandomly(boardSize, shipsAlreadyPlaced, FList.Head(shipsToBePlaced), random).Item3
                    )
                );
        }

        public static Tuple<Ship, string, FRandom> locateShipRandomly(
            int boardSize, FList<Ship> shipsAlreadyLocated, Ship shipToBeLocated, FRandom random)
        {
            return Tuple.Create(
                setPosition(shipToBeLocated,
                    getValidRandomPosition(boardSize, shipsAlreadyLocated, shipToBeLocated, random).Item1,
                    getValidRandomPosition(boardSize, shipsAlreadyLocated, shipToBeLocated, random).Item2),
                String.Format(PlacingMsg, shipToBeLocated.Name),
                getValidRandomPosition(boardSize, shipsAlreadyLocated, shipToBeLocated, random).Item3);
        }

        public static Tuple<Location, Orientations, FRandom> getValidRandomPosition(int boardSize, FList<Ship> shipsAlreadyLocated, Ship shipToBeLocated, FRandom random)
        {
            return isValidPosition(
                    boardSize,
                    shipsAlreadyLocated,
                    new Ship(shipToBeLocated.Name, shipToBeLocated.Size, getRandomPosition(boardSize, random).Item1,
                    getRandomPosition(boardSize, random).Item2)
                ) ?
                getRandomPosition(boardSize, random)
                : getValidRandomPosition(
                    boardSize,
                    shipsAlreadyLocated,
                    shipToBeLocated,
                    getRandomPosition(boardSize, random).Item3
                  );
        }

        public static Tuple<Location, Orientations, FRandom> getRandomPosition(int boardSize, FRandom random)
        {
            return Tuple.Create(
                new Location(FRandom.Skip(0, random, 0, boardSize).Number, FRandom.Skip(1, random, 0, boardSize).Number),
                (Orientations)FRandom.Skip(2, random, 0, 2).Number,
                FRandom.Skip(2, random, 0, 2)); //Deliberately uses same as the last call -  this contains the next seed.
        }

        public static Location Add(Location loc, int colInc, int rowInc)
        {
            return new Location(loc.Col + colInc, loc.Row + rowInc);
        }

        public const string AircraftCarrier = "Aircraft Carrier";
        public const string Battleship = "Battleship";
        public const string Submarine = "Submarine";
        public const string Destroyer = "Destroyer";
        public const string PatrolBoat = "Patrol Boat";
        public const string Minesweeper = "Minesweeper";
        public const string Frigate = "Frigate";

        private const string HitMsg = "Hit a {0} at ({1},{2}).";
        private const string SunkMsg = "{0} sunk!";

        public static Ship setPosition(Ship ship, Location loc, Orientations orient)
        {
            return new Ship(ship.Name, ship.Size, ship.Hits, loc, orient);
        }

        //Calculated based on the size and the orientation of the ship
        public static bool occupies(Ship ship, Location loc)
        {
            return ship.Orientation == Orientations.Horizontal ?
                 ship.Location.Row == loc.Row &&
                    loc.Col >= ship.Location.Col && loc.Col < ship.Location.Col + ship.Size
                 : ship.Location.Col == loc.Col &&
                    loc.Row >= ship.Location.Row && loc.Row < ship.Location.Row + ship.Size;
        }

        public static bool horizontal(Ship ship)
        {
            return ship.Orientation == Orientations.Horizontal;
        }

        public static bool vertical(Ship ship)
        {
            return ship.Orientation == Orientations.Vertical;
        }

        //If ship is horizontal, this will be the first and last column numbers;
        //if vertical, the first and last row numbers
        public static Tuple<int, int> extent(Ship ship)
        {
            return horizontal(ship) ?
                Tuple.Create(ship.HeadCol, ship.HeadCol + ship.Size - 1) :
                Tuple.Create(ship.HeadRow, ship.HeadRow + ship.Size - 1);
        }

        public static bool overlaps(Tuple<int, int> extent1, Tuple<int, int> extent2)
        {
            return covers(extent1, extent2.Item1) ||
                covers(extent1, extent2.Item2) ||
                covers(extent2, extent1.Item1); //No need for 4th test
        }
        public static bool covers(Tuple<int, int> extent, int value)
        {
            return value >= extent.Item1 && value <= extent.Item2;
        }
        //Returns true if the two ships would overlap.
        public static bool intersects(Ship ship1, Ship ship2)
        {
            return ship1.Orientation == ship2.Orientation ?
                (horizontal(ship1) && ship1.HeadRow == ship2.HeadRow ||
                    vertical(ship1) && ship1.HeadCol == ship2.HeadCol)
                    && overlaps(extent(ship1), extent(ship2))
                 : horizontal(ship1) && covers(extent(ship1), ship2.HeadCol) && covers(extent(ship2), ship1.Location.Row)
                    || vertical(ship1) && covers(extent(ship1), ship2.HeadRow) && covers(extent(ship2),ship1.HeadCol);
        }

        public static bool isHitInLocation(Ship ship, Location loc)
        {
            return ship.Hits.Contains(loc);
        }

        public static Tuple<Ship, bool, string> fireAt(Ship ship, Location loc)
        {
            return occupies(ship, loc) ?
                Tuple.Create(AddHit(ship,loc), true, HitMessage(ship, loc))
                : Tuple.Create(ship, false, "");
        }

        private static string HitMessage(Ship ship, Location loc)
        {
            return isSunk(AddHit(ship, loc)) ?
                string.Format(SunkMsg, AddHit(ship,loc).Name)
                : string.Format(HitMsg, AddHit(ship,loc).Name, loc.Col, loc.Row);
        }

        private static Ship AddHit(Ship ship, Location loc)
        {
            return new Ship(ship.Name, ship.Size, ship.Hits.Add(loc), ship.Location, ship.Orientation);
        }

        public static bool isSunk(Ship ship)
        {
            return ship.Hits.Count >= ship.Size;
        }

        public static FList<Ship> TrainingGame()
        {
            return FList.New(
                new Ship(AircraftCarrier, 5, new Location(1, 8), Orientations.Horizontal),
                new Ship(Battleship, 4, new Location(8, 1), Orientations.Vertical),
                new Ship(Submarine, 3, new Location(7, 6), Orientations.Vertical),
                new Ship(Destroyer, 3, new Location(5, 9), Orientations.Horizontal),
                new Ship(PatrolBoat, 2, new Location(1, 4), Orientations.Vertical)
            );
        }

        public static GameBoard fireMissile(Location loc, GameBoard board)
        {
            return checkSquareAndRecordOutcome(board, loc);
        }

        public static GameBoard fireBomb(Location loc, GameBoard board)
        {
            return checkSquaresAndRecordOutcome(board, GenerateLocationsToHit(loc.Col, loc.Row, board));
        }

        private static FList<Location> GenerateLocationsToHit(int centreCol, int centreRow, GameBoard board)
        {
            return FList.New(Enumerable.Range(centreCol - 1, 3)
                .SelectMany(col => Enumerable.Range(centreRow - 1, 3),
                (col, row) => new Location(col, row))
                .ToArray());
        }
    }
}