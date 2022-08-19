using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backbones
{
    internal sealed class SolutionManager
    {
        private readonly Solver _solver;
        private readonly BoolExpr _formula;
        private readonly IReadOnlyDictionary<int, BoolExpr> _variables;

        public SolutionManager(Solver solver, BoolExpr formula, IReadOnlyDictionary<int, BoolExpr> variables)
        {
            _solver = solver;
            _formula = formula;
            _variables = variables;
        }

        public HashSet<int> Solve()
        {
            var result = new HashSet<int>();

            _solver.Assert(_formula);
            var status = _solver.Check();

            if (status == Status.SATISFIABLE)
            {
                var model = _solver.Model;
                foreach (var (key, val) in _variables)
                {
                    if (model.Evaluate(val).IsTrue)
                    {
                        result.Add(key);
                    }
                    else if (model.Evaluate(val).IsFalse)
                    {
                        result.Add(-key);
                    }
                    
                }
            }

            _solver.Reset();

            return result;
        }

        public HashSet<int> SolveWith(BoolExpr assumption)
        {
            _solver.Assert(assumption);
            return Solve();
        }
    }
}
