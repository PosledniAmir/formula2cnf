using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.Stats
{
    internal struct MinMeanMax
    {
        private readonly Tuple<string, TimeSpan> _min;

        private readonly Tuple<string, TimeSpan> _max;

        public TimeSpan Min => _min.Item2;
        public string MinName => _min.Item1;

        public readonly TimeSpan Mean;
        public TimeSpan Max => _max.Item2;
        public string MaxName => _max.Item1;

        public MinMeanMax(Tuple<string, TimeSpan> min, TimeSpan mean, Tuple<string, TimeSpan> max)
        {
            _min = min;
            Mean = mean;
            _max = max;
        }
    }
}
