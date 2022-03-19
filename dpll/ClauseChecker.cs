using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll
{
    internal sealed class ClauseChecker
    {
        private readonly IReadOnlyDictionary<int, IReadOnlySet<int>> _variableToClauses;
        private readonly Stack<IReadOnlyList<int>> _stack;
        private readonly HashSet<int> _satisfied;
        private readonly int _clauses;

        public bool Satisfied => _satisfied.Count == _clauses;

        public ClauseChecker(CnfFormula formula)
        {
            _variableToClauses = GenerateMap(formula).ToDictionary(x => x.Key, x => x.Value);
            _stack = new Stack<IReadOnlyList<int>>();
            _satisfied = new HashSet<int>();
            _clauses = formula.Clauses;
        }

        public void Satisfy(int variable)
        {
            var result = new List<int>();
            foreach(var clause in _variableToClauses[variable])
            {
                if (!_satisfied.Contains(clause))
                {
                    _satisfied.Add(clause);
                    result.Add(clause);
                }
            }

            _stack.Push(result);
        }

        public void Backtrack()
        {
            var result = _stack.Pop();
            foreach (var clause in result)
            {
                _satisfied.Remove(clause);
            }
        }

        private static IEnumerable<KeyValuePair<int, IReadOnlySet<int>>> GenerateMap(CnfFormula formula)
        {
            var result = new Dictionary<int, HashSet<int>>();
            for (var i = 0; i < formula.Formula.Count; i++)
            {
                foreach (var variable in formula.Formula[i])
                {
                    if (result.ContainsKey(variable))
                    {
                        result[variable].Add(i);
                    }
                    else
                    {
                        result.Add(variable, new HashSet<int> { i });
                    }
                }
            }

            foreach (var pair in result)
            {
                yield return new KeyValuePair<int, IReadOnlySet<int>>(pair.Key, pair.Value);
            }
        }
    }
}
