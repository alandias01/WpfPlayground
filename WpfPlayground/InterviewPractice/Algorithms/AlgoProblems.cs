using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlayground.InterviewPractice.Algorithms
{
    public class AlgoProblems
    {
        public AlgoProblems()
        {
            this.LongestSubstringWithoutRepeatingCharacters2();
            //this.temp();
        }

        private void TwoSums_1()
        {
            int[] arr = { 2, 6, 9, 10 };
            int index1 = 0, index2 = 0, target = 11;
            bool found = false;

            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[i] + arr[j] == target)
                    {
                        index1 = i;
                        index2 = j;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
        }

        private void TwoSums_2()
        {
            int[] arr = { 2, 6, 9, 10 };
            int index1 = 0, index2 = 0, target = 11;

            var numMap = new Dictionary<int, int>();
            for (int i = 0; i < arr.Length; i++)
            {
                int complement = target - arr[i];
                if (numMap.TryGetValue(complement, out int indexOfComplement))
                {
                    index1 = i;
                    index2 = indexOfComplement;
                    break;
                }
                numMap.Add(arr[i], i);
            }
        }

        private void LongestSubstringWithoutRepeatingCharacters()
        {
            int index1 = 0, index2 = 0, longestSubStr = 0;
            bool done = false;

            //string word = "abcabcbb";
            string word = "abcabcdbb";
            //string word = "bbbbb";
            //string word = "pwwkew";

            var letters = new Dictionary<char, int>();

            for (int i = 0; i < word.Length; i++)
            {
                letters.Clear();
                letters[word[i]] = i;

                for (int j = i + 1; j < word.Length; j++)
                {
                    if (letters.ContainsKey(word[j]))
                    {
                        int prevIndex = j - 1;  //We broke at this so last index was good
                        int length = prevIndex - i + 1;
                        if (length > longestSubStr)
                        {
                            longestSubStr = length;
                            index1 = i;
                            index2 = prevIndex;
                        }
                        break;
                    }
                    letters[word[j]] = j;
                }
            }
        }

        private void LongestSubstringWithoutRepeatingCharacters2()
        {
            int index1 = 0, index2 = 0, longestSubStr = 0;
            bool done = false;

            string word = "abcabcdbb";

            var letters = new Dictionary<char, int>();
            int j = 0;
            for (int i = 0; i < word.Length; i++)
            {
                char wordAti = word[i];
                if (letters.ContainsKey(wordAti))
                {
                    j = Math.Max(letters[wordAti], j);
                }
                longestSubStr = Math.Max(longestSubStr, i - j + 1);
                letters[wordAti] = i + 1;
            }
        }

        private void MaxConsecutiveNumbers()
        {
            int[] arr = { 1, 5, 3, -2, 6, -1 };
            int maxConsec = 4;
            int currentMax = 0;
            int arrSize = arr.Length;

            for (int i = 0; i < arrSize; i++)
            {
                int rightBoundary = i + maxConsec;
                if (rightBoundary <= arrSize)
                {
                    int sum = 0;
                    for (int j = i; j < rightBoundary; j++)
                    {
                        sum += arr[j];
                    }

                    currentMax = Math.Max(currentMax, sum);
                }
                else break;
            }
        }

        private void MergeDictionaries()
        {
            Dictionary<string, string> Child = new Dictionary<string, string>();
            Dictionary<string, string> Parent = new Dictionary<string, string>();

            Child.Add("childKey", "childValue");
            Child.Add("sharedKey", "childValue");

            Parent.Add("parentKey", "parentValue");
            Parent.Add("sharedKey", "parentValue");

            foreach (var key in Parent.Keys)
            {
                Child.TryAdd(key, Parent[key]);
            }
        }

        private async Task<string> GetData()
        {
            await Task.Delay(100);
            return "abcde";
        }

        private async Task<string> GetFirstNChar()
        {
            var data = await GetData();
            var result = data.Substring(0, 3);
            return result;
        }

        private void temp()
        {
            int index1 = 0, index2 = 0, longestSubStr = 0;            
            string word = "abcabcdbb";

            var letters = new Dictionary<char, int>();

            for (int i = 0; i < word.Length; i++)
            {

                letters.Add(word[i], i);
            }            
        }

        //Uses Sliding window to get max but doesn't give you the index's of where they are
        private int LongestSubstringWithoutRepeatingCharacters3()
        {
            int i = 0, j = 0, max = 0;
            string word = "abcabcdbb";
            var letters = new HashSet<char>();

            while (j < word.Length)
            {
                if (!letters.Contains(word[j]))
                {
                    letters.Add(word[j]);
                    max = Math.Max(max, letters.Count);
                    j++;
                }
                else
                {
                    letters.Remove(word[i]);
                    i++;
                }
            }

            return max;
        }

        /* Determine if there are any two integers in the array whose sum is equal to the given value.
         * Given the root node of a binary tree, swap the 'left' and 'right' children for each node. 
         * Longest substring without repeating characters
         * 
         */
    }
}
