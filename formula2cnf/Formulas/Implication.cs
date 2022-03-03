using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal class Implication : IClause
    {
        private int _variable;
        private IClause _clause;

        public Implication(int variable, IClause clause)
        {
            _variable = variable;
            _clause = clause;
        }
    }
}
