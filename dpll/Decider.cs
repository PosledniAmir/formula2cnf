using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll
{
    internal class Decider
    {
        private readonly Stack<Tuple<int, List<int>>> _decisions;
        private readonly HashSet<int> _undecided;
        private int _locked;

        public Decider(int variables)
        {
            _locked = 1;
            _decisions = new Stack<Tuple<int, List<int>>>();
            _undecided = new HashSet<int>();
            for (var i = 1; i <= variables; i++)
            {
                _undecided.Add(i);
            }
        }

        public int TryDecide()
        {
            if (_undecided.Count > 0)
            {
                var item = _undecided.First();
                _undecided.Remove(item);
                _decisions.Push(Tuple.Create(item, new List<int>()));
                return item;
            }
            else
            {
                return 0;
            }
        }

        public bool TryDecide(int variable)
        {
            var item = Math.Abs(variable);
            if (_undecided.Contains(-variable))
            {
                _undecided.Remove(item);
            }
            else if (_undecided.Contains(variable))
            {
                _undecided.Remove(item);
            }
            else
            {
                return false;
            }

            _decisions.Peek().Item2.Add(variable);
            return true;
        }

        private bool BacktrackAboveLocked(out int result)
        {
            result = 0;
            while (_decisions.Count > _locked)
            {
                var (variable, forced) = _decisions.Pop();
                if (variable > 0)
                {
                    _undecided.Add(-variable);
                    result += forced.Count + 1;
                    return true;
                }
                else if (variable < 0)
                {
                    _undecided.Add(-variable);
                    result += forced.Count + 1;
                }
                else
                {
                    throw new ArgumentException($"Invalid variable found {variable}");
                }
            }
            return false;
        }

        public int Backtrack()
        {
            if (BacktrackAboveLocked(out var result))
            {
                return result;
            }

            var (variable, forced) = _decisions.Peek();

            if (variable > 0)
            {
                _undecided.Add(-variable);
                _decisions.Pop();
                _locked++;
                return result + forced.Count + 1;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable<int> GetModel()
        {
            foreach(var (variable, forced) in _decisions)
            {
                yield return variable;
                foreach (var item in forced)
                {
                    yield return item;
                }
            }
        }
    }
}
