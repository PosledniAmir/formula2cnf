using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Runner
{
    public sealed class SatStats
    {
        public readonly TimeSpan Elapsed;

        public int Decisions;

        public int Resolutions;

        public int LearnedClauses;

        public SatStats(TimeSpan elapsed, int decisions, int resolutions, int learnedClauses)
        {
            Decisions = decisions;
            Resolutions = resolutions;
            LearnedClauses = learnedClauses;
            Elapsed = elapsed;
        }

        public SatStats(TimeSpan elapsed, int decisions, int resolutions) : this(elapsed, decisions, resolutions, 0)
        {
        }

        public SatStats(TimeSpan elapsed) : this(elapsed, 0, 0, 0)
        {
        }
    }
}
