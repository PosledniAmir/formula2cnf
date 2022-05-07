using dpll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cdcl.Algorithm
{
    internal sealed class ImplicationGraph
    {
        private readonly Stack<DecisionTrail> _trail;
        private readonly IFormulaPruner _formula;

        public ImplicationGraph(IFormulaPruner formula)
        {
            _trail = new Stack<DecisionTrail>();
            _formula = formula;
        }

        public void Decide(int variable, int clause)
        {
            _trail.Push(new DecisionTrail(variable, clause));
        }

        public void ByImplication(int variable, int clause)
        {
            if (_trail.Count > 0)
            {
                _trail.Peek().Implication(variable, clause);
            }
        }

        public HashSet<int> Conflict(int variable, int clause, IReadOnlySet<int> model)
        {
            var uip = new HashSet<int>();
            var rest = new HashSet<int>();
            var step = _trail.Pop();

            do
            {
                var literals = _formula.Literals(clause);
                var (responsible, outsiders) = step.Responsible(literals);
                Merge(uip, responsible);
                Merge(rest, outsiders.Where(l => model.Contains(l)));
                (variable, clause) = step.Pop();
                uip.Remove(variable);
            } while (uip.Count > 1);

            return BuildClause(uip.First(), rest);
        }

        private void Merge(HashSet<int> set, IEnumerable<int> items)
        {
            foreach (var item in items)
            {
                set.Add(item);
            }
        }

        private HashSet<int> BuildClause(int variable, HashSet<int> rest)
        {
            var result = new HashSet<int>();
            result.Add(variable);
            foreach (var item in rest)
            {
                if (!result.Contains(-item))
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
