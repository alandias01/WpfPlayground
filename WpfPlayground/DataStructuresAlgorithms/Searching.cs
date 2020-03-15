using System;

namespace WpfPlayground.DataStructuresAlgorithms
{
    public class Searching
    {
        int[] arr = { 11, 12, 13, 14, 15 };
        string[] words = { "alan", "Sybil", "Ken", "Ben", "Willy", "John", "Mike", "Cartman" };
        public Searching()
        {
            Console.WriteLine(SeqSearch(arr, 13));
            Console.WriteLine(SeqSearch_better(words, "Ben"));
            Console.WriteLine(SeqSearch_80_20(words, "Ben"));
        }

        public int SeqSearch(int[] arr, int sValue)
        {
            for (int index = 0; index <= arr.GetUpperBound(0); index++)
                if (arr[index] == sValue)
                    return index;
            return -1;
        }

        public int find_min(int[] arr)
        {
            int min = arr[0];
            foreach (int m in arr)
                min = m < min ? m : min;
            return min;
        }

        public int find_max(int[] arr)
        {
            int max = arr[0];
            foreach (int m in arr)
                max = m > max ? m : max;
            return max;
        }

        static int SeqSearch_better(string[] arr, string value) //If found, moves item closer
        {
            for (int i = 0; i <= arr.GetUpperBound(0); i++)
            {
                if (arr[i] == value)
                {
                    swap(arr, i);
                    return i;
                }
            }
            return -1;
        }

        static int SeqSearch_80_20(string[] arr, string value)
        {
            //If the item is outside the first 20% of the array, then move it closer by 1
            //Ex. searching for o in 'aeiou' reorders to 'aeoiu'.  Only 'a' is in 20% range so 
            //if we were searching for 'a', it wouldn't be swapped

            for (int i = 0; i <= arr.GetUpperBound(0); i++)
            {
                if (arr[i] == value && i > (arr.GetUpperBound(0) * .2))
                {
                    swap(arr, i);
                    return i;
                }

                else if (arr[i] == value)
                { return i; }

            }
            return -1;
        }

        public int binary_search(int[] arr, int value)
        {
            //Works only if collection is sorted
            int upper, lower, mid;
            lower = 0; upper = arr.GetUpperBound(0);

            while (lower <= upper)
            {
                mid = (lower + upper) / 2;
                if (arr[mid] == value) { return mid; }

                else if (value < arr[mid]) { upper = mid - 1; }
                else { lower = mid + 1; }
            }
            return -1;
        }

        public int recursive_bin_search(int[] arr, int value, int lower, int upper)
        {
            int mid;
            mid = (upper + lower) / 2;
            if (value < arr[mid])
                return recursive_bin_search(arr, value, lower, mid - 1);
            else if (value == arr[mid])
                return mid;
            else
                return recursive_bin_search(arr, value, mid + 1, upper);
        }

        //Recursion
        public int fibon(int n)
        {
            if (n > 1)
                return n * fibon(n - 1);
            else return 1;
        }

        public double multipleCashFlow(double c, double r, double n)
        {
            if (n <= 1) { return c / (Math.Pow(1 + r, 1)); }
            else
            {
                double val = c / (Math.Pow(1 + r, n));
                return val + multipleCashFlow(c, r, n - 1);
            }
        }

        static void swap(string[] arr, int index)
        {
            string temp = arr[index];
            arr[index] = arr[index - 1];
            arr[index - 1] = temp;
        }
    }
}
