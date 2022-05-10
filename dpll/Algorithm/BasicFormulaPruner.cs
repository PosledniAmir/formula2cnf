using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    internal sealed class BasicFormulaPruner : IFormulaPruner
    {
        private readonly Stack<Tuple<int, IReadOnlyList<int>>> _stack;
        private readonly CnfFormula _formula;
        private readonly List<IReadOnlyList<int>> _original;
        private readonly Dictionary<int, HashSet<int>> _variableToClauses;

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
            return _original[clause];
        }

        public BasicFormulaPruner(CnfFormula formula)
        {
            _original = new List<IReadOnlyList<int>>();
            _variableToClauses = GenerateMap(formula).ToDictionary(x => x.Key, x => x.Value);
            _stack = new Stack<Tuple<int, IReadOnlyList<int>>>();
            _formula = formula;
            for (var i = 0; i < _formula.Formula.Count; i++)
            {
                var clause = _formula.Formula[i];
                _original.Add(new List<int>(clause));
            }
        }

        public SatisfyStep Satisfy(int variable, int clause, FormulaState state)
        {
            var clauses = new List<int>();
            var toBeAdded = new List<int>();
            var failed = false;
            var conflict = -1;
            foreach (var item in state.Unsatisfied)
            {
                var dicsoveredClause = _formula.Formula[item];
                if (dicsoveredClause.Contains(-variable))
                {
                    clauses.Add(item);
                    dicsoveredClause.Remove(-variable);
                    if (dicsoveredClause.Count == 1)
                    {
                        toBeAdded.Add(item);
                    }
                    else if (dicsoveredClause.Count == 0)
                    {
                        failed = true;
                        conflict = item;
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

            if (!failed)
            {
                return new SatisfyStep(toBeAdded, satisfied);
            }
            else
            {
                return new SatisfyStep(conflict);
            }
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

        private static IEnumerable<KeyValuePair<int, HashSet<int>>> GenerateMap(CnfFormula formula)
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
                yield return new KeyValuePair<int, HashSet<int>>(pair.Key, pair.Value);
            }
        }

        public int AddClause(IEnumerable<int> clause)
        {
            var list = clause.ToList();
            _original.Add(list);
            var id = _formula.Clauses;
            _formula.AddClause(list);
            foreach (var item in clause)
            {
                if (_variableToClauses.TryGetValue(item, out var value))
                {
                    value.Add(id);
                }
                else
                {
                    _variableToClauses[item] = new HashSet<int> { id };
                }
            }
            return id;
        }

        public void Reset(HashSet<int> set)
        {
            var original = new List<IReadOnlyList<int>>(_original);
            _stack.Clear();
            _original.Clear();
            _formula.Clear();
            _variableToClauses.Clear();

            for (var i = 0; i < original.Count; i++)
            {
                if (!set.Contains(i))
                {
                    var item = original[i];
                    AddClause(item);
                }
            }
        }
    }
}
