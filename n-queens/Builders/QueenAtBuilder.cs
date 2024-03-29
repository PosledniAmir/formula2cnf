﻿using n_queens.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_queens.Builders
{
    internal sealed class QueenAtBuilder
    {
        public readonly int Size;
        public readonly int X;
        public readonly int Y;

        public QueenAtBuilder(int size, int x, int y)
        {
            Size = size;
            X = x;
            Y = y;
        }

        private IEnumerable<Tuple<int, int>> GenerateRow()
        {
            foreach (var item in Enumerable.Range(0, Size))
            {
                if (item != X)
                {
                    yield return Tuple.Create(item, Y);
                }
            }
        }

        private IEnumerable<Tuple<int, int>> GenerateColumn()
        {
            foreach (var item in Enumerable.Range(0, Size))
            {
                if (item != Y)
                {
                    yield return Tuple.Create(X, item);
                }
            }
        }

        private IEnumerable<Tuple<int, int>> GenerateLeftDiagonal()
        {
            var x = X - 1;
            var y = Y - 1;
            while (x >= 0 && y >= 0)
            {
                yield return new Tuple<int, int>(x, y);
                x--;
                y--;
            }

            x = X + 1;
            y = Y + 1;
            while (x < Size && y < Size)
            {
                yield return new Tuple<int, int>(x, y);
                x++;
                y++;
            }
        }

        private IEnumerable<Tuple<int, int>> GenerateRightDiagonal()
        {
            var x = X - 1;
            var y = Y + 1;
            while (x >= 0 && y < Size)
            {
                yield return new Tuple<int, int>(x, y);
                x--;
                y++;
            }

            x = X + 1;
            y = Y - 1;
            while (x < Size && y >= 0)
            {
                yield return new Tuple<int, int>(x, y);
                x++;
                y--;
            }
        }

        public Formula Build()
        {
            var literals = new List<Formula>(4 * Size)
            {
                new Literal(true, X, Y)
            };

            literals.AddRange(GenerateRow().Select(t => new Literal(false, t.Item1, t.Item2)));
            literals.AddRange(GenerateColumn().Select(t => new Literal(false, t.Item1, t.Item2)));
            literals.AddRange(GenerateLeftDiagonal().Select(t => new Literal(false, t.Item1, t.Item2)));
            literals.AddRange(GenerateRightDiagonal().Select(t => new Literal(false, t.Item1, t.Item2)));

            return FormulaMerger.Merge(Op.And, literals);
        }
    }
}
