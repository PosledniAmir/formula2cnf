using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backbones
{
    internal static class JeroslowWang
    {
        public static Dictionary<int, double> Compute(CnfFormula formula)
        {
            var score = Enumerable.Range(1, formula.Variables).ToDictionary(k => k, k => 0.0d);

            foreach (var clause in formula.Formula)
            {
                foreach (var literal in clause)
                {
                    var positive = Math.Abs(literal);
                    score[positive] += Math.Pow(2, -clause.Count);
                }
            }

            return score;
        }
    }
}
