using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_queens
{
    internal sealed class Literal : Formula
    {
        public readonly bool Positive;
        public readonly int X;
        public readonly int Y;

        public Literal(bool positive, int x, int y)
        {
            Positive = positive;
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            if (Positive)
            {
                return $"p_{X}_{Y}";
            }
            else
            {
                return $"(not p_{X}_{Y})";
            }
        }
    }
}
