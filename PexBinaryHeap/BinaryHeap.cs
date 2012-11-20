using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PexBinaryHeap
{
    public sealed class BinaryHeap<TPriority, TValue>
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

        public TValue GetFirst()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Cannot get an element from empty heap.");
            }

            return items[0].Value;
        }

        public TValue ExtractFirst()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Cannot extract an element from empty heap.");
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
            for (int element = 0; element < Count; element++)
            {
                var leftChild = LeftChild(element);
                if (Exists(leftChild))
                {
                    Contract.Assert(IsLess(element, leftChild));
                }

                var rightChild = RightChild(element);
                if (Exists(rightChild))
                {
                    Contract.Assert(IsLess(element, rightChild));
                }
            }
            return true;
        }

        private bool Exists(int element)
        {
            return element < Count;
        }

        private void BubbleUp(int element)
        {
            Contract.Requires(0 <= element && element < Count);

            var current = element;
            while (current > 0 && IsLess(current, Parent(current)))
            {
                Swap(current, Parent(current));
                current = Parent(current);
            }
        }

        private int Parent(int element)
        {
            Contract.Requires(0 <= element && element < Count);

            return (element - 1)/2;
        }

        private void BubbleDown(int element)
        {
            Contract.Requires(0 <= element && element < Count);

            var lesserChild = GetMinElement(
                element, 
                LeftChild(element), 
                RightChild(element));
            if (lesserChild != element)
            {
                Swap(element, lesserChild);
                BubbleDown(lesserChild);
            }
        }

        private int RightChild(int element)
        {
            Contract.Requires(0 <= element && element < Count);

            return element * 2 + 2;
        }

        private int LeftChild(int element)
        {
            Contract.Requires(0 <= element && element < Count);

            return element * 2 + 1;
        }

        private int GetMinElement(int element, int leftChild, int rightChild)
        {
            Contract.Requires(0 <= element && element < Count);
            Contract.Requires(0 <= leftChild);
            Contract.Requires(0 <= rightChild);

            int result;
            if (leftChild < Count && IsLess(leftChild, element))
            {
                result = leftChild;
            }
            else
            {
                result = element;
            }
            if (rightChild < Count && IsLess(rightChild, result))
            {
                result = rightChild;
            }
            return result;
        }

        private void Swap(int left, int right)
        {
            Contract.Requires(0 <= left && left <= Count);
            Contract.Requires(0 <= right && right <= Count);
            
            var temp = items[left];
            items[left] = items[right];
            items[right] = temp;
        }

        private bool IsLess(int left, int right)
        {
            Contract.Requires(0 <= left && left <= Count);
            Contract.Requires(0 <= right && right <= Count);

            return compare(
                items[left].Key,
                items[right].Key) <= 0;
        }
    }
}
