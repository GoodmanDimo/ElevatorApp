using ElevatorApp.Elevators;
using ElevatorApp.Utility;

namespace ElevatorApp.Buildings
{
    /// <summary>
    /// Building class to handle logic related to handling of the elevators in a buliding.
    /// </summary>
    public class Building
    {
        public List<Elevator> Elevators { get; set; }
        public Elevator Elevator { get; set; }

        public Building()
        {
            InitialiseElevators();
        }

        /// <summary>
        /// Method to initialise elevators in a building.
        /// </summary>
        private void InitialiseElevators()
        {
            Elevators = new List<Elevator>();

            // Initialize elevators
            for (int i = 0; i < Constants.NumberOfElevators; i++)
            {
                Elevator = new PassengerElevator(Constants.ElevatorCapacity);
                Elevator.ElevatorIndex = i + 1;
                Elevator.Capacity = Constants.ElevatorCapacity;
                Elevator.Direction = Direction.Stationary;
                Elevators.Add(Elevator);
            }
        }

        /// <summary>
        /// Method to find the nearest elevator to the requested floor number.
        /// </summary>
        public Elevator FindNearestElevator(int targetFloorNumber)
        {
            Elevator nearestElevator = new PassengerElevator(Constants.ElevatorCapacity);
            int minDistance = int.MaxValue;

            // Find the nearest available elevator unisn the min algorithm
            foreach (var elevator in Elevators)
            {
                if (!elevator.IsMoving && elevator.Direction == Direction.Stationary)
                {
                    var distanceToFloor = elevator.CurrentFloor - targetFloorNumber;
                    var absolututeDistance = Math.Abs(distanceToFloor);

                    int distance = absolututeDistance;

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestElevator = elevator;
                        nearestElevator.Direction = distanceToFloor > 0 ? Direction.Up : Direction.Down;
                    }
                }
            }

            return nearestElevator;
        }
    }
}