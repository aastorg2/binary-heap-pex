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
            items[0] = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            if (items.Count > 0)
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
                var leftIndex = LeftChildIndex(i);
                if (leftIndex < Count)
                {
                    Contract.Assert(Less(i, leftIndex));
                }

                var rightIndex = RightChildIndex(i);
                if (rightIndex < Count)
                {
                    Contract.Assert(Less(i, rightIndex));
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

            var lesserChildIndex = GetIndexOfMinValue(
                index, 
                LeftChildIndex(index), 
                RightChildIndex(index));
            if (lesserChildIndex != index)
            {
                Swap(index, lesserChildIndex);
                BubbleDown(lesserChildIndex);
            }
        }

        private int RightChildIndex(int index)
        {
            Contract.Requires(0 <= index && index < Count);

            return index * 2 + 2;
        }

        private int LeftChildIndex(int index)
        {
            Contract.Requires(0 <= index && index < Count);

            return index * 2 + 1;
        }

        private int GetIndexOfMinValue(int index, int leftChildIndex, int rightChildIndex)
        {
            Contract.Requires(0 <= index && index < Count);
            Contract.Requires(0 <= leftChildIndex);
            Contract.Requires(0 <= rightChildIndex);

            int minIndex;
            if (leftChildIndex < Count && Less(leftChildIndex, index))
            {
                minIndex = leftChildIndex;
            }
            else
            {
                minIndex = index;
            }
            if (rightChildIndex < Count && Less(rightChildIndex, minIndex))
            {
                minIndex = rightChildIndex;
            }
            return minIndex;
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
