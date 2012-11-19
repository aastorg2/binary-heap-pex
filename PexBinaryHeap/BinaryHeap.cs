using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PexBinaryHeap
{
    public class BinaryHeap<TPriority, TValue>
    {
        private readonly List<KeyValuePair<TPriority, TValue>> items = 
            new List<KeyValuePair<TPriority, TValue>>();

        private readonly Comparison<TPriority> compare; 

        public int Count
        {
            get { return items.Count; }
        }

        public BinaryHeap() : this(Comparer<TPriority>.Default.Compare)
        {
            Contract.Ensures(Count == 0);
        }

        public BinaryHeap(Comparison<TPriority> priorityComparison)
        {
            Contract.Requires(priorityComparison != null);
            Contract.Ensures(Count == 0);

            compare = priorityComparison;
        }

        public void Add(TPriority priority, TValue value)
        {
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);

            items.Add(new KeyValuePair<TPriority, TValue>(priority, value));
            BubbleUp(items.Count - 1);
        }

        public TValue GetValue()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Cannot extract element from empty heap.");
            }

            return items[0].Value;
        }

        public TValue Extract()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Cannot extract element from empty heap.");
            }

            var result = items[0].Value;
            Swap(0, Count - 1);
            items.RemoveAt(Count - 1);
            if (Count > 0)
            {
                BubbleDown(0);
            }

            return result;
        }

        public override string ToString()
        {
            return String.Join(", ", items.Select(it => it.Key));
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(HeapPropertyIsSatisfied());
            Contract.Invariant(items != null);
            Contract.Invariant(compare != null);
            Contract.Invariant(Count >= 0);
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
            Contract.Requires(0 <= index && index < Count);

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

        private void BubbleDown(int index)
        {
            Contract.Requires(0 <= index && index < Count);

            if (index == Count - 1)
            {
                return;
            }

            var leftChildIndex = index * 2 + 1;
            var rightChildIndex = index * 2 + 2;
            
            if (leftChildIndex < Count && 
                rightChildIndex < Count)  // both children are present
            {
                // pick lesser child and swap it with parent
                if (Less(leftChildIndex, rightChildIndex))
                {
                    if (Less(leftChildIndex, index))
                    {
                        Swap(leftChildIndex, index);
                        BubbleDown(leftChildIndex);
                    }
                }
                else
                {
                    if (Less(rightChildIndex, index))
                    {
                        Swap(rightChildIndex, index);
                        BubbleDown(rightChildIndex);
                    }
                }
            }
            else if (leftChildIndex < Count)    // only left child is present
            {
                if (Less(leftChildIndex, index))
                {
                    Swap(leftChildIndex, index);
                    BubbleDown(leftChildIndex);
                }
            }
        }

        private void Swap(int leftIndex, int rightIndex)
        {
            Contract.Requires(0 <= leftIndex && leftIndex <= Count);
            Contract.Requires(0 <= rightIndex && rightIndex <= Count);
            
            var temp = items[leftIndex];
            items[leftIndex] = items[rightIndex];
            items[rightIndex] = temp;
        }

        private bool Less(int leftIndex, int rightIndex)
        {
            Contract.Requires(0 <= leftIndex && leftIndex <= Count);
            Contract.Requires(0 <= rightIndex && rightIndex <= Count);

            return compare(
                items[leftIndex].Key,
                items[rightIndex].Key) <= 0;
        }
    }
}
