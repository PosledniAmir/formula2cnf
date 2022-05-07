using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    internal sealed class BasicFormula : IFormula
    {
        private readonly Stack<Tuple<int, IReadOnlyList<int>>> _stack;
        private readonly CnfFormula _formula;
        private readonly IReadOnlyDictionary<int, IReadOnlySet<int>> _variableToClauses;

        public int Variables => _formula.Variables;
        public int Clauses => _formula.Clauses;

        public bool IsUnit(int clause)
        {
            return _formula.Formula[clause].Count == 1;
        }

        public bool IsEmpty(int clause)
        {
            return _formula.Formula.Count == 0;
        }

        public bool IsSatisfied(int clause, FormulaState state)
        {
            return !state.Unsatisfied.Contains(clause);
        }

        public IEnumerable<int> Literals(int clause)
        {
            return _formula.Formula[clause];
        }

        public BasicFormula(CnfFormula formula)
        {
            _variableToClauses = GenerateMap(formula).ToDictionary(x => x.Key, x => x.Value);
            _stack = new Stack<Tuple<int, IReadOnlyList<int>>>();
            _formula = formula;
            var units = new List<int>();
            for (var i = 0; i < _formula.Formula.Count; i++)
            {
                var clause = _formula.Formula[i];
                if (clause.Count == 1)
                {
                    units.Add(i);
                }
            }
        }

        public RemoveStep Satisfy(int variable, FormulaState state)
        {
            var clauses = new List<int>();
            var toBeAdded = new List<int>();
            var failed = false;
            foreach (var item in state.Unsatisfied)
            {
                var clause = _formula.Formula[item];
                if (clause.Contains(-variable))
                {
                    clauses.Add(item);
                    clause.Remove(-variable);
                    if (clause.Count == 1)
                    {
                        toBeAdded.Add(item);
                    }
                    else if (clause.Count == 0)
                    {
                        failed = true;
                        break;
                    }
                }
            }

            _stack.Push(new Tuple<int, IReadOnlyList<int>>(-variable, clauses));

            var satisfied = new List<int>();
            if (_variableToClauses.ContainsKey(variable))
            {
                foreach (var item in _variableToClauses[variable])
                {
                    satisfied.Add(item);
                }
            }

            return new RemoveStep(!failed, toBeAdded, satisfied);
        }

        public void Backtrack(int times)
        {
            for (var i = 0; i < times; i++)
            {
                Backtrack();
            }
        }

        public void Backtrack()
        {
            var step = _stack.Pop();

            foreach (var clause in step.Item2)
            {
                var set = _formula.Formula[clause];
                set.Add(step.Item1);
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
