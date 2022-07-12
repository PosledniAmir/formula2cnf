using cdcl.Algorithm;
using dpll.Algorithm;
using dpll.Runner;
using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watched.Algorithm;

namespace cdcl
{
    public sealed class CdclFactory : ISatFactory
    {
        public readonly int Decisions;
        public readonly float Multiplier;
        public readonly int Cache;

        public CdclFactory(int decisions, float multiplier, int cache)
        {
            Decisions = decisions;
            Multiplier = multiplier;
            Cache = cache;
        }

        public AbstractSat Create(CnfFormula cnf)
        {
            return new CdclSat(new WatchedPruner(new WatchedFormula(cnf)), Decisions, Multiplier, Cache);
        }

        public override string ToString()
        {
            return $"{nameof(CdclFactory)}:{nameof(Decisions)}={Decisions};{nameof(Multiplier)}={Multiplier};{nameof(Cache)}={Cache};";
        }
    }
}
