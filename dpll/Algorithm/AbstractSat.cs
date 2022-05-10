using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public abstract class AbstractSat
    {
        protected List<Outcome> Failure(int conflict) => new List<Outcome> { new Outcome(0, -1, conflict, false) };
        protected readonly ClauseChecker _clauseChecker;
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
                return new Outcome(variable, clause, step.ConflictClause, step.Result);
            }
            else
            {
                _stack.Push(Tuple.Create(-1, 1, new DecisionSet()));
                _resolutions++;
                return new Outcome(variable, clause, -1, step.Result);
            }
        }

        private IEnumerable<Outcome> TryVariables(IEnumerable<int> variables, int clause)
        {
            int last = 0;
            foreach (var variable in variables)
            {
                var step = _clauseChecker.Satisfy(variable, clause);
                yield return new Outcome(variable, clause, step.ConflictClause, step.Result);
                if (!step.Result)
                {
                    yield break;
                }
                last = variable;
            }
        }

        private int Times(List<Outcome> outcomes)
        {
            if (Success(outcomes))
            {
                return outcomes.Count;
            }
            return outcomes.Count - 1;
        }

        protected bool Success(List<Outcome> outcomes)
        {
            return outcomes[^1].Success;
        }

        protected List<Outcome> Decision()
        {
            var (clause, set) = _clauseChecker.GetDecisionSet();

            if (set.Count == 0)
            {
                return Failure(clause);
            }

            var variables = set.Pop();
            var outcomes = TryVariables(variables, clause).ToList();
            var times = Times(outcomes);
            _decisions += times;
            _stack.Push(Tuple.Create(clause, times, set));
            return outcomes;
        }

        protected List<Outcome> Decide(int clause, DecisionSet set)
        {
            while (set.Count > 0)
            {
                var variables = set.Pop();
                var outcomes = TryVariables(variables, clause).ToList();
                var times = Times(outcomes);
                if (Success(outcomes))
                {
                    _decisions += times;
                    _stack.Push(Tuple.Create(clause, 1, set));
                    for (var i = 1; i < times; i++)
                    {
                        _stack.Push(Tuple.Create(clause, 1, new DecisionSet()));
                    }
                    return outcomes;
                }
                else
                {
                    _clauseChecker.Backtrack(times);
                }
            }
            return Failure(clause);
        }

        protected Tuple<int, DecisionSet> Backtrack()
        {
            var (clause, times, set) = _stack.Pop();
            _clauseChecker.Backtrack(times);
            return Tuple.Create(clause, set);
        }

        protected bool CanBacktrack()
        {
            return _stack.Count > 0;
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
