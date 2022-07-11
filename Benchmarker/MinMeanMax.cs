using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker
{
    internal struct MinMeanMax
    {
        public readonly Tuple<string, TimeSpan> Min;
        public readonly TimeSpan Mean;
        public readonly Tuple<string, TimeSpan> Max;

        public MinMeanMax(Tuple<string, TimeSpan> min, TimeSpan mean, Tuple<string, TimeSpan> max)
        {
            Min = min;
            Mean = mean;
            Max = max;
        }
    }
}
