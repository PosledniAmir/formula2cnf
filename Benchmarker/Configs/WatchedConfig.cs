using dpll.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watched;

namespace Benchmarker.Configs
{
    public sealed class WatchedConfig : SatConfig
    {
        public override ISatFactory GetFactory()
        {
            return new WatchedFactory();
        }
    }
}
