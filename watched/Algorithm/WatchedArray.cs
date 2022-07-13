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
        private int _first;
        private int _second;

        public T First => GetValue(_first);
        public T Second => GetValue(_second);

        public IReadOnlyList<T> Array => _array;

        public WatchedArray(IEnumerable<T> items)
        {
            _array = items.ToArray();
            _first = 0;
            _second = 1;
        }

        private T GetValue(int at)
        {
            if (at >= _array.Length)
            {
                return default;
            }

            return _array[at];
        }

        public void TakeFirst(Func<T, bool> fn)
        {
            _first = GetUpdatedIndex(fn);
        }

        public void TakeSecond(Func<T, bool> fn)
        {
            _second = GetUpdatedIndex(fn);
        }

        private int GetUpdatedIndex(Func<T, bool> fn)
        {
            for (var i = 0; i < _array.Length; i++)
            {
                if ((i == _first) || (i == _second))
                {
                    continue;
                }

                if (fn(_array[i]))
                {
                    return i;
                }
            }

            return _array.Length;
        }
    }
}
