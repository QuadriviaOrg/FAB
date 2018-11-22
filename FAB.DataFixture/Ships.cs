using Quadrivia.FunctionalLibrary;

namespace Quadrivia.FAB
{
    public static class Ships
    {
        //Returns five ships with no placings. Intention is that after creating a new
        // GameBoard with this set, you would call RandomiseShipPlacement() on it.
        public static FList<Ship> UnplacedShips5()
        {
             return FList.New(
                new Ship(Battleships.AircraftCarrier,5),
                new Ship(Battleships.Battleship, 4),
                new Ship(Battleships.Submarine, 3),
                new Ship(Battleships.Destroyer, 3),
                new Ship(Battleships.PatrolBoat, 2)
            );
        }
        public static FList<Ship> UnplacedShips4()
        {
            return FList.New(
                new Ship(Battleships.PatrolBoat, 2),
                new Ship(Battleships.PatrolBoat, 2),
                new Ship(Battleships.PatrolBoat, 2),
                new Ship(Battleships.PatrolBoat, 2)
            );
        }


        //Contains only two small ships.  Intent is for testing a complete game scenario.
        public static FList<Ship> SmallTestGame()
        {
            return FList.New(
                new Ship(Battleships.Minesweeper, 1,  new Location(2, 3), Orientations.Horizontal),
                new Ship(Battleships.Frigate, 2, new Location(4, 5), Orientations.Vertical)
            );
        }
    }
}
