using cdcl;
using dpll.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.Configs
{
    public sealed class CdclConfig : SatConfig
    {
        public int Decisions { get; set; } = 1000;
        public float Multiplier { get; set; } = 1.1F;
        public int Cache { get; set; } = 1000;

        public override ISatFactory GetFactory()
        {
            return new CdclFactory(Decisions, Multiplier, Cache);
        }
    }
}
