using dpll.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.Stats
{
    internal sealed class SatAggregator
    {
        private readonly List<Tuple<string, SatResult>> _successes;
        private readonly List<Tuple<string, SatResult>> _failures;

        public int SuccessCount => _successes.Count;
        public int FailureCount => _failures.Count;

        public SatAggregator()
        {
            _successes = new List<Tuple<string, SatResult>>();
            _failures = new List<Tuple<string, SatResult>>();
        }

        public void Add(string filePath, SatResult result)
        {
            if (result.HadError())
            {
                _failures.Add(Tuple.Create(filePath, result));
            }
            else
            {
                _successes.Add(Tuple.Create(filePath, result));
            }
        }

        public MinMeanMax GetMinMeanMax()
        {
            string minStr = "";
            TimeSpan minTs = TimeSpan.MaxValue;
            string maxStr = "";
            TimeSpan maxTs = TimeSpan.Zero;
            TimeSpan mean = TimeSpan.Zero;

            foreach (var (file, time) in _successes)
            {
                if (time.Stats.Elapsed < minTs)
                {
                    minStr = file;
                    minTs = time.Stats.Elapsed;
                }

                if (time.Stats.Elapsed > maxTs)
                {
                    maxStr = file;
                    maxTs = time.Stats.Elapsed;
                }

                mean += time.Stats.Elapsed;
            }

            if (_successes.Count != 0)
            {
                mean /= _successes.Count;
            }

            return new MinMeanMax(Tuple.Create(minStr, minTs), mean, Tuple.Create(maxStr, maxTs));
        }
    }
}
