// <copyright file="BinaryHeapTPriorityTValueTests.cs" company="Eleks">Copyright © Eleks 2012</copyright>

using System;
using System.Collections.Generic;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

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
                heap.ObjectInvariant();
            }
            Assert.AreEqual(countBeforeAdding + valuesToAdd.Length, heap.Count);
            PexObserve.ValueForViewing("heap values", heap.ToString());
        }
    }
}
