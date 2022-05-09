using dpll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    internal sealed class WatchedPruner : IFormulaPruner
    {
        private readonly WatchedFormula _formula;
        public int Variables => _formula.Variables;
        public int Clauses => _formula.Clauses;

        public WatchedPruner(WatchedFormula formula)
        {
            _formula = formula;
        }

        public bool IsUnit(int clause)
        {
            return _formula.Formula[clause].Unit;
        }

        public bool IsEmpty(int clause)
        {
            return _formula.Formula[clause].Unsatisfiable;
        }

        public bool IsSatisfied(int clause, FormulaState state)
        {
            foreach (var variable in _formula.Formula[clause].Literals)
            {
                if (state.Model.Contains(variable))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<int> Literals(int clause)
        {
            return _formula.Formula[clause].Literals;
        }

        public SatisfyStep Satisfy(int variable, int clause, FormulaState state)
        {
            var satisfied = new List<int>();
            var failed = false;
            var toBeAdded = new List<int>();
            var conflict = -1;
            foreach (var discoveredClause in _formula.SetFalseOn(-variable, state.Model))
            {
                if (discoveredClause.Unit)
                {
                    if (IsSatisfied(discoveredClause.ClauseId, state))
                    {
                        satisfied.Add(discoveredClause.ClauseId);
                    }
                    else
                    {
                        toBeAdded.Add(discoveredClause.ClauseId);
                    }
                }
                else if (discoveredClause.Unsatisfiable)
                {
                    if (IsSatisfied(discoveredClause.ClauseId, state))
                    {
                        satisfied.Add(discoveredClause.ClauseId);
                    }
                    else
                    {
                        failed = true;
                        conflict = discoveredClause.ClauseId;
                        break;
                    }
                }
            }

            if (clause > -1)
            {
                satisfied.Add(clause);
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
            _formula.Backtrack();
        }

        public int AddClause(IEnumerable<int> literals)
        {
            throw new NotImplementedException();
        }
    }
}
