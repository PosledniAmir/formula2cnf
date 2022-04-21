using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    internal sealed class ClauseChecker : IClauseChecker
    {
        private readonly IReadOnlyDictionary<int, IReadOnlySet<int>> _variableToClauses;
        private readonly Stack<Tuple<int, IReadOnlyList<int>>> _stack;
        private readonly HashSet<int> _unsatisfied;
        private readonly HashSet<int> _model;
        private readonly ClausePruner _pruner;

        public IReadOnlySet<int> Unsatisfied => _unsatisfied;
        public bool Satisfied => _unsatisfied.Count == 0;
        public IReadOnlySet<int> Model => _model;

        public Tuple<int, HashSet<int>> GetDecisionSet()
        {
            if (_unsatisfied.Count == 0)
            {
                return Tuple.Create(-1, new HashSet<int>());
            }

            var clause = _unsatisfied.First();
            var set = new HashSet<int>();
            foreach (var item in _pruner.Formula.Formula[clause])
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
            foreach(var clause in _pruner.Units.Where(c => _unsatisfied.Contains(c)))
            {
                return Tuple.Create(clause, _pruner.Formula.Formula[clause].First());
            }

            return Tuple.Create(-1, 0);
        }

        public ClauseChecker(CnfFormula formula)
        {
            _variableToClauses = GenerateMap(formula).ToDictionary(x => x.Key, x => x.Value);
            _stack = new Stack<Tuple<int, IReadOnlyList<int>>>();
            _unsatisfied = new HashSet<int>();
            var clauses = formula.Clauses;
            for (var i = 0; i < clauses; i++)
            {
                _unsatisfied.Add(i);
            }
            _model = new HashSet<int>();
            _pruner = new ClausePruner(formula);
        }

        public bool Satisfy(int variable, int clause)
        {
            if (_model.Contains(-variable))
            {
                return false;
            }

            if (!_pruner.Prune(variable, _unsatisfied))
            {
                _pruner.Backtrack();
                return false;
            }

            _model.Add(variable);
            var result = new List<int>();
            foreach(var c in _variableToClauses[variable])
            {
                if (_unsatisfied.Contains(c))
                {
                    _unsatisfied.Remove(c);
                    result.Add(c);
                }
            }

            _stack.Push(new Tuple<int, IReadOnlyList<int>>(variable, result));
            return true;
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
