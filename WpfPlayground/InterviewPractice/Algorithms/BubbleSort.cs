using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPlayground.InterviewPractice.Algorithms
{
    public class BubbleSort
    {
        public BubbleSort()
        {
            int[] arr = { 1, 7, 3, 2, 6, 9, 4, 5, 8, 11 };
            this.BubbleSortAlgo(arr);
        }

        private void BubbleSortAlgo(int[] arr)
        {

            for(int outer = arr.Length - 1; outer > 1; outer--)
            {
                for(int inner = 0; inner < outer; inner++)
                {
                    if (arr[inner] > arr[inner + 1]) this.Swap(arr, inner);
                }
            }
        }

        private void Swap(int[] arr, int index)
        {
            int temp = arr[index];
            arr[index] = arr[index + 1];
            arr[index + 1] = temp;

        }
    }
}
