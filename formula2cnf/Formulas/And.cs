using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal class And : IClause
    {
        private int _left;
        private int _right;

        public And(int left, int right)
        {
            _left = left;
            _right = right;
        }
    }
}
