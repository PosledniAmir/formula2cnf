using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.Stats
{
    internal sealed class StatRow
    {
        public readonly string Benchmark;
        public readonly string Variables;
        public readonly string Clauses;
        public readonly string Sat;
        public readonly MinMeanMax Stats;

        //Benchmark must lead to directory that is named in this format
        //<NAME>_<VARIABLES>_<CLAUSES>_<SAT>
        public StatRow(string benchmark, MinMeanMax stats)
        {
            var dir = Path.GetFileName(benchmark) ?? "N/A";
            var namedColumns = ParseColumns(dir);
            Benchmark = namedColumns[0];
            Variables = namedColumns[1];
            Clauses = namedColumns[2];
            Sat = namedColumns[3];
            Stats = stats;
        }

        private static string[] ParseColumns(string dirName)
        {
            var result = new string[4];
            var namedColumns = dirName.Split('_', StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < namedColumns.Length; i++)
            {
                result[i] = namedColumns[i];
            }

            for(int i = namedColumns.Length; i < result.Length; i++)
            {
                result[i] = "N/A";
            }

            return result;
        }
    }
}
