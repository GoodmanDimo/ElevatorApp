using ElevatorApp.Utility;

namespace ElevatorApp.Elevators
{
    /// <summary>
    /// Passenger elevator class to handle elevator logic related to passengers.
    /// </summary>
    public class PassengerElevator(int capacity) : Elevator(capacity)
    {
        /// <summary>
        /// Load passengers method to handle the logic of boarding passengers to the elevator.
        /// </summary>
        /// <param name="passengers">Number of passengers waiting to board the elevator.</param>
        /// <returns></returns>
        public override Responses LoadPassengers(int passengers)
        {
            if (passengers == 0)
                return Responses.InvalidInput;

            if (Passengers + passengers > Capacity)
            {
                return Responses.CapacityExceeded;
            }
            else
            {
                Passengers += passengers;
                return Responses.ValidInput;
            }
        }

        public override void UnloadPassengers(int numPassengers)
        {
            Passengers -= numPassengers;

            if (Passengers < 0)
                Passengers = 0;
        }
    }
}