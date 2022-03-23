using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    internal sealed class Resolutor
    {
        private readonly Stack<Tuple<int, IReadOnlyList<int>>> _stack;
        private readonly CnfFormula _formula;

        public Tuple<int, IReadOnlyList<int>>? LastStep => _stack.FirstOrDefault();

        public Resolutor(CnfFormula formula)
        {
            _formula = formula;
            _stack = new Stack<Tuple<int, IReadOnlyList<int>>>();
        }

        public bool UnitResolute(int clause)
        {
            if (_formula.Formula[clause].Count != 1)
            {
                throw new ArgumentException("Only clause with one literal can be used in UnitResolution.");
            }
            var literal = - _formula.Formula[clause].First();

            var resolution = new List<int>();
            bool failed = false;
            for(var i = 0; i < _formula.Formula.Count; i++)
            {
                var item = _formula.Formula[i];
                if (item.Contains(literal))
                {
                    resolution.Add(i);
                    item.Remove(literal);
                    if (item.Count == 0)
                    {
                        failed = true;
                        break;
                    }
                }
            }

            _stack.Push(new Tuple<int, IReadOnlyList<int>>(literal, resolution));
            return !failed;
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
            var step = _stack.Pop();

            foreach(var clause in step.Item2)
            {
                _formula.Formula[clause].Add(step.Item1);
            }
        }
    }
}
