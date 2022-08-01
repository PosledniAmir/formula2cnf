using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_queens
{
    internal sealed class RowAtBuilder
    {
        private readonly IReadOnlyList<CellAtBuilder> _cells;
        public readonly int Y;
        public int Size => _cells.Count;

        public RowAtBuilder(int size, int y)
        {
            Y = y;
            _cells = Enumerable
                .Range(0, size)
                .Select(x => new CellAtBuilder(size, x, y))
                .ToList();
        }

        public Formula Build()
        {
            return FormulaMerger.Merge(Op.And, _cells.Select(c => c.Build()).ToList());
        }
    }
}
