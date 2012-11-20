// <copyright file="BinaryHeapTPriorityTValueTests.cs" company="Eleks">Copyright © Eleks 2012</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;
using System.Linq;
using FluentAssertions;

namespace PexBinaryHeap.Tests.Pex
{
    [PexClass(typeof(BinaryHeap<, >))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class BinaryHeapTests
    {
        [PexMethod]
        public void Add_SeveralValues_CountIsIncrementedByCountOfNewValues<TPriority, TValue>(
            [PexAssumeUnderTest] BinaryHeap<TPriority, TValue> heap,
            [PexAssumeNotNull] Tuple<TPriority, TValue>[] valuesToAdd)
        {
            PexAssume.AreElementsNotNull(valuesToAdd);
            var countBeforeAdding = heap.Count;
            foreach (var priorityAndValue in valuesToAdd)
            {
                heap.Add(priorityAndValue.Item1, priorityAndValue.Item2);
            }
            PexObserve.ValueForViewing("heap values", heap.ToString());

            Assert.AreEqual(countBeforeAdding + valuesToAdd.Length, heap.Count);
        }

        [PexMethod]
        public void GetFirst_WhenAddedSeveralElements_ReturnsMin(
            [PexAssumeNotNull] int[] values)
        {
            PexAssume.IsNotNullOrEmpty(values);
            var minValue = values.Min();
            var heap = new BinaryHeap<int, int>();
            foreach (var value in values)
            {
                heap.Add(value, value);
            }

            var valueWithLeastPriority = heap.GetFirst();

            Assert.AreEqual(minValue, valueWithLeastPriority);
            PexObserve.ValueForViewing("heap values", heap.ToString());
            PexObserve.ValueForViewing("value with min priority", valueWithLeastPriority);
        }

        [PexMethod]
        public void GetFirst_WhenCalled_CountStaysTheSame<TPriority, TValue>(
            Tuple<TPriority, TValue>[] values)
        {
            PexAssume.IsNotNullOrEmpty(values);
            PexAssume.AreElementsNotNull(values);
            var heap = new BinaryHeap<TPriority, TValue>();
            foreach (var value in values)
            {
                heap.Add(value.Item1, value.Item2);
            }
            var count = heap.Count;

            heap.GetFirst();

            Assert.AreEqual(count, heap.Count);
        }

        [PexMethod]
        public void ExtractFirst_WhenHeapContainsSeveralElements_ResultIsMinValue(int[] values)
        {
            PexAssume.IsNotNullOrEmpty(values);
            var minValue = values.Min();
            var heap = new BinaryHeap<int, int>();
            foreach (var value in values)
            {
                heap.Add(value, value);
            }

            var extractedValue = heap.ExtractFirst();

            Assert.AreEqual(minValue, extractedValue);
        }

        [PexMethod]
        public void ExtractFirst_WhenCalled_CountIsDecremented<TPriority, TValue>(
            Tuple<TPriority, TValue>[] values)
        {
            PexAssume.IsNotNullOrEmpty(values);
            PexAssume.AreElementsNotNull(values);
            var heap = new BinaryHeap<TPriority, TValue>();
            foreach (var value in values)
            {
                heap.Add(value.Item1, value.Item2);
            }
            var count = heap.Count;

            heap.ExtractFirst();

            Assert.AreEqual(count - 1, heap.Count);
        }

        [Test]
        public void ExtractFirst_WhenHeapIsEmtpy_ExceptionIsThrown()
        {
            var heap = new BinaryHeap<int, int>();

            Action extractAction = () => heap.ExtractFirst();

            extractAction.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void GetFirst_WhenHeapIsEmtpy_ExceptionIsThrown()
        {
            var heap = new BinaryHeap<int, int>();

            Action getValueAction = () => heap.GetFirst();

            getValueAction.ShouldThrow<InvalidOperationException>();
        }
    }
}
