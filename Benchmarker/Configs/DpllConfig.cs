using dpll.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.Configs
{
    public sealed class DpllConfig : SatConfig
    {
        public override ISatFactory GetFactory()
        {
            return new DpllFactory();
        }
    }
}
