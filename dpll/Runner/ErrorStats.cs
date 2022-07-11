using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Runner
{
    public sealed class ErrorStats : ISatStatPrinter
    {
        private TimeSpan _elapsed;
        public TimeSpan Elapsed => _elapsed;

        public ErrorStats(TimeSpan elapsed)
        {
            _elapsed = elapsed;
        }

        public override string ToString()
        {
            return "No stats available.";
        }
    }
}
