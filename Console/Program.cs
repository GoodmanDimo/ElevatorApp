using ElevatorApp.Buildings;
using ElevatorApp.ElevatorControllers;
using ElevatorApp.Elevators;
using ElevatorApp.Models;
using ElevatorApp.Utility;

internal class Program
{
    public static int lastWriteCursorTop;
    private static readonly List<ElevatorRequest> elevatorRequests = new();

    private static readonly List<ElevatorResponse> elevatorResponses = new();

    private static Elevator? nearestElevator = new PassengerElevator(Constants.ElevatorCapacity);
    private static readonly Building building = new Building();

    private static async Task Main(string[] args)
    {
        //building.InitialiseElevators();

        while (true)
        {
            await ReadUserElevatorCalls();
            await CallElevatorAsync();

            await MoveMultipleElevatorsAsync();

            await LoadPassengersToElevatorsAsync();

            await Task.WhenAll(ReadUserElevatorCalls(), CallElevatorAsync(), MoveMultipleElevatorsAsync(),
                                MoveMultipleElevatorsAsync(), LoadPassengersToElevatorsAsync());
        }
    }

    private static void MoveConsoleBufferOutputs(string consoleMessage)
    {
        try
        {
            //Modifying the console output to print the statuses of the elevator simulations above the inputs prompts.
            lock (Console.Out)
            {
                int messageLines = consoleMessage.Length / Console.BufferWidth + 1;
                int inputBufferLines = Console.CursorTop - lastWriteCursorTop + 1;

                Console.MoveBufferArea(sourceLeft: 0, sourceTop: lastWriteCursorTop,
                                       targetLeft: 0, targetTop: lastWriteCursorTop + messageLines,
                                       sourceWidth: Console.BufferWidth, sourceHeight: inputBufferLines);

                int cursorLeft = Console.CursorLeft;
                Console.CursorLeft = 0;
                Console.CursorTop -= inputBufferLines - 1;
                Console.WriteLine(consoleMessage);

                lastWriteCursorTop = Console.CursorTop;
                Console.CursorLeft = cursorLeft;
                Console.CursorTop += inputBufferLines - 1;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error has occured, Please contact the IT team");
        }
    }

    private static async Task ReadUserElevatorCalls()
    {
        ElevatorRequest elevatorRequest = new();

        try
        {
            Console.WriteLine($"---------------Request Elelevator--------------------------------");

            Console.WriteLine($"Enter floor number to call elevator 1 to {Constants.NumberOfFloors} :");

            if (!int.TryParse(Console.ReadLine(), out int floorNumber) || floorNumber < Constants.StartFloor || floorNumber > Constants.NumberOfFloors)
            {
                Console.WriteLine("Invalid input. Please enter a valid floor number.");
                return;
            }
            else
            {
                elevatorRequest.FloorNumber = floorNumber;
            }

            if (floorNumber > 1)
            {
                Console.WriteLine($"Enter number of people waiting for elevator at floor : {floorNumber}");

                if (!int.TryParse(Console.ReadLine(), out int numWaitingPassengers) || numWaitingPassengers < 1)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number of people waiting.");
                    return;
                }
                else
                {
                    elevatorRequest.NumPassengersWaiting = numWaitingPassengers;
                }
            }

            //avoid more than 1 request to the same floor. Update with the latest num passengers waiting instead
            var floorRequestExist = elevatorRequests.FirstOrDefault(x => x.FloorNumber == floorNumber);

            if (floorRequestExist != null)
            {
                floorRequestExist.NumPassengersWaiting = elevatorRequest.NumPassengersWaiting;
            }
            else
            {
                elevatorRequests.Add(elevatorRequest);
            }

            Console.WriteLine("user inputs delayed");

            await Task.Delay(5000);
            Console.WriteLine("user inputs continue");
            ReadUserElevatorCalls();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error has occured while trying to diplay simulations. Please contact the IT support team.");
        }
    }

