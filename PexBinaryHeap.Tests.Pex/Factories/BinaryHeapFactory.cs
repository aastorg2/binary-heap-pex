using System;
using Microsoft.Pex.Framework;
using PexBinaryHeap;

namespace PexBinaryHeap
{
    /// <summary>A factory for PexBinaryHeap.BinaryHeap`2[System.Int32,System.Int32] instances</summary>
    public static partial class BinaryHeapFactory
    {
        /// <summary>A factory for PexBinaryHeap.BinaryHeap`2[System.Int32,System.Int32] instances</summary>
        [PexFactoryMethod(typeof(BinaryHeap<int, int>))]
        public static BinaryHeap<int, int> Create()
        {
            BinaryHeap<int, int> binaryHeap = new BinaryHeap<int, int>();
            return binaryHeap;
        }
    }
}
