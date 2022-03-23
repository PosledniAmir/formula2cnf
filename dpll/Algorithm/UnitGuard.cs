using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    internal sealed class UnitGuard
    {
        private readonly HashSet<int> _clauses;
        private readonly Stack<Tuple<int, IReadOnlyList<int>>> _resoluted;
        private readonly CnfFormula _formula;

        public IReadOnlySet<int> Clauses => _clauses;

        public UnitGuard(CnfFormula formula)
        {
            _formula = formula;
            _clauses = new HashSet<int>();
            _resoluted = new Stack<Tuple<int, IReadOnlyList<int>>>();
            FillGuard();
        }

        private IEnumerable<int> UnitClauses(IEnumerable<int> resolutedClauses)
        {
            return resolutedClauses.Where(i => _formula.Formula[i].Count == 1 && !_clauses.Contains(i));
        }

        private void FillGuard()
        {
            for (int i = 0; i < _formula.Formula.Count; i++)
            {
                if (_formula.Formula[i].Count == 1)
                {
                    _clauses.Add(i);
                }
            }
        }

        public void Add(int resolutionClause, IReadOnlyList<int> resolutedClauses)
        {
            var units = UnitClauses(resolutedClauses).ToList();
            if (resolutionClause != -1)
            {
                _clauses.Remove(resolutionClause);
                _resoluted.Push(new Tuple<int, IReadOnlyList<int>>(resolutionClause, units));
            }

            foreach (var item in units)
            {
                _clauses.Add(item);
            }
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
            var (clause, units) = _resoluted.Pop();
            _clauses.Add(clause);
            foreach (var item in units)
            {
                _clauses.Remove(item);
            }
        }

    }
}
