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
            _clauseChecker = new ClauseChecker(_formula);
            _decisions = 0;
            _resolutions = 0;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var comma = "";
            foreach (var value in GetModel())
            {
                builder.Append(comma);
                builder.Append(value.ToString());
                comma = ", ";
            }

            return builder.ToString();           
        }

        public bool IsSatisfiable()
        {
            while(true)
            {
                if (Resolution(out var times))
                {
                    if (_clauseChecker.Satisfied)
                    {
                        return true;
                    }

                    if (Decide(times))
                    {
                        if (_clauseChecker.Satisfied)
                        {
                            return true;
                        }
                        continue;
                    }
                }

                if (!Backtrack())
                {
                    return false;
                }
            }
        }

        private bool Flip(bool riseLock)
        {
            var (_, times, set) = _stack.Pop();
            _clauseChecker.Backtrack(times + 1);
            _resolutor.Backtrack(times);

            while (set.Count > 0)
            {
                var variable = set.First();
                set.Remove(variable);
                if (_clauseChecker.Satisfy(variable))
                {
                    if (riseLock && set.Count == 0)
                    {
                        _locked++;
                    }
                    _stack.Push(Tuple.Create(variable, 0, set));
                    return true;
                }
            }

            return false;
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
                var result = Flip(true);
                return result;
            }

            return false;
        }

        private bool Decide(int times)
        {
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
                _clauseChecker.Backtrack(times);
                _resolutor.Backtrack(times);
                return false;
            }

            var variable = set.First();
            set.Remove(variable);

            if (!_clauseChecker.Satisfy(variable))
            {
                _clauseChecker.Backtrack(times);
                _resolutor.Backtrack(times);
                return false;
            }

            _decisions++;
            _stack.Push(Tuple.Create(variable, times, set));
            return true;
        }

        private bool Resolution(out int times)
        {
            times = 0;

            var clause = _clauseChecker.GetFirstUnitClause();
            while (clause > -1)
            {
                if (!_resolutor.UnitResolute(clause))
                {
                    _clauseChecker.Backtrack(times);
                    _resolutor.Backtrack(times + 1);
                    return false;
                }

                var variable = _formula.Formula[clause].First();
                if (!_clauseChecker.Satisfy(variable))
                {
                    _clauseChecker.Backtrack(times);
                    _resolutor.Backtrack(times + 1);
                    return false;
                }
                _resolutions++;
                times++;
                clause = _clauseChecker.GetFirstUnitClause();
            }

            return true;
        }

        public IEnumerable<int> GetModel()
        {
            return _clauseChecker.Model.OrderBy(x => Math.Abs(x));
        }
    }
}
