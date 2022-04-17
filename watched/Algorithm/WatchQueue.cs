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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public WatchQueue(IEnumerable<T> values)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _queue = new Stack<T>(values);
#pragma warning disable CS8601 // Possible null reference assignment.
            _first = default;
            _second = default;
#pragma warning restore CS8601 // Possible null reference assignment.
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
#pragma warning disable CS8601 // Possible null reference assignment.
            last = default;
#pragma warning restore CS8601 // Possible null reference assignment.
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
#pragma warning disable CS8601 // Possible null reference assignment.
            last = default;
#pragma warning restore CS8601 // Possible null reference assignment.
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
#pragma warning disable CS8603 // Possible null reference return.
                _first = default;
                return default;
#pragma warning restore CS8603 // Possible null reference return.
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
#pragma warning disable CS8603 // Possible null reference return.
                _second = default;
                return default;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
    }
}
