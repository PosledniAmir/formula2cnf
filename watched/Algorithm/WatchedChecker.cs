using dpll.Algorithm;
using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    internal sealed class WatchedChecker : IClauseChecker
    {
        private readonly Stack<Tuple<int, IReadOnlyList<int>>> _stack;
        private readonly HashSet<int> _unsatisfied;
        private readonly HashSet<int> _model;
        private readonly WatchedPruner _pruner;

        public IReadOnlySet<int> Unsatisfied => _unsatisfied;
        public bool Satisfied => _unsatisfied.Count == 0;
        public IReadOnlySet<int> Model => _model;

        public Tuple<int, HashSet<int>> GetDecisionSet()
        {
            if (_unsatisfied.Count == 0)
            {
                var range = Enumerable
                    .Range(1, _pruner.Formula.Variables)
                    .Where(v => !_model.Contains(v) && !_model.Contains(-v))
                    .SelectMany(v => new[] {v, -v});
                return Tuple.Create(-1, new HashSet<int>(range));
            }

            var clause = _unsatisfied.First();
            var set = new HashSet<int>();
            foreach (var item in _pruner.Formula.Formula[clause].Literals)
            {
                if (!_model.Contains(item) && !_model.Contains(-item))
                {
                    set.Add(item);
                }
            }

            return Tuple.Create(clause, set);
        }

        public Tuple<int, int> GetFirstUnitVariable()
        {
            foreach (var clause in _pruner.Units.Where(c => _unsatisfied.Contains(c)))
            {
                var (first, second) = _pruner.Formula.Formula[clause].Exposed;
                if ((first != 0) && (second != 0))
                {
                    throw new ArgumentException("Non-unit clause in unit clauses error");
                }
                else if (first != 0)
                {
                    return Tuple.Create(clause, first);
                }
                else if (second != 0)
                {
                    return Tuple.Create(clause, second);
                }
                else
                {
                    throw new ArgumentException("Non-unit clause in unit clauses error");
                }
            }

            return Tuple.Create(-1, 0);
        }

        public WatchedChecker(WatchedFormula formula)
        {
            _stack = new Stack<Tuple<int, IReadOnlyList<int>>>();
            _unsatisfied = new HashSet<int>();
            var clauses = formula.Clauses;
            for (var i = 0; i < clauses; i++)
            {
                _unsatisfied.Add(i);
            }
            _model = new HashSet<int>();
            _pruner = new WatchedPruner(formula);
        }

        public bool Satisfy(int variable, int clause)
        {
            if (_model.Contains(-variable))
            {
                return false;
            }

            if (!_pruner.Prune(variable, _model, out var satisfied))
            {
                _pruner.Backtrack();
                return false;
            }

            _model.Add(variable);
            var result = new List<int>();
            if (clause > -1)
            {
                _unsatisfied.Remove(clause);
                result.Add(clause);
            }
            result.AddRange(SatisfiedFilter(variable, satisfied));

            _stack.Push(new Tuple<int, IReadOnlyList<int>>(variable, result));
            return true;
        }

        private IEnumerable<int> SatisfiedFilter(int variable, List<int> satisfied)
        {
            foreach (var clause in satisfied)
            {
                if (_unsatisfied.Contains(clause))
                {
                    _unsatisfied.Remove(clause);
                    yield return clause;
                }
            }

            foreach (var clause in _pruner.Formula.GetExposedOn(variable))
            {
                if (_unsatisfied.Contains(clause))
                {
                    _unsatisfied.Remove(clause);
                    yield return clause;
                }
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
            var (variable, result) = _stack.Pop();
            _model.Remove(variable);
            foreach (var clause in result)
            {
                _unsatisfied.Add(clause);
            }
            _pruner.Backtrack();
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
