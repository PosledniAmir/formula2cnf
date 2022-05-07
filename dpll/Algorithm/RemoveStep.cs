using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public readonly struct RemoveStep
    {
        public readonly bool Result;
        public readonly IReadOnlyList<int> Units;
        public readonly IReadOnlyList<int> Satisfied;

        public RemoveStep(bool result, IReadOnlyList<int> units, IReadOnlyList<int> satisfied)
        {
            Result = result;
            Units = units;
            Satisfied = satisfied;
        }
    }
}
