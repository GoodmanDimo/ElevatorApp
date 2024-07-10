using ElevatorApp.Elevators;
using ElevatorApp.Utility;

namespace UnitTests
{
    [TestFixture]
    public class ElevatorTests
    {
        private Elevator elevator;

        [SetUp]
        public void Setup()
        {
            int capacity = Constants.ElevatorCapacity;
            elevator = new PassengerElevator(capacity);
        }

        [Test]
        public void LoadPassengers_Invalid()
        {
            var inValidPassengerRequest = elevator.LoadPassengers(0);

            if (inValidPassengerRequest == Responses.InvalidInput)
                Assert.Pass($"Test Case passed when number of passengers waiting is 0");
            else
                Assert.Fail($"Test case {nameof(LoadPassengers_Invalid)}. The expected output is {Responses.InvalidInput} when number of passengers waiting is 0.");
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(8)]
        public void LoadPassengers_Valid(int numPassengersWaiting)
        {
            var inValidPassengerRequest = elevator.LoadPassengers(numPassengersWaiting);

            if (inValidPassengerRequest == Responses.ValidInput)
                Assert.Pass($"Test {nameof(LoadPassengers_Valid)} passed when number number of passengers waiting is {numPassengersWaiting} based on a maximum capicity of {elevator.Capacity}");
            else
                Assert.Fail($"Test case {nameof(LoadPassengers_Valid)}. failed when number number of passengers waiting is {numPassengersWaiting} based on a maximum capicity of {elevator.Capacity}");
        }

        [TestCase(6)]
        [TestCase(9)]
        [TestCase(8)]
        public void LoadPassengers_CapacityExceeded(int numPassengersWaiting)
        {
            var CapacityExceeded = elevator.LoadPassengers(numPassengersWaiting);

            if (CapacityExceeded == Responses.ValidInput)
                Assert.Pass($"Test {nameof(LoadPassengers_CapacityExceeded)} passed when number number of passengers waiting is {numPassengersWaiting} based on a maximum capicity of {elevator.Capacity}");
            else
                Assert.Fail($"Test {nameof(LoadPassengers_CapacityExceeded)} failed when number number of passengers waiting is {numPassengersWaiting} based on a maximum capicity of {elevator.Capacity}");
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(8)]
        public void UnloadPassengers(int numPassengers)
        {
            elevator.Passengers = 10;
            var oringinalPassengerCounts = elevator.Passengers;
            elevator.UnloadPassengers(numPassengers);

            var remainingPassengers = elevator.Passengers;

            if (remainingPassengers + numPassengers == oringinalPassengerCounts)
                Assert.Pass($"Test {nameof(UnloadPassengers)} passed when number number of passengers offboarding the elevator is {numPassengers} based on  {elevator.Passengers} passengers currently in the elevator.");
            else
                Assert.Fail($"Test case {nameof(UnloadPassengers)}. failed when number number of passengers waiting is {numPassengers} based on  {elevator.Passengers} passengers currently in the elevator.");
        }
    }
}