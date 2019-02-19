

using System;

namespace TofuCore.Utility.Heap
{
    public interface IHeapable<T> : IComparable<T>
    {
        int HeapIndex { get; set; }

    }

}
