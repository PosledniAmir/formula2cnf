using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal class And : IConsumer
    {
        private readonly int _left;
        private readonly int _right;

        public And(int left, int right)
        {
            _left = left;
            _right = right;
        }

        public IEnumerable<IReadOnlyList<int>> Generate(Equivalence generator)
        {
            var value = generator.Variable;
            if (_left == -_right)
            {
                yield return new List<int> { value };
            }
            else
            {
                yield return new List<int> { value, -_left, -_right };
            }
            yield return new List<int> { -value, _left };
            yield return new List<int> { -value, _right };
        }

        public IEnumerable<IReadOnlyList<int>> Generate(Implication generator)
        {
            var value = generator.Variable;
            yield return new List<int> { -value, _left };
            yield return new List<int> { -value, _right };
        }
    }
}
