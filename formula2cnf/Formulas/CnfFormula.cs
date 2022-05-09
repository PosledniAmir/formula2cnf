using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    public sealed class CnfFormula : IEnumerable<HashSet<int>>
    {
        private readonly List<HashSet<int>> _formula;
        private readonly int _variables;

        public int Clauses => _formula.Count;
        public int Variables => _variables;
        public IReadOnlyList<HashSet<int>> Formula => _formula;

        public CnfFormula(IEnumerable<IClauseGenerator> generators)
        {
            _formula = generators.SelectMany(g => g.Generate())
                .Select(NegatedVariableFilter)
                .Where(c => c.Count > 0)
                .Select(c => new HashSet<int>(c))
                .ToList();
            _variables = _formula.Select(c => c.Select(v => Math.Abs(v)).Max()).Max();
        }

        public CnfFormula(IEnumerable<IEnumerable<int>> formula)
        {
            var variables = 0;
            var list = new List<HashSet<int>>();

            foreach (var clause in formula)
            {
                var set = new HashSet<int>();
                foreach (var variable in clause)
                {
                    variables = Math.Max(Math.Abs(variable), variables);
                    set.Add(variable);
                }
                list.Add(set);
            }

            _variables = variables;
            _formula = list;
        }

        public int AddClause(IEnumerable<int> clause)
        {
            _formula.Add(clause.ToHashSet());
            return Clauses - 1;
        }

        private static HashSet<int> NegatedVariableFilter(IReadOnlyList<int> clause)
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

            return result;
        }

        public IEnumerator<HashSet<int>> GetEnumerator()
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
