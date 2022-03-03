using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal class Not : IConsumer
    {
        private readonly int _value;

        public Not(int value)
        {
            _value = value;
        }

        public List<List<int>> Generate(Implication generator)
        {
            throw new NotImplementedException();
        }

        public List<List<int>> Generate(Equivalence generator)
        {
            throw new NotImplementedException();
        }
    }
}
