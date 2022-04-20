using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    internal sealed class ClausePruner
    {
        private readonly WatchedFormula _formula;
        private readonly HashSet<int> _units;

        public IReadOnlySet<int> Units => _units;
        public WatchedFormula Formula => _formula;

        public ClausePruner(WatchedFormula formula)
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

        public bool Prune(int variable, IReadOnlySet<int> model)
        {
            var clauses = new List<int>();
            var failed = false;
            foreach (var clause in _formula.SetFalseOn(-variable, model))
            {
                clauses.Add(clause.ClauseId);
                clause.SetFalse(-variable, model);
                if (clause.Unit)
                {
                    _units.Add(variable);
                }
                else if (clause.Unsatisfiable)
                {
                    failed = true;
                    break;
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
