using dpll.Algorithm;
using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Runner
{
    public interface ISatFactory
    {
        public AbstractSat Create(CnfFormula cnf);
    }
}
