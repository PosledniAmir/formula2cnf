﻿using formula2cnf.Formulas;
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
        private readonly LockedStack _stack;
        private int _decisions;
        private int _resolutions;

        public int Decisions => _decisions;
        public int Resolutions => _resolutions;

        public DpllSat(IClauseChecker checker)
        {
            _stack = new LockedStack();
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

        public IEnumerable<IReadOnlyList<int>> GetModels()
        {
            var cont = true;
            while (cont)
            {
                if (Resolution())
                {
                    if (Decide())
                    {
                        continue;
                    }
                    else if (_clauseChecker.Satisfied)
                    {
                        yield return GetModel().ToList();
                    }
                }

                if (!Backtrack())
                {
                    cont = false;
                }
            }
        }

        public bool IsSatisfiable()
        {
            foreach (var item in GetModels())
            {
                return true;
            }

            return false;
        }

        private bool Flip()
        {
            var (clause, times, set) = _stack.Pop();
            _clauseChecker.Backtrack(times);

            while (set.Count > 0)
            {
                var variable = set.First();
                set.Remove(variable);
                if (_clauseChecker.Satisfy(variable, clause))
                {
                    _stack.Push(Tuple.Create(clause, 1, set));
                    return true;
                }
            }

            return false;
        }

        private bool Backtrack()
        {
            while (_stack.Count > 0)
            {
                if (Flip())
                {
                    return true;
                }
            }

            return false;
        }

        private bool Decide()
        {
            var (clause, set) = _clauseChecker.GetDecisionSet();

            if (set.Count == 0)
            {
                return false;
            }

            var variable = set.First();
            set.Remove(variable);

            if (!_clauseChecker.Satisfy(variable, clause))
            {
                return false;
            }

            _decisions++;
            _stack.Push(Tuple.Create(clause, 1, set));
            return true;
        }

        private bool Resolution()
        {
            var times = 0;

            var (clause, variable) = _clauseChecker.GetFirstUnitVariable();
            while (variable != 0)
            {
                if (!_clauseChecker.Satisfy(variable, clause))
                {
                    _clauseChecker.Backtrack(times);
                    return false;
                }
                _resolutions++;
                times++;
                (clause, variable) = _clauseChecker.GetFirstUnitVariable();
            }

            if (times > 0)
            {
                _stack.Push(Tuple.Create(-1, times, new HashSet<int>()));
            }
            return true;
        }

        private IEnumerable<int> GetModel()
        {
            return _clauseChecker.Model.OrderBy(x => Math.Abs(x));
        }
    }
}
