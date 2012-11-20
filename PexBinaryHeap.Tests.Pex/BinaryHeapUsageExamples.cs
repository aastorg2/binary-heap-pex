using System.Collections.Generic;
using Microsoft.Pex.Framework;
using NUnit.Framework;
using System.Linq;

namespace PexBinaryHeap.Tests.Pex
{
    [TestFixture]
    [PexClass(typeof(BinaryHeap<,>))]
    public partial class BinaryHeapUsageExamples
    {
        [Test]
        public void HeapSortWorks()
        {
            var items = new[] {6, 5, 4, 3, 2, 1};

            var result = HeapSort(items);

            Assert.That(result, Is.EqualTo(new[] {1, 2, 3, 4, 5, 6}));
        }
        
        [PexMethod]
        public void HeapSortWorks(int[] values)
        {
            PexAssume.IsNotNull(values);

            var result = HeapSort(values).ToArray();

            PexObserve.ValueForViewing("sorted", result);
            Assert.That(result, Is.Ordered);
        }

        [Test]
        public void RunningMedianWorksOnSortedSequence()
        {
            var items = new[] { 1, 2, 3, 4, 5 };

            var result = RunningMedian(items);

            Assert.That(result, Is.EqualTo(new[] { 1, 1, 2, 2, 3 }));
        }

        [Test]
        public void RunningMedianWorksOnSortedDescendingSequence()
        {
            var items = new[] { 5, 4, 3, 2, 1 };

            var result = RunningMedian(items);

            Assert.That(result, Is.EqualTo(new[] { 5, 4, 4, 3, 3 }));
        }

        private IEnumerable<T> HeapSort<T>(IEnumerable<T> items)
        {
            var heap = new BinaryHeap<T, T>();
            foreach (var item in items)
            {
                heap.Add(item, item);
            }

            while (heap.Count != 0)
            {
                yield return heap.ExtractFirst();
            }
        }

        private IEnumerable<int> RunningMedian(IEnumerable<int> items)
        {
            var lowerHalf = new BinaryHeap<int, int>((x, y) => 
                -Comparer<int>.Default.Compare(x, y)); // first element is max
            var higherHalf = new BinaryHeap<int, int>();  // first element is min
            foreach (var n in items)
            {
                if (lowerHalf.Count == 0 || n <= lowerHalf.GetFirst())
                {
                    lowerHalf.Add(n, n);
                }
                else
                {
                    higherHalf.Add(n, n);
                }

                while (lowerHalf.Count > higherHalf.Count + 1 &&
                    lowerHalf.Count > 1)
                {
                    var maxLowerHalfValue = lowerHalf.ExtractFirst();
                    higherHalf.Add(maxLowerHalfValue, maxLowerHalfValue);
                }
                while (higherHalf.Count > lowerHalf.Count)
                {
                    var minHigherHalfValue = higherHalf.ExtractFirst();
                    lowerHalf.Add(minHigherHalfValue, minHigherHalfValue);
                }
                yield return lowerHalf.GetFirst();
            }
        } 
    }
}