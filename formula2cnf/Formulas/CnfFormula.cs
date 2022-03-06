using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal sealed class CnfFormula : IEnumerable<IReadOnlyList<int>>
    {
        private readonly IReadOnlyList<IReadOnlyList<int>> _formula;
        private readonly int _variables;

        public CnfFormula(IEnumerable<IClauseGenerator> generators)
        {
            _formula = generators.SelectMany(g => g.Generate())
                .Select(NegatedVariableFilter)
                .Where(c => c.Count > 0)
                .ToList();
            _variables = _formula.Select(c => c.Select(v => Math.Abs(v)).Max()).Max();
        }

        private static List<int> NegatedVariableFilter(List<int> clause)
        {
            var result = new HashSet<int>(clause.Count);

            foreach (var item in clause)
            {
                if (result.Contains(-item))
                {
                    result.Remove(-item);
                }
                else
                {
                    result.Add(item);
                }
            }

            return result.ToList();
        }

        public IEnumerator<IReadOnlyList<int>> GetEnumerator()
        {
            return _formula.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var builder = new StringBuilder().Append($"p cnf {_variables} {_formula.Count}");
            foreach (var item in _formula.Select(PrintClause))
            {
                builder.Append(Environment.NewLine);
                builder.Append(item);
            }

            return builder.ToString();
        }

        public static string PrintClause(IEnumerable<int> clause)
        {
            var builder = new StringBuilder();

            foreach (var item in clause)
            {
                builder.Append(item);
                builder.Append(' ');
            }

            builder.Append('0');

            return builder.ToString();
        }
    }
}
