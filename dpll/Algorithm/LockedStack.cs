using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public sealed class LockedStack
    {
        private int _level;
        private readonly Stack<Tuple<int, List<int>, bool>> _stack;

        public LockedStack()
        {
            _level = 1;
            _stack = new Stack<Tuple<int, List<int>, bool>>();
            _stack.Push(Tuple.Create(0, new List<int>(), true));
        }

        public void Reset()
        {
            _level = 1;
            _stack.Clear();
            _stack.Push(Tuple.Create(0, new List<int>(), true));
        }

        public bool CanFlip()
        {
            return !_stack.Peek().Item3;
        }

        public int Flip(int variable)
        {
            if (_level == _stack.Count)
            {
                _level++;
            }
            _stack.Push(Tuple.Create(-variable, new List<int>(), true));
            return -variable;
        }

        public void AddResolution(int variable)
        {
            var (_, list, _) = _stack.Peek();
            list.Add(variable);
        }

        public int LastDecision()
        {
            return _stack.Peek().Item1;
        }

        public void Decide(int variable)
        {
            _stack.Push(Tuple.Create(variable, new List<int>(), false));
        }

        public Tuple<int, List<int>> Pop()
        {
            if (_level == _stack.Count)
            {
                throw new ArgumentException("Cannot pop.");
            }
            var (v, l, _) = _stack.Pop();
            return Tuple.Create(v, l);
        }

        public bool CanPop()
        {
            return _level != _stack.Count;
        }
    }
}
