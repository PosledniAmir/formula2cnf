using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal sealed class First : IClauseGenerator
    {
        private readonly int _variable;

        public First(int variable)
        {
            _variable = variable;
        }

        public int Variable => _variable;

        public IEnumerable<IReadOnlyList<int>> Generate()
        {
            yield return new List<int> { _variable };
        }
    }
}
