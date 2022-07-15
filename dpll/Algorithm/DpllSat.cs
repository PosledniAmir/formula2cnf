using dpll.Runner;
using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public sealed class DpllSat : AbstractSat
    {
        public DpllSat(IFormulaPruner formula) : base(formula)
        {
        }

        public override IEnumerable<IReadOnlyList<int>> GetModels()
        {
            var cont = true;
            while (cont)
            {
                if (ExhaustiveResolution())
                {
                    if (Decision().Success)
                    {
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

        public override SatStats GetStats(TimeSpan elapsed)
        {
            return new SatStats(elapsed, Decisions, Resolutions);
        }

        private bool BacktrackAndChoose()
        {
            while (CanBacktrack())
            {
                var (last, can) = Backtrack();

                if (!can)
                {
                    continue;
                }

                last = _stack.Flip(last);
                var outcome = _clauseChecker.Satisfy(last, -1);
                if (outcome.Result)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ExhaustiveResolution()
        {
            var times = 0;
            var (clause, variable) = GetFirstUnitClause();
            while(variable != 0)
            {
                times++;

                if (!Resolution(clause, variable).Success)
                {
                    return false;
                }

                (clause, variable) = GetFirstUnitClause();
            }
            return true;
        }
    }
}
