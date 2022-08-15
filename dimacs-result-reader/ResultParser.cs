using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dimacs_result_reader
{
    internal static class ResultParser
    {
        public static Dictionary<string, bool> Parse(string resFilePath, Dictionary<int, string> variables)
        {
            var result = new Dictionary<string, bool>();
            var lines = File.ReadAllLines(resFilePath).Skip(6).ToList();

            foreach (var line in lines)
            {
                var values = line.Split(' ', StringSplitOptions.TrimEntries);
                if (values.Length >= 3)
                {
                    var value = int.Parse(values[0]);
                    var truthness = bool.Parse(values[2]);
                    if (variables.TryGetValue(Math.Abs(value), out var name))
                    {
                        result.Add(name, truthness);
                    }
                }
            }

            return result;
        }
    }
}
