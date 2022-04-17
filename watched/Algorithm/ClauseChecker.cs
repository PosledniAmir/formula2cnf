using dpll.Algorithm;
using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    internal sealed class ClauseChecker : IClauseChecker
    {
        private readonly struct Step
        {
            public readonly int Variable;
            public readonly IReadOnlyList<int> AddedUnitClauses;
            public readonly IReadOnlyList<int> RemovedUnitClauses;
            public readonly IReadOnlyList<int> Satisfied;

            public Step(int variable, IReadOnlyList<int> added, IReadOnlyList<int> removed, IReadOnlyList<int> satisfied)
            {
                Variable = variable;
                AddedUnitClauses = added;
                RemovedUnitClauses = removed;
                Satisfied = satisfied;
            }
        }
        private readonly ClauseWatcher _watcher;
        private readonly HashSet<int> _model;
        private readonly HashSet<int> _unsatisfied;
        private readonly HashSet<int> _units;
        private readonly Stack<Step> _stack;

        public IReadOnlySet<int> Model => _model;

        public bool Satisfied => _unsatisfied.Count == 0;

        public ClauseChecker(CnfFormula formula)
        {
            _unsatisfied = new HashSet<int>();
            _units = new HashSet<int>();
            _model = new HashSet<int>();
            _watcher = new ClauseWatcher(formula.Formula, formula.Variables);

            for(int i = 0; i < _watcher.Formula.Count; i++)
            {
                var clause = _watcher.Formula[i];
                if (clause.Unit)
                {
                    _units.Add(i);
                }
                _unsatisfied.Add(i);
            }
            _stack = new Stack<Step>();
        }

        public bool Satisfy(int variable)
        {
            if (_model.Contains(-variable))
            {
                return false;
            }

            if (!_watcher.Prune(variable, _model, out var units))
            {
                _watcher.Backtrack();
                return false;
            }

            _model.Add(variable);
            var satisfied = new List<int>();
            var removed = new List<int>();
            foreach (var clause in _watcher.GetSatisfiedClauses(variable))
            {
                if (_unsatisfied.Contains(clause))
                {
                    _unsatisfied.Remove(clause);
                    satisfied.Add(clause);
                }

                if (_units.Contains(clause))
                {
                    _units.Remove(clause);
                    removed.Add(clause);
                }
            }

            var added = new List<int>();
            foreach (var unit in units)
            {
                if (_unsatisfied.Contains(unit))
                {
                    _units.Add(unit);
                    added.Add(unit);
                }
            }

            _stack.Push(new Step(variable, added, removed, satisfied));
            return true;
        }

        public int GetFirstUnitVariable()
        {
            foreach (var clause in _units)
            {
                var (first, second) = _watcher.Formula[clause].Exposed;
                if (first != 0)
                {
                    return first;
                }
                else if (second != 0)
                {
                    return second;
                }
                else
                {
                    throw new ArgumentException("Unit clause did not have any free literals.");
                }
            }

            return 0;
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
            _model.Remove(step.Variable);

            foreach (var clause in step.AddedUnitClauses)
            {
                _units.Remove(clause);
            }

            foreach (var clause in step.Satisfied)
            {
                _unsatisfied.Add(clause);
            }

            foreach (var clause in step.RemovedUnitClauses)
            {
                _units.Add(clause);
            }

            _watcher.Backtrack();
        }

        public HashSet<int> GetDecisionSet()
        {
            var clause = _unsatisfied.First();
            var set = new HashSet<int>();
            foreach (var item in _watcher.Formula[clause].Literals)
            {
                if (!_model.Contains(item) && !_model.Contains(-item))
                {
                    set.Add(item);
                }
            }

            return set;
        }
    }
}
