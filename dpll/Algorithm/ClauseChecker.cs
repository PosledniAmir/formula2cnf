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
        private readonly ClausePruner _pruner;
        private readonly Model _model;

        public IReadOnlySet<int> Unsatisfied => _model.Unsatisfied;
        public bool Satisfied => _model.Unsatisfied.Count == 0;
        public IReadOnlySet<int> Model => _model.Evaluation;

        public Tuple<int, DecisionSet> GetDecisionSet()
        {
            if (Unsatisfied.Count == 0)
            {
                return Tuple.Create(-1, new DecisionSet());
            }

            var clause = Unsatisfied.First();
            var set = new Stack<int>();
            foreach (var item in _pruner.Formula.Formula[clause])
            {
                if (!Model.Contains(item) && !Model.Contains(-item))
                {
                    set.Push(item);
                }
            }

            return Tuple.Create(clause, new DecisionSet(set));
        }

        public Tuple<int, int> GetFirstUnitVariable()
        {
            foreach(var clause in _pruner.Units.Where(c => Unsatisfied.Contains(c)))
            {
                return Tuple.Create(clause, _pruner.Formula.Formula[clause].First());
            }

            return Tuple.Create(-1, 0);
        }

        public ClauseChecker(CnfFormula formula)
        {
            _variableToClauses = GenerateMap(formula).ToDictionary(x => x.Key, x => x.Value);
            var clauses = formula.Clauses;
            _model = new Model(clauses);
            _pruner = new ClausePruner(formula);
        }

        public bool Satisfy(int variable, int clause)
        {
            if (!_model.Accepts(variable))
            {
                return false;
            }

            if (!_pruner.Prune(variable, Unsatisfied))
            {
                _pruner.Backtrack();
                return false;
            }

            var result = new List<int>();
            if (_variableToClauses.ContainsKey(variable))
            {
                foreach (var c in _variableToClauses[variable])
                {
                    result.Add(c);
                }
            }

            _model.Add(variable, result);
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
            _model.Backtrack();
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
