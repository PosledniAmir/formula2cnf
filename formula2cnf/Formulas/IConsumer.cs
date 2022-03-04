using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal interface IConsumer
    {
        public IEnumerable<List<int>> Generate(Implication generator);
        public IEnumerable<List<int>> Generate(Equivalence generator);
    }
}
