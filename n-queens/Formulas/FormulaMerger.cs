using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_queens.Formulas
{
    internal static class FormulaMerger
    {
        private static List<Formula> MergeStep(Op op, IReadOnlyList<Formula> formulas)
        {
            var count = formulas.Count;
            var odd = formulas.Count % 2 == 1;
            var result = new List<Formula>(count);

            for (var i = 0; i < count - 1; i += 2)
            {
                result.Add(new TwoFormulas(op, Tuple.Create(formulas[i], formulas[i + 1])));
            }

            if (odd)
            {
                result.Add(formulas.Last());
            }

            return result;
        }

        public static Formula Merge(Op op, IReadOnlyList<Formula> formulas)
        {
            while (formulas.Count != 1)
            {
                formulas = MergeStep(op, formulas);
            }

            return formulas[0];
        }
    }
}
