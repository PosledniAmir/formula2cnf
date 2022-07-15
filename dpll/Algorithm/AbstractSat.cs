using dpll.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public abstract class AbstractSat
    {
        protected readonly Outcome Failure =  new(0, -1, -1, false);
        protected readonly ClauseChecker _clauseChecker;
        protected readonly LockedStack _stack;
        private int _decisions;
        private int _resolutions;
        public int Decisions => _decisions;
        public int Resolutions => _resolutions;
        public bool Satisfied => _clauseChecker.Satisfied;

        protected IReadOnlySet<int> Model => _clauseChecker.Model;

        public abstract SatStats GetStats(TimeSpan elapsed);

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
                builder.Append(value);
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
                return new Outcome(variable, clause, step.ConflictClause, step.Result);
            }
            else
            {
                _stack.AddResolution(variable);
                _resolutions++;
                return new Outcome(variable, clause, -1, step.Result);
            }
        }

        protected bool Success(List<Outcome> outcomes)
        {
            return outcomes[^1].Success;
        }

        protected Outcome Decision()
        {
            var last = _stack.LastDecision();
            var variable = _clauseChecker.GetDecision(last);

            if (variable == 0)
            {
                return Failure;
            }

            var step = _clauseChecker.Satisfy(variable, -1);
            var result = new Outcome(variable, -1, step.ConflictClause, step.Result);
            if (step.Result)
            {
                ++_decisions;
                _stack.Decide(variable);
            }
            return result;
        }

        protected Tuple<int, bool> Backtrack()
        {
            var can = _stack.CanFlip();
            var (variable, resolutions) = _stack.Pop();
            _clauseChecker.Backtrack(resolutions.Count + 1);
            return Tuple.Create(variable, can);
        }

        protected bool CanBacktrack()
        {
            return _stack.CanPop();
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
