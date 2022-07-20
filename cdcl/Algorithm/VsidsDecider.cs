using dpll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cdcl.Algorithm
{
    internal sealed class VsidsDecider : IVariableDecider
    {
        public const double Decay = 1 / 0.95;
        public const long StartValue = 100;
        private long _bump;
        private readonly SortedDictionary<long, HashSet<int>> _sorted;
        private readonly long[] _handlesToValues;
        private readonly Stack<int> _stack;

        public VsidsDecider(int variables)
        {
            _stack = new Stack<int>();
            _sorted = new SortedDictionary<long, HashSet<int>>();
            _handlesToValues = new long[variables];
            _sorted[StartValue] = new HashSet<int>(Enumerable.Range(1, variables));
            for (var i = 0; i < variables; i++)
            {
                _handlesToValues[i] = StartValue;
            }
            _bump = StartValue;
        }

        public int Decide()
        {
            if (_sorted.Count == 0)
            {
                return 0;
            }

            var (_, valueSet) = _sorted.Last();
            var value = valueSet.First();
            valueSet.Remove(value);
            return value;
        }

        public void Update(int variable)
        {
            variable = Math.Abs(variable);
            var counter = _handlesToValues[variable - 1];
            var set = _sorted[counter];
            set.Remove(variable);

            if (set.Count == 0)
            {
                _sorted.Remove(counter);
            }

            _stack.Push(variable);
        }

        public void Backtrack()
        {
            var popped = _stack.Pop();
            var counter = _handlesToValues[popped - 1];
            if (!_sorted.TryGetValue(counter, out var set))
            {
                set = new HashSet<int>();
                _sorted[counter] = set;
            }
            set.Add(popped);
        }

        public void Reset()
        {
            _stack.Clear();
            _sorted.Clear();
            _sorted[StartValue] = new HashSet<int>(Enumerable.Range(1, _handlesToValues.Length));
            for (var i = 0; i < _handlesToValues.Length; i++)
            {
                _handlesToValues[i] = StartValue;
            }
            _bump = StartValue;
        }

        private void Rescale()
        {
            var minimum = _handlesToValues.Min();

            for (var i = 0; i < _handlesToValues.Length; i++)
            {
                var current = _handlesToValues[i];
                _handlesToValues[i] = (current / minimum) * StartValue;
            }

            var sortedValues = _sorted
                .Select(pair => Tuple.Create((pair.Key / minimum) * StartValue, pair.Value))
                .ToList();

            _sorted.Clear();

            foreach(var (key, value) in sortedValues)
            {
                _sorted[key] = value;
            }
        }

        public void Learn(IEnumerable<int> clause)
        {
            var test = (long)(_bump * Decay);

            if (test < _bump)
            {
                _bump = (long)(StartValue * Decay);
                Rescale();
            }
            else
            {
                _bump = test;
            }

            foreach (var variable in clause)
            {
                var abs = Math.Abs(variable);
                var counter = _handlesToValues[abs - 1];
                if (_sorted.TryGetValue(counter, out var set))
                {
                    if (set.Contains(abs))
                    {
                        set.Remove(abs);
                    }
                }

                counter = counter + _bump;
                _handlesToValues[abs - 1] = counter;
                if (!_sorted.TryGetValue(counter, out set))
                {
                    set = new HashSet<int>();
                    _sorted[counter] = set;
                }
                set.Add(abs);
            }
        }
    }
}
