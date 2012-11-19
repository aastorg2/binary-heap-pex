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
        public void Add_SeveralValues_CountIsIncrementedByCountOfNewValues<TPriority, TValue>(
            [PexAssumeUnderTest] BinaryHeap<TPriority, TValue> heap,
            [PexAssumeNotNull] KeyValuePair<TPriority, TValue>[] valuesToAdd)
        {
            var countBeforeAdding = heap.Count;
            foreach (var priorityAndValue in valuesToAdd)
            {
                heap.Add(priorityAndValue.Key, priorityAndValue.Value);
            }
            PexObserve.ValueForViewing("heap values", heap.ToString());

            Assert.AreEqual(countBeforeAdding + valuesToAdd.Length, heap.Count);
        }

        [PexMethod]
        public void GetValue_WhenAddedSeveralElements_ReturnsMin(
            [PexAssumeNotNull] int[] values)
        {
            PexAssume.IsNotNullOrEmpty(values);
            var minValue = values.Min();
            var heap = new BinaryHeap<int, int>();
            foreach (var value in values)
            {
                heap.Add(value, value);
            }

            var valueWithLeastPriority = heap.GetValue();

            Assert.AreEqual(minValue, valueWithLeastPriority);
            PexObserve.ValueForViewing("value with min priority", valueWithLeastPriority);
        }
    }
}
