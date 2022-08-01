using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dimacs_result_reader
{
    internal static class ResultParser
    {
        public static IEnumerable<string> Parse(string resFilePath, Dictionary<int, string> variables)
        {
            var line = File
                .ReadAllLines(resFilePath)
                .Where(l => l.StartsWith("v")).First();

            var values = line.Split(' ', StringSplitOptions.TrimEntries);

            foreach (var value in values)
            {
                if (int.TryParse(value, out var assignment))
                {
                    if (variables.TryGetValue(Math.Abs(assignment), out var name))
                    {
                        var truthness = assignment > 0 ? true : false;
                        yield return $"{name} = {truthness}";
                    }
                }
            }
        }
    }
}
