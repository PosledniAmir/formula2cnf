using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf
{
    internal static class DimacsPrinter
    {
        public static string ToString(List<List<int>> cnf)
        {
            var max = -1;
            var lines = new List<string>();
            foreach (var item in cnf.Where(c => c.Count > 0))
            {
                var (line, current) = ClauseWithMax(item);
                if (current > max)
                {
                    max = current;
                }
                lines.Add(line);
            }
            
            var builder = new StringBuilder().AppendLine($"p cnf {max + 1} {cnf.Count}");
            foreach (var item in lines)
            {
                builder.AppendLine(item);
            }

            return builder.ToString();
        }

        public static Tuple<string, int> ClauseWithMax(List<int> clause)
        {
            var builder = new StringBuilder();
            var max = -1;

            foreach (var item in clause)
            {
                builder.Append(item);
                builder.Append(' ');
                var absolute = Math.Abs(item);
                if (absolute > max)
                {
                    max = absolute;
                }
            }

            builder.Append('0');

            return Tuple.Create(builder.ToString(), max);
        }
    }
}
