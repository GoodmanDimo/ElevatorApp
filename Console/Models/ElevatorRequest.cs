namespace ElevatorApp.Models
{
    /// <summary>
    /// Class model for defining properties related to calling the elevator.
    /// </summary>
    public class ElevatorRequest
    {
        public int FloorNumber { get; set; }
        public int NumPassengersWaiting { get; set; }
    }
}