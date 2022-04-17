using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public sealed class DpllSat
    {
        private readonly IClauseChecker _clauseChecker;
        private readonly Stack<Tuple<int, int, HashSet<int>>> _stack;
        private int _locked;
        private int _decisions;
        private int _resolutions;

        public int Decisions => _decisions;
        public int Resolutions => _resolutions;

        public DpllSat(IClauseChecker checker)
        {
            _stack = new Stack<Tuple<int, int, HashSet<int>>>();
            _locked = 1;
            _clauseChecker = checker;
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
            _clauseChecker.Backtrack(times);

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
                    _stack.Push(Tuple.Create(variable, 1, set));
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
            var set = _clauseChecker.GetDecisionSet();

            if (set.Count == 0)
            {
                _clauseChecker.Backtrack(times);
                return false;
            }

            var variable = set.First();
            set.Remove(variable);

            if (!_clauseChecker.Satisfy(variable))
            {
                _clauseChecker.Backtrack(times);
                return false;
            }

            _decisions++;
            _stack.Push(Tuple.Create(variable, times + 1, set));
            return true;
        }

        private bool Resolution(out int times)
        {
            times = 0;

            var variable = _clauseChecker.GetFirstUnitVariable();
            while (variable != 0)
            {
                if (!_clauseChecker.Satisfy(variable))
                {
                    _clauseChecker.Backtrack(times);
                    return false;
                }
                _resolutions++;
                times++;
                variable = _clauseChecker.GetFirstUnitVariable();
            }

            return true;
        }

        public IEnumerable<int> GetModel()
        {
            return _clauseChecker.Model.OrderBy(x => Math.Abs(x));
        }
    }
}
