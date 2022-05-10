using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public sealed class Model
    {
        private readonly Stack<Tuple<int, IReadOnlyList<int>>> _stack;
        private readonly HashSet<int> _evaluation;
        private readonly HashSet<int> _unsatisfied;

        public IReadOnlySet<int> Evaluation => _evaluation;
        public IReadOnlySet<int> Unsatisfied => _unsatisfied;

        public Model(int clausesCount)
        {
            _evaluation = new HashSet<int>();
            _unsatisfied = new HashSet<int>();
            for (int i = 0; i < clausesCount; i++)
            {
                _unsatisfied.Add(i);
            }
            _stack = new Stack<Tuple<int, IReadOnlyList<int>>>();
        }

        public bool Accepts(int variable)
        {
            if (!_evaluation.Contains(variable) && !_evaluation.Contains(-variable))
            {
                return true;
            }
            return false;
        }

        public void Add(int variable, IReadOnlyList<int> satisfiedClauses)
        {
            if (_evaluation.Contains(variable) || _evaluation.Contains(-variable))
            {
                throw new ArgumentException($"Cannot add {variable}, either it is already in {nameof(_evaluation)} or its negative is.");
            }

            _evaluation.Add(variable);
            var result = new List<int>();
            foreach (var clause in satisfiedClauses)
            {
                if (_unsatisfied.Contains(clause))
                {
                    _unsatisfied.Remove(clause);
                    result.Add(clause);
                }
            }

            _stack.Push(Tuple.Create<int, IReadOnlyList<int>>(variable, result));
        }

        public void Backtrack(int times)
        {
            for (var i = 0; i < times; i++)
            {
                Backtrack();
            }
        }

        internal void Reset(int clausesCount)
        {
            _evaluation.Clear();
            _stack.Clear();
            _unsatisfied.Clear();
            for (int i = 0; i < clausesCount; i++)
            {
                _unsatisfied.Add(i);
            }
        }

        public void Backtrack()
        {
            var (variable, clauses) = _stack.Pop();
            _evaluation.Remove(variable);

            foreach (var clause in clauses)
            {
                _unsatisfied.Add(clause);
            }
        }

        public void AddClause(int clause)
        {
            _unsatisfied.Add(clause);
        }
    }
}
