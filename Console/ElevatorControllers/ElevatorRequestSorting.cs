using ElevatorApp.Models;

namespace ElevatorApp.ElevatorControllers
{
    /// <summary>
    /// Class responsible for sorting elevator requests.
    /// Class is based on the quick sorting algorithm.
    /// </summary>
    public class ElevatorRequestSorting
    {
        public ElevatorRequestSorting()
        { }

        /// <summary>
        /// Quick sorting method based on partitions to get the elevator request that faster and quicker to fulfill.
        /// </summary>
        public void Quicksort(List<ElevatorRequest> arr, int low, int high)
        {
            if (low < high)
            {
                // Partition the array and get the pivot index
                int pivotIndex = Partition(arr, low, high);

                // Recursively sort elements before and after the pivot
                Quicksort(arr, low, pivotIndex - 1);
                Quicksort(arr, pivotIndex + 1, high);
            }
        }

        private int Partition(List<ElevatorRequest> arr, int low, int high)
        {
            int pivot = arr[high].FloorNumber;
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (arr[j].FloorNumber <= pivot)
                {
                    i++;
                    // Swap arr[i] and arr[j]
                    ElevatorRequest temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            // Swap arr[i+1] and arr[high] (pivot)
            ElevatorRequest temp1 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp1;

            return i + 1;
        }
    }
}