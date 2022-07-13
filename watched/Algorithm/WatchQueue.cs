using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    internal sealed class WatchQueue<T>
    {
        private readonly Stack<T> _queue;
        private T _first;
        private T _second;

        public T First => _first;
        public T Second => _second;

        public WatchQueue(IEnumerable<T> values)
        {
            _queue = new Stack<T>(values);
            _first = default;
            _second = default;
            TryFillFirst(out _);
            TryFillSecond(out _);
        }

        private bool TryFillFirst(out T last)
        {
            if (_queue.Count > 0)
            {
                last = _first;
                _first = _queue.Pop();
                return true;
            }
            last = default;
            return false;
        }

        private bool TryFillSecond(out T last)
        {
            if (_queue.Count > 0)
            {
                last = _second;
                _second = _queue.Pop();
                return true;
            }
            last = default;
            return false;
        }

        public void Enqueue(T value)
        {
            _queue.Push(value);
        }

        public T TakeFirst()
        {
            if (TryFillFirst(out var result))
            {
                return result;
            }
            else
            {
                _first = default;
                return default;
            }
        }

        public T TakeSecond()
        {
            if (TryFillSecond(out var result))
            {
                return result;
            }
            else
            {
                _second = default;
                return default;
            }
        }
    }
}
