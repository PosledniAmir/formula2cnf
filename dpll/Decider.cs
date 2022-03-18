using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll
{
    internal class Decider
    {
        private enum Decision { Decided, Forced}
        private readonly Stack<Tuple<int, Decision>> _stack;
        private readonly HashSet<int> _undecided;

        public Decider(int variables)
        {
            _undecided = new HashSet<int>();
            for(var i = 1; i <= variables; i++)
            {
                _undecided.Add(i);
            }

            _stack = new Stack<Tuple<int, Decision>>();
        }

        public bool TryDecide()
        {
            if (_undecided.Count > 0)
            {
                var decision = _undecided.First();
                _undecided.Remove(decision);
                _stack.Push(Tuple.Create(decision, Decision.Decided));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryDecide(int variable)
        {
            if (_undecided.Contains(-variable))
            {
                variable = -variable;
            }
            else if (!_undecided.Contains(variable))
            {
                return false;
            }

            _undecided.Remove(variable);
            _stack.Push(Tuple.Create(variable, Decision.Forced));
            return true;
        }

        public void Backtrack()
        {
            var (variable, decision) = _stack.Pop();
            if ((decision == Decision.Decided) && (variable > 0))
            {
                _undecided.Add(- variable);
            } else if (decision == Decision.Forced)
            {
                _undecided.Add(variable);
            }
        }
    }
}
