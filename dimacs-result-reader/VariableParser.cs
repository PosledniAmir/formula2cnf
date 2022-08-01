using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dimacs_result_reader
{
    internal static class VariableParser
    {
        public static Dictionary<int, string> Parse(string dmcFilePath)
        {
            var lines = File
                .ReadAllLines(dmcFilePath)
                .Where(l => l.StartsWith("c"))
                .Where(l => l.Contains('='));

            var result = new Dictionary<int, string>();

            foreach (var line in lines)
            {
                var split = line.Split('=', StringSplitOptions.TrimEntries);
                result[int.Parse(split[1])] = split[0]; 
            }

            return result;
        }
    }
}
