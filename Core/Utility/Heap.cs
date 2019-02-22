namespace TofuCore.Utility.Heap
{
    public class Heap<T> where T : IHeapable<T>
    {
        private T[] _contents;
        private int _itemCount;

        public Heap(int maxHeapSize)
        {
            _contents = new T[maxHeapSize];
        }

        public void Add(T item)
        {
            item.HeapIndex = _itemCount;
            _contents[_itemCount] = item;
            SortUp(item);
            _itemCount++;
        }

        public T RemoveFirst()
        {
            T top = _contents[0];
            _itemCount--;
            _contents[0] = _contents[_itemCount];
            _contents[0].HeapIndex = 0;
            SortDown(_contents[0]);
            return top;
        }

        public bool Contains(T item)
        {
            return Equals(_contents[item.HeapIndex], item);
        }

        public int Count => _itemCount;

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = _contents[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }
            }
        }

        void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < _itemCount)
                {
                    swapIndex = childIndexLeft;
                    if (childIndexRight < _itemCount)
                    {
                        if (_contents[childIndexLeft].CompareTo(_contents[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(_contents[swapIndex]) < 0)
                    {
                        Swap(item, _contents[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        void Swap(T itemA, T itemB)
        {
            _contents[itemA.HeapIndex] = itemB;
            _contents[itemB.HeapIndex] = itemA;
            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }
}