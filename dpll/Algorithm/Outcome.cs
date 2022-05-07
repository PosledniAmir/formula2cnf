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
        public readonly int Variable;
        public readonly int Clause;

        public Outcome(bool success, int variable, int clause)
        {
            Success = success;
            Variable = variable;
            Clause = clause;
        }

        public Outcome()
        {
            Success = false;
            Variable = 0;
            Clause = -1;
        }
    }
}
