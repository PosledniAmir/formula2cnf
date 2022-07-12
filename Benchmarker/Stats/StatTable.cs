using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.Stats
{
    internal sealed class StatTable
    {
        public readonly string FactoryName;
        private readonly List<StatRow> _stats;

        public StatTable(string factoryName)
        {
            FactoryName = factoryName;
            _stats = new List<StatRow>();
        }

        public void Add(string benchmark, MinMeanMax stats)
        {
            _stats.Add(new StatRow(benchmark, stats));
        }

        public List<List<string>> GetColumns()
        {
            var result = new List<List<string>>();
            for (var i = 0; i < 7; i++)
            {
                result.Add(new List<string>());
            }

            result[0].Add(nameof(StatRow.Benchmark));
            result[1].Add(nameof(StatRow.Variables));
            result[2].Add(nameof(StatRow.Clauses));
            result[3].Add(nameof(StatRow.Sat));
            result[4].Add(nameof(StatRow.Stats.Min));
            result[5].Add(nameof(StatRow.Stats.Mean));
            result[6].Add(nameof(StatRow.Stats.Max));

            foreach(var row in _stats)
            {
                result[0].Add(row.Benchmark);
                result[1].Add(row.Variables);
                result[2].Add(row.Clauses);
                result[3].Add(row.Sat);
                result[4].Add(row.Stats.Min.TotalMilliseconds.ToString());
                result[5].Add(row.Stats.Mean.TotalMilliseconds.ToString());
                result[6].Add(row.Stats.Max.TotalMilliseconds.ToString());
            }

            return result;
        }
    }
}
