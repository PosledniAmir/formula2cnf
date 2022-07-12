using dpll.Algorithm;
using dpll.Runner;
using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watched.Algorithm;

namespace watched
{
    public sealed class WatchedFactory : ISatFactory
    {
        public AbstractSat Create(CnfFormula cnf)
        {
            return new DpllSat(new WatchedPruner(new WatchedFormula(cnf)));
        }

        public override string ToString()
        {
            return $"{nameof(WatchedFactory)}";
        }
    }
}
