using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker.Stats
{
    internal sealed class TablePrinter
    {
        private readonly List<List<string>> _columns;
        public TablePrinter(List<List<string>> columns)
        {
            _columns = columns;
        }

        private int GetRowCount()
        {
            var min = int.MaxValue;

            foreach(var column in _columns)
            {
                if (min > column.Count)
                {
                    min = column.Count;
                }
            }

            return min;
        }

        private static int GetColumnWidth(List<string> column)
        {
            var max = 0;
            foreach(var cell in column)
            {
                if (max < cell.Length)
                {
                    max = cell.Length;
                }
            }

            return max;
        }

        private static string GetWithWidth(string item, int width)
        {
            var builder = new StringBuilder(item);

            for (int i = item.Length; i < width; i++)
            {
                builder.Append(' ');
            }

            return builder.ToString();
        }

        private static string GetSeparator(List<int> columnWidths)
        {
            var builder = new StringBuilder();

            foreach(var width in columnWidths)
            {
                builder.Append("|-");
                for(int i = 0; i < width; i++)
                {
                    builder.Append('-');
                }
                builder.Append("-|");
            }

            return builder.ToString();
        }

        public IEnumerable<string> GetLines()
        {
            var rows = GetRowCount();
            var columns = _columns.Count;
            var columnWidths = new List<int>();

            foreach(var item in _columns)
            {
                columnWidths.Add(GetColumnWidth(item));
            }

            for(int row = 0; row < rows; row++)
            {
                var builder = new StringBuilder();

                for(int col = 0; col < columns; col++)
                {
                    builder
                        .Append("| ")
                        .Append(GetWithWidth(_columns[col][row], columnWidths[col]))
                        .Append(" |");
                }

                yield return builder.ToString();

                if (row == 0)
                {
                    yield return GetSeparator(columnWidths);
                }
            }
        }
    }
}