    private static async Task CallElevatorAsync()
    {
        if (elevatorRequests.Count > 0)
        {
            ElevatorRequestSorting sort = new ElevatorRequestSorting();
            sort.Quicksort(elevatorRequests, 0, elevatorRequests.Count - 1);

            var request = elevatorRequests.Last();

            nearestElevator = building.FindNearestElevator(request.FloorNumber);

            if (nearestElevator != null)
            {
                MoveConsoleBufferOutputs($"Calling elevator {nearestElevator.ElevatorIndex} to floor {request.FloorNumber}");

                MoveConsoleBufferOutputs($"Elevator {nearestElevator.ElevatorIndex} doors are closing");

                nearestElevator.IsDoorClosed = true;
                nearestElevator.IsMoving = true;

                nearestElevator.TargetFloor = request.FloorNumber;

                var elevatorResponse = new ElevatorResponse
                {
                    Elevator = nearestElevator,
                    Request = request
                };

                elevatorResponses.Add(elevatorResponse);
            }
            else
            {
                MoveConsoleBufferOutputs("All elevators are busy. Please make a new request.");
            }

            //remove request to move the queue
            elevatorRequests.Remove(request);
        }

        await Task.Delay(5000);
        CallElevatorAsync();
    }

    private static async Task MoveMultipleElevatorsAsync()
    {
        if (elevatorResponses.Count > 0)
        {
            List<Task> moveElevatorTasks = new List<Task>();
            foreach (var response in elevatorResponses)
            {
                moveElevatorTasks.Add(Task.Run(async () => await MoveToAsync(response.Elevator)));
            }

            // Wait with delay after all elevator tasks have started
            await Task.WhenAny(Task.WhenAll(moveElevatorTasks), Task.Delay(5000));

            // Wait for all elevator tasks to complete
            //await Task.WhenAll(moveElevatorTasks);
        }

        await Task.Delay(5000);
        MoveMultipleElevatorsAsync();
    }

    private static async Task LoadPassengersToElevatorsAsync()
    {
        if (elevatorResponses.Count > 0)
        {
            // Simulate loading passengers
            List<Task> loadElevatorPassengersTasks = new List<Task>();
            foreach (var response in elevatorResponses)
            {
                loadElevatorPassengersTasks.Add(Task.Run(async () => await LoadPassengersAsync(response)));
            }

            // Wait with delay after all elevator tasks have started
            await Task.WhenAny(Task.WhenAll(loadElevatorPassengersTasks), Task.Delay(5000));

            // Wait for all elevator tasks to complete
            await Task.WhenAll(loadElevatorPassengersTasks);
        }

        await Task.Delay(5000);
        MoveMultipleElevatorsAsync();
    }

    // Method to move elevator to a target floor
    public static async Task MoveToAsync(Elevator nearestElevator)
    {
        if (nearestElevator.IsMoving)
        {
            if (nearestElevator.CurrentFloor == nearestElevator.TargetFloor)
            {
                MoveConsoleBufferOutputs($"Elevator {nearestElevator.ElevatorIndex} is currently at floor {nearestElevator.CurrentFloor} to floor {nearestElevator.TargetFloor}");
            }
            else if (nearestElevator != null)
            {
                MoveConsoleBufferOutputs($"Elevator {nearestElevator.ElevatorIndex} is departing from floor {nearestElevator.CurrentFloor} to floor {nearestElevator.TargetFloor}");

                while (nearestElevator.CurrentFloor != nearestElevator.TargetFloor
                        && nearestElevator.CurrentFloor < Constants.NumberOfFloors
                        && nearestElevator.CurrentFloor >= Constants.StartFloor)
                {
                    // Simulate movement with delays
                    await Task.Delay(5000);
                    if (nearestElevator.Direction == Direction.Up)
                        nearestElevator.CurrentFloor++;
                    else
                        nearestElevator.CurrentFloor--;

                    MoveConsoleBufferOutputs($"Elevator {nearestElevator.ElevatorIndex} is at floor {nearestElevator.CurrentFloor}, and {DirectionOfMovement(nearestElevator)}");
                }

                MoveConsoleBufferOutputs($"Elevator {nearestElevator.ElevatorIndex} arrived at floor {nearestElevator.TargetFloor}.");
                MoveConsoleBufferOutputs($"Elevator {nearestElevator.ElevatorIndex} doors are opening");

                ResetElevatorState(nearestElevator);
            }
        }

        await Task.Delay(5000);
        MoveToAsync(nearestElevator);
    }

