using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    internal sealed class WatchedPruner
    {
        private readonly WatchedFormula _formula;
        private readonly HashSet<int> _units;
        private readonly Stack<List<int>> _stack;

        public IReadOnlySet<int> Units => _units;
        public WatchedFormula Formula => _formula;

        public WatchedPruner(WatchedFormula formula)
        {
            _formula = formula;
            _stack = new Stack<List<int>>();
            _units = new HashSet<int>();
            for (var i = 0; i < _formula.Formula.Count; i++)
            {
                var clause = _formula.Formula[i];
                if (clause.Unit)
                {
                    _units.Add(i);
                }
            }
        }

        public bool Prune(int variable, IReadOnlySet<int> model, out List<int> satisfied)
        {
            var clauses = new List<int>();
            satisfied = new List<int>();
            var failed = false;
            var added = new List<int>();
            foreach (var clause in _formula.SetFalseOn(-variable, model))
            {
                clauses.Add(clause.ClauseId);
                if (clause.Unit)
                {
                    if (clause.Literals.Any(l => model.Contains(l)))
                    {
                        satisfied.Add(clause.ClauseId);
                    }
                    else
                    {
                        _units.Add(clause.ClauseId);
                        added.Add(clause.ClauseId);
                    }
                }
                else if (clause.Unsatisfiable)
                {
                    if (clause.Literals.Any(l => model.Contains(l)))
                    {
                        satisfied.Add(clause.ClauseId);
                    }
                    else if (clause.Literals.Any(l => l != -variable && !model.Contains(-l)))
                    {
                        throw new ArgumentException("This cannot happen.");
                    }
                    else
                    {
                        failed = true;
                        break;
                    }
                }
            }

            _stack.Push(added);
            return !failed;
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
            _formula.Backtrack();
            var toRemove = _stack.Pop();
            foreach(var item in toRemove)
            {
                _units.Remove(item);
            }
        }
    }
}
