// <copyright file="BinaryHeapTPriorityTValueTests.cs" company="Eleks">Copyright © Eleks 2012</copyright>

using System;
using System.Collections.Generic;
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
            [PexAssumeNotNull] KeyValuePair<TPriority, TValue>[] valuesToAdd)
        {
            var heap = new BinaryHeap<TPriority, TValue>();
            var countBeforeAdding = heap.Count;
            foreach (var priorityAndValue in valuesToAdd)
            {
                heap.Add(priorityAndValue.Key, priorityAndValue.Value); // todo: check that count is incremented only by one
            }
            PexObserve.ValueForViewing("heap values", heap.ToString());

            Assert.AreEqual(countBeforeAdding + valuesToAdd.Length, heap.Count);
        }

        [PexMethod]
        public void Add_WhenNewMinValueIsAdded_ItWillBeRetrievedLater<TValue>(
            KeyValuePair<int, TValue>[] values)
        {
            PexAssume.IsNotNull(values);
            var heap = new BinaryHeap<int, TValue>(values);

            var newValue = PexChoose.Value<KeyValuePair<int, TValue>>("newValue");
            PexAssume.TrueForAll(values, value => newValue.Key < value.Key);
            PexObserve.ValueForViewing("newValue", newValue);

            heap.Add(newValue.Key, newValue.Value);

            var minValue = heap.GetFirst();
            Assert.That(minValue, Is.EqualTo(newValue.Value));
        }

        [PexMethod]
        public void GetFirst_WhenAddedSeveralElements_ReturnsMin(
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
            var count = heap.Count;

            heap.ExtractFirst(); // todo: call this multiple times

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
