using dpll.Algorithm;
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
        private readonly UnitSet _units;

        public IReadOnlySet<int> Units => _units.Units;
        public WatchedFormula Formula => _formula;

        public WatchedPruner(WatchedFormula formula)
        {
            _formula = formula;
            var units = new List<int>();
            for (var i = 0; i < _formula.Formula.Count; i++)
            {
                var clause = _formula.Formula[i];
                if (clause.Unit)
                {
                    units.Add(i);
                }
            }
            _units = new UnitSet(units);
        }

        public bool Prune(int variable, IReadOnlySet<int> model, out List<int> satisfied)
        {
            var clauses = new List<int>();
            satisfied = new List<int>();
            var failed = false;
            var toBeAdded = new List<int>();
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
                        toBeAdded.Add(clause.ClauseId);
                    }
                }
                else if (clause.Unsatisfiable)
                {
                    if (clause.Literals.Any(l => model.Contains(l)))
                    {
                        satisfied.Add(clause.ClauseId);
                    }
                    else
                    {
                        failed = true;
                        break;
                    }
                }
            }

            _units.Add(toBeAdded);
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
            _units.Backtrack();
        }
    }
}
