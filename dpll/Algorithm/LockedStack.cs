﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public sealed class LockedStack
    {
        private readonly Stack<Tuple<int, int, DecisionSet>> _stack;
        private int _locked;
        public int Count => _stack.Count - _locked;

        public LockedStack()
        {
            _locked = 0;
            _stack = new Stack<Tuple<int, int, DecisionSet>>();
        }
        
        public void Push(Tuple<int, int, DecisionSet> item)
        {
            if (_stack.Count == _locked && item.Item1 != -1 && item.Item3.Count == 0)
            {
                _locked++;
            }

            if (item.Item1 == -1 && _stack.Count > 0)
            {
                var (clause, times, set) = _stack.Pop();
                item = Tuple.Create(clause, ++times, set);
            }

            _stack.Push(item);
        }

        public Tuple<int, int, DecisionSet> Pop()
        {
            return _stack.Pop();
        }

        public void Reset()
        {
            _stack.Clear();
        }
    }
}
