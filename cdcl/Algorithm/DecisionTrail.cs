using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cdcl.Algorithm
{
    internal sealed class DecisionTrail
    {
        private readonly HashSet<int> _decided;
        private readonly Stack<Tuple<int, int>> _variableClauseStack;

        public int Count => _variableClauseStack.Count;

        public DecisionTrail(int variable, int clause)
        {
            _variableClauseStack = new Stack<Tuple<int, int>>();
            _variableClauseStack.Push(Tuple.Create(variable, clause));
            _decided = new HashSet<int>();
            _decided.Add(variable);
        }

        public void Implication(int variable, int clause)
        {
            _variableClauseStack.Push(Tuple.Create(variable, clause));
            _decided.Add(variable);
        }

        public Tuple<IEnumerable<int>, IEnumerable<int>> Responsible(IEnumerable<int> literals)
        {
            var responsible = new List<int>();
            var outsiders = new List<int>();

            foreach (var literal in literals)
            {
                if (_decided.Contains(-literal))
                {
                    responsible.Add(-literal);
                }
                else
                {
                    outsiders.Add(-literal);
                }
            }

            return Tuple.Create<IEnumerable<int>, IEnumerable<int>>(responsible, outsiders);
        }

        public Tuple<int, int> Pop()
        {
            return _variableClauseStack.Pop();
        }
    }
}
