using n_queens.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_queens.Builders
{
    internal sealed class CellAtBuilder
    {
        private readonly QueenAtBuilder _builder;
        public int Size => _builder.Size;
        public int X => _builder.X;
        public int Y => _builder.Y;

        public CellAtBuilder(int size, int x, int y)
        {
            _builder = new QueenAtBuilder(size, x, y);
        }

        public Formula Build()
        {
            var queenAt = _builder.Build();
            var notQueenAt = new Literal(false, X, Y);
            return new TwoFormulas(Op.Or, notQueenAt, queenAt);
        }
    }
}
