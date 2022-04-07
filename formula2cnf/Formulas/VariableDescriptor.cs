using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    public sealed class VariableDescriptor
    {
        private readonly IReadOnlyDictionary<string, int> _variables;
        private readonly IReadOnlyDictionary<int, string> _variableNames;
        private readonly int _first;
        private readonly IReadOnlySet<int> _generated;

        public VariableDescriptor(int first, int count, IEnumerable<Tuple<int, string>> variables)
        {
            var generated = new HashSet<int>(Enumerable.Range(1, count));
            var dict = new Dictionary<string, int>();
            var names = new Dictionary<int, string>();
            foreach (var (computed, given) in variables)
            {
                generated.Remove(computed);
                dict.Add(given, computed);
                names.Add(computed, given);
            }
            _first = first;
            _variables = dict;
            _variableNames = names;
            _generated = generated;
        }

        public override string ToString()
        {
            var builder = new StringBuilder()
                .AppendLine($"c Root variable: {_first}")
                .AppendLine("c Original variables:");
            foreach (var variable in _variables)
            {
                builder.AppendLine($"c {variable.Key} = {variable.Value}");
            }

            builder.AppendLine("c Generated variables:");
            foreach (var variable in _generated)
            {
                builder.AppendLine($"c {variable}");
            }

            return builder.ToString();
        }

        public bool TryTranslate(int i, out string? variable)
        {
            return _variableNames.TryGetValue(i, out variable);
        }
    }
}
