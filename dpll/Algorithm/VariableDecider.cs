using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    internal sealed class VariableDecider
    {
        private HashSet<int> _variables;
        private Stack<int> _stack;

        public VariableDecider(int variables)
        {
            _variables = new HashSet<int>(Enumerable.Range(1, variables));
            _stack = new Stack<int>();
        }

        public int Decide()
        {
            if (_variables.Count == 0)
            {
                return 0;
            }
            else
            {
                var value = _variables.First();
                return - value;
            }
        }

        public void Update(int value)
        {
            value = Math.Abs(value);
            _variables.Remove(value);
            _stack.Push(value);
        }

        public void Backtrack()
        {
            var value = _stack.Pop();
            _variables.Add(value);
        }
    }
}
