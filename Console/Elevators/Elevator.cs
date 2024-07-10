using ElevatorApp.Utility;

namespace ElevatorApp.Elevators
{
    /// <summary>
    /// Elevator to handle elevator logic.
    /// </summary>
    public abstract class Elevator
    {
        public int ElevatorIndex { get; set; }
        public int CurrentFloor { get; set; }
        public Direction Direction { get; set; }
        public bool IsMoving { get; set; }
        public bool IsDoorClosed { get; set; }
        public int Capacity { get; set; }

        public int TargetFloor { get; set; }
        public int Passengers { get; set; }

        public Elevator(int capacity)
        {
            Capacity = capacity; ;
        }

        public abstract Responses LoadPassengers(int passengers);

        public abstract void UnloadPassengers(int passengers);
    }
}