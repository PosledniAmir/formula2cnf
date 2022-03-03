﻿using System;
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

        public List<List<int>> Generate(Implication generator)
        {
            throw new NotImplementedException();
        }

        public List<List<int>> Generate(Equivalence generator)
        {
            var value = generator.Variable;

            if (_left == -_right)
            {
                return new List<List<int>>
                {
                    new List<int> { _left },
                    new List<int> { _right },
                };
            }
            else
            {
                return new List<List<int>>
                {
                    new List<int> { - value, _left },
                    new List<int> { - value, _right },
                    new List<int> { value, - _left, - _right },
                };
            }
        }
    }
}
