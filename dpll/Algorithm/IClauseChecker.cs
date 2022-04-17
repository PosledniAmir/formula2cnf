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
        public int GetFirstUnitVariable();
        public bool Satisfy(int literal);
        public void Backtrack(int times);
        public HashSet<int> GetDecisionSet();
    }
}
