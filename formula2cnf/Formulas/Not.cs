using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal class Not : IClause
    {
        private int _value;

        public Not(int value)
        {
            _value = value;
        }
    }
}
