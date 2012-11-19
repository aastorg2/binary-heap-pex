using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PexBinaryHeap
{
    public class BinaryHeap<TPriority, TValue>
    {
        private readonly List<Tuple<TPriority, TValue>> items = 
            new List<Tuple<TPriority, TValue>>();

        private readonly Comparer<TPriority> comparer = Comparer<TPriority>.Default; 

        public int Count
        {
            get { return items.Count; }
        }

        public void Add(TPriority priority, TValue value)
        {
            items.Add(Tuple.Create(priority, value));
            BubbleUp(items.Count - 1);
        }

        public TValue GetValue()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Cannot extract element from empty heap.");
            }
            return items[0].Item2;
        }

        public override string ToString()
        {
            return String.Join(", ", items.Select(it => it.Item1));
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(HeapPropertyIsSatisfied());
        }

        [Pure]
        private bool HeapPropertyIsSatisfied()
        {
            for (int i = 0; i < Count; i++)
            {
                var leftIndex = 2 * i + 1;
                if (leftIndex < Count && !Less(i, leftIndex))
                {
                    return false;
                }

                var rightIndex = 2 * i + 2;
                if (rightIndex < Count && !Less(i, rightIndex))
                {
                    return false;
                }
            }
            return true;
        }

        private void BubbleUp(int index)
        {
            if (index <= 0)
            {
                return;
            }
            var parentIndex = (index - 1)/2;
            if (Less(index, parentIndex))
            {
                Swap(index, parentIndex);
                BubbleUp(parentIndex);
            }
        }

        private void Swap(int i, int j)
        {
            var temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }

        private bool Less(int firstIndex, int secondIndex)
        {
            return comparer.Compare(
                items[firstIndex].Item1,
                items[secondIndex].Item1) <= 0;
        }
    }
}
