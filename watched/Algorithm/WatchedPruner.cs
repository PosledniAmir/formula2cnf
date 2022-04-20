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

        public IReadOnlySet<int> Units => _units;
        public WatchedFormula Formula => _formula;

        public WatchedPruner(WatchedFormula formula)
        {
            _formula = formula;
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
            foreach (var clause in _formula.SetFalseOn(-variable, model))
            {
                clauses.Add(clause.ClauseId);
                if (clause.Unit)
                {
                    _units.Add(variable);
                }
                else if (clause.Unsatisfiable)
                {
                    if (clause.Literals.Any(l => model.Contains(l)))
                    {
                        satisfied.Add(variable);
                    }
                    else
                    {
                        failed = true;
                        break;
                    }
                }
            }

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
        }
    }
}
