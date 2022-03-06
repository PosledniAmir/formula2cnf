using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal interface IConsumer
    {
        public IEnumerable<IReadOnlyList<int>> Generate(Implication generator);
        public IEnumerable<IReadOnlyList<int>> Generate(Equivalence generator);
    }
}
