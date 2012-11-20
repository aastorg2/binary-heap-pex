// <copyright file="BinaryHeapTPriorityTValueTests.cs" company="Eleks">Copyright © Eleks 2012</copyright>

using System;
using System.Collections.Generic;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;
using System.Linq;

namespace PexBinaryHeap.Tests.Pex
{
    [PexClass(typeof(BinaryHeap<, >))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class BinaryHeapTests
    {
        [PexMethod]
        public void Ctor_WhenCalledWithValues_ResultingCountIsEqualToValuesCount<TPriority, TValue>(
            KeyValuePair<TPriority, TValue>[] values)
        {
            PexAssume.IsNotNull(values);
            var initialValuesCount = values.Count();

            var heap = new BinaryHeap<TPriority, TValue>(values);

            Assert.That(heap.Count, Is.EqualTo(initialValuesCount));
        }

        [PexMethod]
        public void Ctor_WhenCalledWithValues_MinValueBecomesFirst(
            KeyValuePair<int, int>[] values)
        {
            PexAssume.IsNotNullOrEmpty(values);
            PexAssume.TrueForAll(values, value => value.Key == value.Value);
            var minValue = values.Min(it => it.Value);

            var heap = new BinaryHeap<int, int>(values);

            var firstValue = heap.ExtractFirst();
            Assert.That(firstValue, Is.EqualTo(minValue));
        }

        [PexMethod]
        public void Add_SeveralValues_CountIsIncrementedByCountOfNewValues<TPriority, TValue>(
            [PexAssumeUnderTest] BinaryHeap<TPriority, TValue> heap,
            [PexAssumeNotNull] KeyValuePair<TPriority, TValue>[] valuesToAdd)
        {
            var initialCount = heap.Count;

            foreach (var priorityAndValue in valuesToAdd)
            {
                var previousCount = heap.Count;
                heap.Add(priorityAndValue.Key, priorityAndValue.Value);
                Assert.That(heap.Count, Is.EqualTo(previousCount + 1));
            }

            PexObserve.ValueForViewing("heap values", heap.ToString());
            Assert.That(heap.Count, Is.EqualTo(initialCount + valuesToAdd.Length));
        }

        [PexMethod]
        public void Add_WhenNewMinValueIsAdded_ItBecomesFirstValue<TValue>(
            KeyValuePair<int, TValue>[] existingValues,
            KeyValuePair<int, TValue> newMinValue)
        {
            PexAssume.IsNotNull(existingValues);
            var heap = new BinaryHeap<int, TValue>(existingValues);
            PexAssume.TrueForAll(existingValues, value => newMinValue.Key < value.Key);

            heap.Add(newMinValue.Key, newMinValue.Value);

            var minValue = heap.GetFirst();
            Assert.That(minValue, Is.EqualTo(newMinValue.Value));
        }

        [PexMethod]
        public void GetFirst_HeapIsNotEmpty_ReturnsMinValue(
            [PexAssumeNotNull] int[] values)
        {
            PexAssume.IsNotNullOrEmpty(values);
            var minValue = values.Min();
            var heap = new BinaryHeap<int, int>(values.Select(it => new KeyValuePair<int, int>(it, it)));

            var valueWithLeastPriority = heap.GetFirst();

            Assert.AreEqual(minValue, valueWithLeastPriority);
            PexObserve.ValueForViewing("heap values", heap.ToString());
            PexObserve.ValueForViewing("value with min priority", valueWithLeastPriority);
        }

        [PexMethod]
        public void GetFirst_WhenCalled_CountStaysTheSame<TPriority, TValue>(
            KeyValuePair<TPriority, TValue>[] values)
        {
            PexAssume.IsNotNullOrEmpty(values);
            var heap = new BinaryHeap<TPriority, TValue>(values);
            var count = heap.Count;

            heap.GetFirst();

            Assert.AreEqual(count, heap.Count);
        }

        [PexMethod]
        public void ExtractFirst_WhenHeapContainsSeveralElements_ResultIsMinValue(int[] values)
        {
            PexAssume.IsNotNullOrEmpty(values);
            var minValue = values.Min();
            var heap = new BinaryHeap<int, int>(values.Select(it => new KeyValuePair<int, int>(it, it)));

            var extractedValue = heap.ExtractFirst();

            Assert.AreEqual(minValue, extractedValue);
        }

        [PexMethod]
        public void ExtractFirst_WhenCalled_CountIsDecremented<TPriority, TValue>(
            KeyValuePair<TPriority, TValue>[] values)
        {
            PexAssume.IsNotNullOrEmpty(values);
            var heap = new BinaryHeap<TPriority, TValue>(values);

            for (int i = 0; i < values.Length; i++)
            {
                var previousCount = heap.Count;
                heap.ExtractFirst();
                Assert.That(heap.Count, Is.EqualTo(previousCount - 1));
            }
        }

        [Test]
        public void ExtractFirst_WhenHeapIsEmtpy_ExceptionIsThrown()
        {
            var heap = new BinaryHeap<int, int>();

            TestDelegate extractAction = () => heap.ExtractFirst();

            Assert.That(extractAction, Throws.InvalidOperationException);
        }

        [Test]
        public void GetFirst_WhenHeapIsEmtpy_ExceptionIsThrown()
        {
            var heap = new BinaryHeap<int, int>();

            TestDelegate getValueAction = () => heap.GetFirst();

            Assert.That(getValueAction, Throws.InvalidOperationException);
        }
    }
}
