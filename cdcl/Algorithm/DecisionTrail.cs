using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cdcl.Algorithm
{
    internal sealed class DecisionTrail
    {
        private readonly Stack<Tuple<int, int>> _variableClauseStack;

        public int Count => _variableClauseStack.Count;

        public DecisionTrail(int variable, int clause)
        {
            _variableClauseStack = new Stack<Tuple<int, int>>();
            _variableClauseStack.Push(Tuple.Create(variable, clause));
        }

        public void Implication(int variable, int clause)
        {
            _variableClauseStack.Push(Tuple.Create(variable, clause));
        }

        public Tuple<int, int> Pop()
        {
            return _variableClauseStack.Pop();
        }
    }
}
