using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    internal sealed class ClauseChecker : IClauseChecker
    {
        private readonly FormulaState _state;
        private readonly IFormula _formula;

        public IReadOnlySet<int> Unsatisfied => _state.Unsatisfied;
        public bool Satisfied => Unsatisfied.Count == 0;
        public IReadOnlySet<int> Model => _state.Model;

        public Tuple<int, DecisionSet> GetDecisionSet()
        {
            var result = Tuple.Create(-1, new DecisionSet());
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
            foreach(var clause in _state.Units.Where(c => Unsatisfied.Contains(c)))
            {
                if (!_formula.IsSatisfied(clause, _state))
                {
                    return Tuple.Create(clause, _formula.Literals(clause).First(l => _state.Accepts(l)));
                }
            }

            return Tuple.Create(-1, 0);
        }

        public ClauseChecker(IFormula formula)
        {
            _state = new FormulaState(formula);
            _formula = formula;
        }

        public bool Satisfy(int variable, int clause)
        {
            if (!_state.Accepts(variable))
            {
                return false;
            }

            var step = _formula.Satisfy(variable, _state);
            if (!step.Result)
            {
                _formula.Backtrack();
                return false;
            }

            _state.Update(variable, step);
            return true;
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
    }
}
