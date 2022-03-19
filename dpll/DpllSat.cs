using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll
{
    internal sealed class DpllSat
    {
        private readonly CnfFormula _formula;
        private readonly Resolutor _resolutor;
        private readonly UnitGuard _unitGuard;
        private readonly Decider _decider;
        private readonly ClauseChecker _clauseChecker;

        public DpllSat(CnfFormula formula)
        {
            _formula = formula;
            _resolutor = new Resolutor(_formula);
            _unitGuard = new UnitGuard(_formula);
            _decider = new Decider(_formula.Variables);
            _clauseChecker = new ClauseChecker(_formula);
        }

        public bool IsSatisfiable()
        {
            FillGuard();
            var unsat = false;
            while (!unsat)
            {
                if (ForceResolutions())
                {
                    if (_clauseChecker.Satisfied)
                    {
                        return true;
                    }

                    var decided = _decider.TryDecide();
                    if (decided != 0)
                    {
                        _clauseChecker.Satisfy(decided);
                        if (_clauseChecker.Satisfied)
                        {
                            return true;
                        }
                    }
                }

                if (Backtrack())
                {
                    continue;
                }
                else
                {
                    unsat = true;
                }
            }

            return false;
        }

        private bool Backtrack()
        {
            var times = _decider.Backtrack();
            var result = false;
            for (var i = 0; i < times; i++)
            {
                _resolutor.Backtrack();
                _unitGuard.BackTrack();
                _clauseChecker.Backtrack();
                result = true;
            }

            return result;
        }

        private void FillGuard()
        {
            var unitClauses = _formula.Where(c => c.Count == 1).Select(c => c.First());
            foreach (var unitClause in unitClauses)
            {
                _unitGuard.Add(unitClause, new List<int>());
            }
        }

        private bool ForceResolutions()
        {
            while(_unitGuard.Clauses.Count > 0)
            {
                var clause = _unitGuard.Clauses.First();
                if (!_resolutor.UnitResolute(clause))
                {
                    _resolutor.Backtrack();
                    return false;
                }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                _unitGuard.Add(clause, _resolutor.LastStep.Item2);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                var variable = _formula.Formula[clause].First();
                if (_decider.TryDecide(variable))
                {
                    _resolutor.Backtrack();
                    _unitGuard.BackTrack();
                    return false;
                }

                _clauseChecker.Satisfy(variable);
            }

            return true;
        }
    }
}