    private static void ResetElevatorState(Elevator elevatorResponse)
    {
        elevatorResponse.IsDoorClosed = false;
        elevatorResponse.IsMoving = false;
        elevatorResponse.Direction = Direction.Stationary;
    }

    private static string DirectionOfMovement(Elevator selectedElevator)
    {
        var movementDirection = string.Empty;

        if (!selectedElevator.IsMoving)
        {
            movementDirection = EnumHelper.GetDescription(Direction.Stationary);
        }
        else if (selectedElevator.IsMoving && selectedElevator.Direction == Direction.Down)
        {
            movementDirection = EnumHelper.GetDescription(Direction.Down);
        }
        else if (selectedElevator.IsMoving && selectedElevator.Direction == Direction.Up)
        {
            movementDirection = EnumHelper.GetDescription(Direction.Up);
        }

        return movementDirection;
    }

    private static Responses OnborardingPassengerResponses { get; set; }

    private static void offboardPassengers(Elevator selectedElevator)
    {
        try
        {
            if (OnborardingPassengerResponses == Responses.CapacityExceeded)
            {
                Console.WriteLine($"---------------Off-Board Passengers  Elelevator--------------------------------");

                Console.WriteLine($"Enter number of passenger offboarding the elevator {selectedElevator.ElevatorIndex}:");

                if (!int.TryParse(Console.ReadLine(), out int offloadedPassengers) || offloadedPassengers < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number. Number of people off loading from the elevator needs to be a number and cannot be less than 0");
                }

                Thread.Sleep(5000);
                selectedElevator.UnloadPassengers(offloadedPassengers);
                OnborardingPassengerResponses = Responses.ValidInput;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error has occured. Please contact the IT support team.");
        }
    }

    private static async Task LoadPassengersAsync(ElevatorResponse elevatorResponse)
    {
        var nearestElevator = elevatorResponse.Elevator;
        var originalRequest = elevatorResponse.Request;
        bool capacityExceeded = false;

        if (elevatorResponse.Elevator != null)
        {
            OnborardingPassengerResponses = nearestElevator.LoadPassengers(originalRequest.NumPassengersWaiting);
            var onBoardingPassengers = OnborardingPassengerResponses;

            if (onBoardingPassengers == Responses.InvalidInput)
            {
                MoveConsoleBufferOutputs($"Invalid input.please try again.");
            }
            else if (onBoardingPassengers == Responses.CapacityExceeded)
            {
                MoveConsoleBufferOutputs($"The maximum capacity of elevator {nearestElevator.ElevatorIndex} is {nearestElevator.Capacity} passengers. The number of passengers waiting to onboard elevator {nearestElevator.ElevatorIndex} exceeds maximum capacity. Enter number of passenger On-boarding the elevator {nearestElevator.ElevatorIndex}");

                capacityExceeded = true;
                offboardPassengers(nearestElevator);
            }
            else
            {
                MoveConsoleBufferOutputs($"Elevator {nearestElevator.ElevatorIndex} loaded with {originalRequest.NumPassengersWaiting} passengers");
            }
        }

        if (!capacityExceeded)
        {
            elevatorResponses.Remove(elevatorResponse);
        }

        await Task.Delay(5000);
        LoadPassengersAsync(elevatorResponse);
    }
}