using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal class Implication : IClauseGenerator
    {
        private readonly int _variable;
        private readonly IConsumer _clause;

        public int Variable => _variable;

        public Implication(int variable, IConsumer clause)
        {
            _variable = variable;
            _clause = clause;
        }

        public IEnumerable<List<int>> Generate()
        {
            return _clause.Generate(this);
        }
    }
}
