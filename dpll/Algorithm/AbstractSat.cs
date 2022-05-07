using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public abstract class AbstractSat
    {
        private readonly ClauseChecker _clauseChecker;
        private readonly LockedStack _stack;
        private int _decisions;
        private int _resolutions;
        public int Decisions => _decisions;
        public int Resolutions => _resolutions;
        public bool Satisfied => _clauseChecker.Satisfied;

        protected IReadOnlySet<int> Model => _clauseChecker.Model;

        protected AbstractSat(IFormulaPruner formula)
        {
            _stack = new LockedStack();
            _clauseChecker = new ClauseChecker(formula);
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

        protected IEnumerable<int> GetModel()
        {
            return _clauseChecker.Model.OrderBy(x => Math.Abs(x));
        }

        protected Tuple<int, int> GetFirstUnitClause()
        {
            return _clauseChecker.GetFirstUnitVariable();
        }

        protected Outcome Resolution(int clause, int variable)
        {
            var step = _clauseChecker.Satisfy(variable, clause);
            if (!step.Result)
            {
                return new Outcome(false, variable, clause);
            }
            else
            {
                _stack.Push(Tuple.Create(-1, 1, new DecisionSet()));
                _resolutions++;
                return new Outcome(true, variable, clause);
            }
        }

        protected Outcome TryVariables(IEnumerable<int> variables, int clause, out int times)
        {
            times = 0;
            int last = 0;
            foreach (var variable in variables)
            {
                var step = _clauseChecker.Satisfy(variable, clause);
                if (!step.Result)
                {
                    return new Outcome(false, variable, clause);
                }
                times++;
                last = variable;
            }
            return new Outcome(true, last, clause);
        }

        protected Outcome Decision()
        {
            var (clause, set) = _clauseChecker.GetDecisionSet();

            if (set.Count == 0)
            {
                return new Outcome(false, 0, clause);
            }

            var variables = set.Pop();
            var outcome = TryVariables(variables, clause, out var times);
            _decisions += times;
            _stack.Push(Tuple.Create(clause, times, set));
            return outcome;
        }

        private bool Flip()
        {
            var (clause, times, set) = _stack.Pop();
            _clauseChecker.Backtrack(times);

            while (set.Count > 0)
            {
                var variables = set.Pop();
                var outcome = TryVariables(variables, clause, out times);
                if (outcome.Success)
                {
                    _decisions += times;
                    _stack.Push(Tuple.Create(clause, times, set));
                    return true;
                }
                else
                {
                    _clauseChecker.Backtrack(times);
                }
            }

            return false;
        }

        protected bool BacktrackAndChoose()
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

        public abstract IEnumerable<IReadOnlyList<int>> GetModels();

        public bool IsSatisfiable()
        {
            foreach (var _ in GetModels())
            {
                return true;
            }

            return false;
        }
    }
}
