using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    internal sealed class DpllSat
    {
        private readonly CnfFormula _formula;
        private readonly Resolutor _resolutor;
        private readonly UnitGuard _unitGuard;
        private readonly ClauseChecker _clauseChecker;
        private readonly Stack<Tuple<int, int, HashSet<int>>> _stack;
        private int _locked;
        private int _decisions;
        private int _resolutions;

        public int Decisions => _decisions;
        public int Resolutions => _resolutions;

        public DpllSat(CnfFormula formula)
        {
            _stack = new Stack<Tuple<int, int, HashSet<int>>>();
            _locked = 1;
            _formula = formula;
            _resolutor = new Resolutor(_formula);
            _unitGuard = new UnitGuard(_formula);
            _clauseChecker = new ClauseChecker(_formula);
            _decisions = 0;
            _resolutions = 0;
        }

        public bool IsSatisfiable()
        {
            while(true)
            {
                if (Resolution(out var times) && Decide(times))
                {
                    if (_clauseChecker.Satisfied)
                    {
                        return true;
                    }
                }
                else if (!Backtrack())
                {
                    return false;
                }
            }
        }

        private bool Flip(bool riseLock)
        {
            var (_, times, set) = _stack.Pop();
            if (riseLock && set.Count == 1)
            {
                _locked++;
            }
            if (set.Count > 0)
            {
                var variable = set.First();
                set.Remove(variable);
                _stack.Push(Tuple.Create(variable, times, set));
                return true;
            }
            else
            {
                _unitGuard.Backtrack(times);
                _clauseChecker.Backtrack(times + 1);
                _resolutor.Backtrack(times);
                return true;
            }
        }

        private bool Backtrack()
        {
            while (_stack.Count > _locked)
            {
                if (Flip(false))
                {
                    return true;
                }
            }

            if (_stack.Count == _locked)
            {
                return Flip(true);
            }

            return false;
        }

        private bool Decide(int times)
        {
            if (_clauseChecker.Unsatisfied.Count == 0)
            {
                return true;
            }

            var clause = _clauseChecker.Unsatisfied.First();
            var set = new HashSet<int>();
            foreach (var item in _formula.Formula[clause])
            {
                if (!_clauseChecker.Model.Contains(item) && !_clauseChecker.Model.Contains(-item))
                {
                    set.Add(item);
                }
            }

            if (set.Count == 0)
            {
                return false;
            }

            var variable = set.First();
            set.Remove(variable);

            if (!_clauseChecker.Satisfy(variable))
            {
                throw new ArgumentException("This should not be possible.");
            }

            _decisions++;
            _stack.Push(Tuple.Create(variable, times, set));
            return true;
        }

        private bool Resolution(out int times)
        {
            times = 0;

            while (_unitGuard.Clauses.Count > 0)
            {
                var clause = _unitGuard.Clauses.First();
                if (!_resolutor.UnitResolute(clause))
                {
                    _unitGuard.Backtrack(times);
                    _clauseChecker.Backtrack(times);
                    _resolutor.Backtrack(times + 1);
                    return false;
                }

                var variable = _formula.Formula[clause].First();
                if (!_clauseChecker.Satisfy(variable))
                {
                    _unitGuard.Backtrack(times);
                    _clauseChecker.Backtrack(times + 1);
                    _resolutor.Backtrack(times + 1);
                    return false;
                }
                _resolutions++;
                _unitGuard.Add(clause, _resolutor.LastStep.Item2);
                times++;
            }

            return true;
        }

        public IEnumerable<int> GetModel()
        {
            return _clauseChecker.Model.OrderBy(x => Math.Abs(x));
        }
    }
}
