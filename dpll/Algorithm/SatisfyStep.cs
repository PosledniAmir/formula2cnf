using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public class SatisfyStep
    {
        public readonly bool Result;
        public readonly IReadOnlyList<int> Units;
        public readonly IReadOnlyList<int> Satisfied;
        public readonly int ConflictClause;
        public readonly int ConflictVariable;

        public SatisfyStep(IReadOnlyList<int> units, IReadOnlyList<int> satisfied)
        {
            Result = true;
            Units = units;
            Satisfied = satisfied;
            ConflictClause = -1;
            ConflictVariable = 0;
        }

        public SatisfyStep(int conflictClause, int conflictVariable)
        {
            Result = false;
            Units = new List<int>();
            Satisfied = new List<int>();
            ConflictClause = conflictClause;
            ConflictVariable = conflictVariable;
        }
    }
}
