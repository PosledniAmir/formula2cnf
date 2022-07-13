using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    internal sealed class WatchedArray<T>
    {
        private readonly T[] _array;
        private int _index;

        public IReadOnlyList<T> Array => _array;

        public WatchedArray(IEnumerable<T> items)
        {
            _array = items.ToArray();
            _index = 0;
        }

        public T GetValue(int at)
        {
            var index = _index + at;

            if (index >= _array.Length)
            {
                return default;
            }

            return _array[index];
        }

        public T IncreaseIndex()
        {
            ++_index;
            return GetValue(0);
        }

        public T DecreaseIndex()
        {
            --_index;
            return GetValue(0);
        }
    }
}
