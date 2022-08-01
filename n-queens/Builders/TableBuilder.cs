using n_queens.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_queens.Builders
{
    internal sealed class TableBuilder
    {
        private IReadOnlyList<RowAtBuilder> _rows;
        public int Size => _rows.Count;

        public TableBuilder(int size)
        {
            _rows = Enumerable
                .Range(0, size)
                .Select(y => new RowAtBuilder(size, y))
                .ToList();
        }

        public Formula Build()
        {
            return FormulaMerger.Merge(Op.And, _rows.Select(r => r.Build()).ToList());
        }
    }
}
