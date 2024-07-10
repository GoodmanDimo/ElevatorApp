using ElevatorApp.Elevators;
using ElevatorApp.Utility;

namespace ElevatorApp.Models
{
    public class ElevatorResponse
    {
        public Elevator Elevator { get; set; } = new PassengerElevator(Constants.ElevatorCapacity);
        public ElevatorRequest Request { get; set; } = new ElevatorRequest();
    }
}