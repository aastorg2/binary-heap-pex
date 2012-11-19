using System.Collections.Generic;
using NUnit.Framework;

namespace PexBinaryHeap.Tests.Pex
{
    [TestFixture]
    public class BinaryHeapUsageExamples
    {
        [Test]
        public void HeapSortWorks()
        {
            var items = new[] {6, 5, 4, 3, 2, 1};

            var result = HeapSort(items);

            Assert.That(result, Is.EqualTo(new[] {1, 2, 3, 4, 5, 6}));
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
                yield return heap.Extract();
            }
        } 
    }
}