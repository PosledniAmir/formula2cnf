using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_queens.Formulas
{
    internal sealed class TwoFormulas : Formula
    {
        public readonly Op Operator;
        public readonly Tuple<Formula, Formula> Formulas;

        public TwoFormulas(Op op, Formula left, Formula right) : this(op, Tuple.Create(left, right))
        {

        }

        public TwoFormulas(Op op, Tuple<Formula, Formula> formulas)
        {
            Operator = op;
            Formulas = formulas;
        }

        public override string ToString()
        {
            return $"({Operator.ToString().ToLower()} {Formulas.Item1} {Formulas.Item2})";
        }
    }
}
