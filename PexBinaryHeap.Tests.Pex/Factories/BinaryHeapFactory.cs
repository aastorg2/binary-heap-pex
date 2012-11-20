using Microsoft.Pex.Framework;

namespace PexBinaryHeap.Tests.Pex.Factories
{
    public static partial class BinaryHeapFactory
    {
        [PexFactoryMethod(typeof(BinaryHeap<int, int>))]
        public static BinaryHeap<int, int> Create()
        {
            BinaryHeap<int, int> binaryHeap = new BinaryHeap<int, int>();
            return binaryHeap;
        }
    }
}
