using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public readonly struct Outcome
    {
        public readonly bool Success;
        public readonly int TriedVariable;
        public readonly int TriedClause;
        public readonly int ConflictClause;


        public Outcome(int triedVariable, int triedClause, int conflictClause, bool success)
        {
            Success = success;
            TriedVariable = triedVariable;
            TriedClause = triedClause;
            ConflictClause = conflictClause;
        }

        public Outcome()
        {
            Success = false;
            TriedVariable = 0;
            TriedClause = -1;
            ConflictClause = -1;
        }
    }
}
