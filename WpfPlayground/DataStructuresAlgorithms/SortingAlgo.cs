using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfPlayground.DataStructuresAlgorithms
{
    public class SortingAlgo
    {
        int[] arr = { 5, 6, 8, 3, 0, 2 };
        int upper;
        int numElements;
        public SortingAlgo()
        {
            arr = GenRandArr(11);
            upper = arr.GetUpperBound(0);
            numElements = arr.Count();
            QSort();
        }

        private int[] GenRandArr(int numItems)
        {
            Random rand = new Random();
            HashSet<int> items = new HashSet<int>();

            while (items.Count < numItems)
            {
                items.Add(rand.Next(numItems * 10));
            }

            return items.ToArray();
        }
        
        public void BubbleSort()
        {
         /* This sort campares 2 items at a time that are adjacent and moves 
         * the highest number to the right  After the first pass, we know 
         * the highest number is to the right so the next pass we do, we 
         * wont have to compare the current number to the last item.  Then 
         * for the third pass, we wont have to compare the current item to 
         * the last 2 items.  This contines.  This is why we have 2 loops, outer and inner.
         */
            int temp;
            for (int outer = upper; outer > 1; outer--)
            {
                for (int inner = 0; inner < outer; inner++)
                {
                    if (arr[inner] > arr[inner + 1])
                    {
                        temp = arr[inner];
                        arr[inner] = arr[inner + 1];
                        arr[inner + 1] = temp;
                    }
                }
            }
        }

        public void SelectionSort()
        {
         
         /* This sort starts out assuming the first item is the smallest and 
         * compares the other items to it once it finds something smaller, 
         * the sort makes this the newest min and compares the rest of the 
         * items to it until it finds something smaller.  Once it reaches 
         * all items, we now have the minimum value.  The first value and 
         * the minimum value are replaced.The next round then assumes the 
         * second item is the smallest and repeats the same process by 
         * placing the smallest item in the second spot.
         */
            int min, temp;
            for (int outer = 0; outer < upper; outer++)
            {
                min = outer;
                for (int inner = outer + 1; inner <= upper; inner++)
                {
                    if (arr[inner] < arr[min])
                    {
                        min = inner;
                    }
                }

                temp = arr[outer];
                arr[outer] = arr[min];
                arr[min] = temp;
            }
        }

        public void InsertionSort()
        {
         
         /* This works by starting at index position 1 or the second item and 
         * stores its value into a temp variable.The values before it are 
         * compared to the temp value in a while loop.  If its bigger, it 
         * moves 1 space to right, once we find a value that is not bigger 
         * than temp, temp is place in that spot.
            Ex.

            91, 45, 56, 23 Temp = 45
            91,91,56,23    since 91>temp, 91 pushes right
            45,91,56,23    45 gets place at beginning since there’s nothing on left

            45,91,56,23	temp=56
            45,91,91,23	since 91>temp, 91 pushes right
            45,56,91,23	since 45 is not greater than temp, we place temp here         
         */
            int inner, temp;
            for (int outer = 1; outer < numElements; outer++)
            {
                temp = arr[outer];
                inner = outer;
                while (inner > 0 && arr[inner - 1] >= temp)
                {
                    arr[inner] = arr[inner - 1];
                    --inner;
                }
                arr[inner] = temp;
            }
        }
        
        public void ShellSort()
        {
            int inner, temp;
            int h = 1;
            while (h <= numElements / 3)
                h = h * 3 + 1;

            while (h > 0) //decreases h until h=1
            {
                for (int outer = h; outer < numElements; outer++)
                {
                    temp = arr[outer];
                    inner = outer;
                    while ((inner > h - 1) && arr[inner - h] >= temp)
                    {
                        arr[inner] = arr[inner - h];
                        inner -= h;
                    }
                    arr[inner] = temp;
                }
                h = (h - 1) / 3; //decrease h
            }
        }

        #region QuickSort
        
        public void QSort()
        {
            RecQSort(0, numElements - 1);
        }

        private void RecQSort(int left, int right)
        {
            if (right - left <= 0)
                return;

            else                //Size is 2 or larger
            {
                int pivot = arr[right];
                int partition = Partition(left, right, pivot);
                RecQSort(left, partition - 1);  //Sort Left side
                RecQSort(partition + 1, right); //Sort Right Side
            }
        }

        private int Partition(int left, int right, int pivot)
        {
            int leftPtr = left - 1;     // left (after ++)
            int rightPtr = right;       // right-1 (after --)

            while (true)    //Find Bigger Item
            {
                while (arr[++leftPtr] < pivot)  // find bigger item
                    ;   // (nop)


                while (rightPtr > 0 && arr[--rightPtr] > pivot)// find smaller item
                    ;  // (nop)

                if (leftPtr >= rightPtr) // if pointers cross,
                    break; // partition done

                else // not crossed, so
                    Swap(leftPtr, rightPtr); // swap elements
            }
            Swap(leftPtr, right); // restore pivot
            return leftPtr;
        }

        private void Swap(int item1, int item2)
        {
            int temp = arr[item1];
            arr[item1] = arr[item2];
            arr[item2] = temp;
        }

        #endregion
    }
}
