using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    internal sealed class VariableDecider
    {
        private int _variables;

        public VariableDecider(int variables)
        {
            _variables = variables;
        }

        private int Next(int current)
        {
            if (current >= 0)
            {
                return -(current + 1);
            }
            else
            {
                return -current;
            }
        }

        public int Decide(int lastDecided, IReadOnlySet<int> model)
        {
            var next = Next(lastDecided);
            while (model.Contains(next) || model.Contains(-next))
            {
                next = Next(next);
            }

            if (Math.Abs(next) > _variables)
            {
                return 0;
            }

            return next;
        }
    }
}
