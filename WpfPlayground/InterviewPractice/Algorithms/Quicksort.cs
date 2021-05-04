using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPlayground.InterviewPractice.Algorithms
{
    public class Quicksort
    {        
        public Quicksort()
        {
            int[] arr = { 1, 7, 3, 2, 6, 9, 4, 5, 8, 11 };
            this.QuickSortAlgo(arr, 0, arr.Length - 1);
        }

        private void QuickSortAlgo(int[] arr, int left, int right)
        {
            if (left >= right)
                return;

            int pivot = arr[(left + right) / 2];
            int index = Partition(arr, left, right, pivot);
            this.QuickSortAlgo(arr, left, index - 1);
            this.QuickSortAlgo(arr, index, right);
        }

        private int Partition(int[] arr, int left, int right, int pivot)
        {
            while (left <= right)
            {
                while (arr[left] < pivot) left++;
                while (arr[right] > pivot) right--;
                if (left <= right)
                {
                    this.Swap(arr, left, right);
                    left++;
                    right--;
                }
            }
            return left;
        }

        private void Swap(int[] arr, int left, int right)
        {
            int temp = arr[left];
            arr[left] = arr[right];
            arr[right] = temp;
        }
    }
}
