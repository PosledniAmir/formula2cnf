using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    public sealed class VariableDescriptor
    {
        private readonly IReadOnlyDictionary<string, int> Variables;
        private readonly int First;
        private readonly IReadOnlySet<int> Generated;

        public VariableDescriptor(int first, int count, IEnumerable<Tuple<int, string>> variables)
        {
            var generated = new HashSet<int>(Enumerable.Range(1, count));
            var dict = new Dictionary<string, int>();
            foreach (var (computed, given) in variables)
            {
                generated.Remove(computed);
                dict.Add(given, computed);
            }
            First = first;
            Variables = dict;
            Generated = generated;
        }

        public override string ToString()
        {
            var builder = new StringBuilder()
                .AppendLine($"c Root variable: {First}")
                .AppendLine("c Original variables:");
            foreach (var variable in Variables)
            {
                builder.AppendLine($"c {variable.Key} = {variable.Value}");
            }

            return builder.ToString();
        }
    }
}
