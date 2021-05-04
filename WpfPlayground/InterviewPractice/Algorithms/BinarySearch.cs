using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPlayground.InterviewPractice.Algorithms
{
    public class BinarySearch
    {
        public BinarySearch()
        {
            int[] arr = { 1, 3, 5, 6, 7, 8, 9, 11, 13, 17 };
            int index = this.BinarySearchAlgo(arr, 11, 0, arr.Length - 1);
        }

        private int BinarySearchAlgo(int[] arr, int x, int left, int right)
        {
            if (left > right) return -1;
            int mid = (left + right) / 2;
            if (x == arr[mid]) return mid;
            if (x < arr[mid]) return this.BinarySearchAlgo(arr, x, left, mid - 1);
            else return this.BinarySearchAlgo(arr, x, mid + 1, right);
        }
    }
}
