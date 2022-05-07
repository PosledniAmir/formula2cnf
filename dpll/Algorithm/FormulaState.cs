using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public sealed class FormulaState
    {
        private readonly Model _model;
        private readonly UnitSet _units;
        private readonly IFormula _formula;

        public IReadOnlySet<int> Unsatisfied => _model.Unsatisfied;
        public IReadOnlySet<int> Units => _units.Units;
        public IReadOnlySet<int> Model => _model.Evaluation;

        public FormulaState(IFormula formula)
        {
            _formula = formula;
            var units = new List<int>();
            for (var i = 0; i < _formula.Clauses; i++)
            {
                if (_formula.IsUnit(i))
                {
                    units.Add(i);
                }
            }
            _units = new UnitSet(units);
            _model = new Model(_formula.Clauses);
        }

        public bool IsSatisfied(int clause)
        {
            return _formula.IsSatisfied(clause, this);
        }

        public bool Accepts(int variable)
        {
            return _model.Accepts(variable);
        }

        public void Update(int variable, RemoveStep step)
        {
            if (!step.Result)
            {
                throw new ArgumentException($"Invalid step pushed");
            }
            _units.Add(step.Units);
            _model.Add(variable, step.Satisfied);
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
            _units.Backtrack();
            _model.Backtrack();
        }
    }
}
