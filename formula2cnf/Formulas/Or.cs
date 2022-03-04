using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal class Or : IConsumer
    {
        private int _left;
        private int _right;

        public Or(int left, int right)
        {
            _left = left;
            _right = right;
        }

        public IEnumerable<List<int>> Generate(Equivalence generator)
        {
            var value = generator.Variable;
            yield return new List<int> { -value, _left, _right };
            yield return new List<int> { value, -_left };
            yield return new List<int> { value, -_right };
        }

        public IEnumerable<List<int>> Generate(Implication generator)
        {
            var value = generator.Variable;
            yield return new List<int> { -value, _left, _right };
        }
    }
}
