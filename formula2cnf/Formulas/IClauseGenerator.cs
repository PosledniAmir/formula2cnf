using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal interface IClauseGenerator
    {
        public int Variable { get; }
        public List<List<int>> Generate();
    }
}
