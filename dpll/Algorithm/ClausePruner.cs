using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    internal sealed class ClausePruner
    {
        private readonly Stack<Tuple<int, IReadOnlyList<int>>> _stack;
        private readonly CnfFormula _formula;
        private readonly HashSet<int> _units;

        public IReadOnlySet<int> Units => _units;

        public Tuple<int, IReadOnlyList<int>>? LastStep => _stack.FirstOrDefault();

        public ClausePruner(CnfFormula formula)
        {
            _formula = formula;
            _stack = new Stack<Tuple<int, IReadOnlyList<int>>>();
            _units = new HashSet<int>();
            for (var i = 0; i < _formula.Formula.Count; i++)
            {
                var clause = _formula.Formula[i];
                if (clause.Count == 1)
                {
                    _units.Add(i);
                }
            }
        }

        public bool Prune(int variable, IReadOnlySet<int> unsatisfied)
        {
            var clauses = new List<int>();
            var failed = false;
            foreach (var item in unsatisfied)
            {
                var clause = _formula.Formula[item];
                if (clause.Contains(-variable))
                {
                    clauses.Add(item);
                    clause.Remove(-variable);
                    if (clause.Count == 1)
                    {
                        _units.Add(item);
                    }
                    else if (clause.Count == 0)
                    {
                        failed = true;
                        break;
                    }
                }
            }

            _stack.Push(new Tuple<int, IReadOnlyList<int>>(-variable, clauses));
            return !failed;
        }

        public void Backtrack(int times)
        {
            for (var i = 0; i < times; i++)
            {
                Backtrack();
            }
        }

        public void Backtrack()
        {
            var step = _stack.Pop();

            foreach (var clause in step.Item2)
            {
                var set = _formula.Formula[clause];
                set.Add(step.Item1);
                _units.Remove(clause);
            }
        }
    }
}
