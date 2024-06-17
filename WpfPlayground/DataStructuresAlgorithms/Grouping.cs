using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlayground.DataStructuresAlgorithms
{

    /* If <T> is say an object with props Cusip, qty, price
     * Grouping would take a key to group by, like cusip, the other fields would be aggregations
     * Those values would be placed into Item<T>'s children
     
     */

    
    public class Grouping
    {
        public Grouping()
        {
            CustomGroupby();
        }

        private void CustomGroupby()
        {
            var Items = new Item<Stock>();
            Items.Children.Add(new Item<Stock>() { Value = new Stock("AAPL", 10, 100) });
            Items.Children.Add(new Item<Stock>() { Value = new Stock("AAPL", 10, 200) });
            Items.Children.Add(new Item<Stock>() { Value = new Stock("AAPL", 20, 300) });
            Items.Children.Add(new Item<Stock>() { Value = new Stock("AAPL", 20, 400) });
            Items.Children.Add(new Item<Stock>() { Value = new Stock("BAC", 30, 1000) });
            Items.Children.Add(new Item<Stock>() { Value = new Stock("BAC", 30, 2000) });
            Items.Children.Add(new Item<Stock>() { Value = new Stock("BAC", 40, 3000) });
            Items.Children.Add(new Item<Stock>() { Value = new Stock("BAC", 40, 4000) });
            
        }

        private void LinqGroupby()
        {
            var StockList = new List<Stock>();
            StockList.Add(new Stock("AAPL", 10, 100));
            StockList.Add(new Stock("AAPL", 20, 200));
            StockList.Add(new Stock("AAPL", 20, 300));
            StockList.Add(new Stock("BAC", 30, 400));
            StockList.Add(new Stock("BAC", 30, 500));
            StockList.Add(new Stock("BAC", 40, 600));
            var s = StockList.GroupBy((x) => x.Cusip);
            foreach (var item in s)
            {
                foreach (var subItem in item)
                {

                }
            }            
        }

        private void CustomExtensionGroupby()
        {
            var StockList = new List<Stock>();
            StockList.Add(new Stock("AAPL", 10, 100));
            StockList.Add(new Stock("AAPL", 20, 200));
            StockList.Add(new Stock("AAPL", 20, 300));
            StockList.Add(new Stock("BAC", 30, 400));
            StockList.Add(new Stock("BAC", 30, 500));
            StockList.Add(new Stock("BAC", 40, 600));
        }
    }

    public static class GroupbyExtensions
    {
        public static void CustomGroupby<T>(this IEnumerable<T> source)
        {

        }
    }

    public class Item<T>
    {
        public T Value { get; set; }
        
        public List<Item<T>> Children { get; set; } = new List<Item<T>>();
        public Item()
        {   
        }

    }

    public class Stock
    {
        public Stock(string cusip, int quantity, int price) => (Cusip, Quantity, Price) = (cusip, quantity, price);
        
        public string Cusip { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}


//public void GroupByp<U>(Func<T, U> keySelector)
//{
//    var groups = new Dictionary<U, List<Item<T>>>();

//    // Iterate through children
//    foreach (var child in Children)
//    {
//        // Get the key for the current child
//        var key = keySelector(child.Value);

//        // Check if the group for this key already exists, otherwise create it
//        if (!groups.ContainsKey(key))
//        {
//            groups[key] = new List<Item<T>>();
//        }

//        // Add the current child to the corresponding group
//        groups[key].Add(child);
//    }
//}