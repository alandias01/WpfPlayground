using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfPlayground.DataStructuresAlgorithms
{
    public class FilterListByMultipleItems
    {
        public FilterListByMultipleItems()
        {
            FilterListByMultipleItemsMethod();
        }

        private void FilterListByMultipleItemsMethod()
        {
            /* Result has full list
             * User enters multiple items in arr
             * Create new List<CtpyAcct> thats filtered by items in arr
             */

            List<CtpyAcc> result = new List<CtpyAcc> {
                new CtpyAcc { ctpy = "aa", acct = 1 },
                new CtpyAcc { ctpy = "bb", acct = 2 },
                new CtpyAcc { ctpy = "cc", acct = 3 },
                new CtpyAcc { ctpy = "dd", acct = 4 },
                new CtpyAcc { ctpy = "ee", acct = 5 }
            };

            string[] arr = { "bb", "dd" };

            var filteredList = result.Where(x => arr.Any(y => y == x.ctpy));
        }
    }

    public class CtpyAcc
    {
        public string ctpy;
        public int acct;
    }
}
