using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public interface IClauseChecker
    {
        public IReadOnlySet<int> Model { get; }
        public bool Satisfied { get; }
        public Tuple<int, int> GetFirstUnitVariable();
        public bool Satisfy(int literal, int clause);
        public void Backtrack(int times);
        public Tuple<int, HashSet<int>> GetDecisionSet();
    }
}
