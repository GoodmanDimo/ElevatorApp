using ElevatorApp.Buildings;

namespace UnitTests
{
    [TestFixture]
    public class BuildingTests
    {
        private Building building;

        [SetUp]
        public void Setup()
        {
            building = new Building();
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(8)]
        public void FindNearestElevator_WhenFirstElevatorIsBusy(int targetFloorNumber)
        {
            building.Elevators[0].IsMoving = true;

            var nearestElevator = building.FindNearestElevator(targetFloorNumber);

            if (nearestElevator.ElevatorIndex > 1)
                Assert.Pass($"Test {nameof(FindNearestElevator_WhenFirstElevatorIsBusy)} passed when first elevator is busy");
            else
                Assert.Fail($"Test {nameof(FindNearestElevator_WhenFirstElevatorIsBusy)}. failed when first elevator is busy");
        }

        [TestCase(7)]
        [TestCase(5)]
        public void FindNearestElevator_WhenElevatorIsAtHigerFloors(int targetFloorNumber)
        {
            building.Elevators[6].CurrentFloor = 6;

            var nearestElevator = building.FindNearestElevator(targetFloorNumber);

            if (nearestElevator.ElevatorIndex == 7)
                Assert.Pass($"Test {nameof(FindNearestElevator_WhenElevatorIsAtHigerFloors)} passed when first elevator is far from the requestor");
            else
                Assert.Fail($"Test {nameof(FindNearestElevator_WhenElevatorIsAtHigerFloors)}. failed when first elevator is far from the requestor");
        }
    }
}