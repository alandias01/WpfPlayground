using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPlayground.InterviewPractice.Algorithms
{
    public class AlgoProblems
    {
        public AlgoProblems()
        {
            this.LongestSubstringWithoutRepeatingCharacters2();
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
            bool found = false;

            Dictionary<int, int> numbers = new Dictionary<int, int>();
            for (int i = 0; i < arr.Length; i++)
            {
                int complement = target - arr[i];
                if (numbers.TryGetValue(complement, out index2))
                {
                    index1 = i;
                    found = true;
                    break;
                }
                numbers[arr[i]] = i;
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
    }
}
