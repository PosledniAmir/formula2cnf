using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf
{
    internal struct TextPosition
    {
        public int Position;
        public int Length;

        public TextPosition(int position, int length)
        {
            Position = position;
            Length = length;
        }
    }
}
