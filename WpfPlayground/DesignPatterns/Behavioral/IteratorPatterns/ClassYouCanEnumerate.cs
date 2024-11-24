using System.Collections;
using System.Collections.Generic;

namespace WpfPlayground.DesignPatterns.Behavioral.IteratorPatterns
{
    public class ClassYouCanEnumerate<T> : IEnumerable<T>
    {
        private IEnumerable<T> ie;

        public ClassYouCanEnumerate(IEnumerable<T> enumerable)
        {
            ie = enumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in ie)
            {
                yield return item;
            }

            //ie has an enumerator you can return
            //return ie.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
