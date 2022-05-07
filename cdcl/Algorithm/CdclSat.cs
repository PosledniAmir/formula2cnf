using dpll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watched.Algorithm;

namespace cdcl.Algorithm
{
    internal sealed class CdclSat : AbstractSat
    {
        private readonly ImplicationGraph _graph;

        public CdclSat(IFormulaPruner formula) : base(formula)
        {
            _graph = new ImplicationGraph(formula);
        }

        public override IEnumerable<IReadOnlyList<int>> GetModels()
        {
            var cont = true;
            while (cont)
            {
                if (ExhaustiveResolution())
                {
                    var decision = Decision();
                    if (decision.Success)
                    {
                        _graph.Decide(decision.Variable, decision.Clause);
                        continue;
                    }
                    else if (Satisfied)
                    {
                        yield return GetModel().ToList();
                    }
                }

                if (!BacktrackAndChoose())
                {
                    cont = false;
                }
            }
        }

        private bool ExhaustiveResolution()
        {
            var times = 0;
            var (clause, variable) = GetFirstUnitClause();
            while (variable != 0)
            {
                times++;
                var result = Resolution(clause, variable);
                if (!result.Success)
                {
                    LearnFromConflict(result.Clause, result.Variable);
                    return false;
                }
                else
                {
                    _graph.ByImplication(variable, clause);
                }

                (clause, variable) = GetFirstUnitClause();
            }
            return true;
        }

        private void LearnFromConflict(int clause, int variable)
        {
            var result = _graph.Conflict(variable, clause, Model);
        }
    }
}
