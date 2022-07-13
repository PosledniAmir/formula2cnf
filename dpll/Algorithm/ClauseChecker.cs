using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public sealed class ClauseChecker
    {
        private readonly FormulaState _state;
        private readonly IFormulaPruner _formula;
        private readonly HashSet<int> _learned;

        public IReadOnlySet<int> Unsatisfied => _state.Unsatisfied;
        public bool Satisfied => IsSatisfied();
        public IReadOnlySet<int> Model => _state.Model;
        public int Learned => _learned.Count;

        private bool IsSatisfied()
        {
            foreach (var clause in Unsatisfied)
            {
                if (_formula.IsSatisfied(clause, _state))
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        public Tuple<int, DecisionSet> GetDecisionSet()
        {
            foreach (var clause in Unsatisfied.Where(c => _learned.Contains(c)))
            {
                if (!_formula.IsSatisfied(clause, _state))
                {
                    var stack = new Stack<int>(_formula.Literals(clause).Where(l => _state.Accepts(l)));
                    return Tuple.Create(clause, new DecisionSet(stack));
                }
            }

            foreach (var clause in Unsatisfied)
            {
                if (!_formula.IsSatisfied(clause, _state))
                {
                    var stack = new Stack<int>(_formula.Literals(clause).Where(l => _state.Accepts(l)));
                    return Tuple.Create(clause, new DecisionSet(stack));
                }
            }

            return Tuple.Create(-1, new DecisionSet());
        }

        public Tuple<int, int> GetFirstUnitVariable()
        {
            foreach(var clause in _state.Units.Where(c => _learned.Contains(c)).Where(c => Unsatisfied.Contains(c)))
            {
                if (!_formula.IsSatisfied(clause, _state))
                {
                    return Tuple.Create(clause, _formula.Literals(clause).First(l => _state.Accepts(l)));
                }
            }

            foreach (var clause in _state.Units.Where(c => Unsatisfied.Contains(c)))
            {
                if (!_formula.IsSatisfied(clause, _state))
                { 
                    return Tuple.Create(clause, _formula.Literals(clause).First(l => _state.Accepts(l)));
                }
            }

            return Tuple.Create(-1, 0);
        }

        public Dictionary<int, int> Reset(IEnumerable<int> enumerable)
        {
            var set = enumerable.ToHashSet();
            _learned.Clear();
            var map = _formula.Reset(set);
            _state.Reset(_formula.Clauses);
            foreach (var (_, v) in map)
            {
                _learned.Add(v);
            }
            return map;
        }

        public ClauseChecker(IFormulaPruner formula)
        {
            _state = new FormulaState(formula);
            _formula = formula;
            _learned = new HashSet<int>();
        }

        public SatisfyStep Satisfy(int variable, int clause)
        {
            var step = new SatisfyStep(clause);
            if (!_state.Accepts(variable))
            {
                return step;
            }

            step = _formula.Satisfy(variable, clause, _state);
            if (!step.Result)
            {
                _formula.Backtrack();
                return step;
            }

            _state.Update(variable, step);
            return step;
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
            _state.Backtrack();
        }

        public int AddClause(IEnumerable<int> clause)
        {
            var result = _formula.AddClause(clause);
            _state.CheckClause(result);
            _learned.Add(result);
            return result;
        }
    }
}
