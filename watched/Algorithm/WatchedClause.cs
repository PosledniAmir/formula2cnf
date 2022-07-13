using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    public sealed class WatchedClause
    {
        private readonly int _clauseId;
        private readonly IReadOnlyList<int> _fullLiterals;
        private readonly WatchedArray<int> _literals;

        public int ClauseId => _clauseId;
        public Tuple<int, int> Exposed => Tuple.Create(_literals.First, _literals.Second);
        public bool Unsatisfiable => _literals.First == 0 && _literals.Second == 0;
        public bool Unit => _literals.First == 0 ^ _literals.Second == 0;

        public IReadOnlyList<int> Literals => _fullLiterals;

        public WatchedClause(int clauseId, IEnumerable<int> literals)
        {
            _clauseId = clauseId;
            _literals = new WatchedArray<int>(literals);
            _fullLiterals = new List<int>(literals);
            if (_literals.First == 0 && _literals.Second == 0)
            {
                throw new ArgumentException("Clause cannot be empty");
            }
        }

        public int SetFalse(int literal, IReadOnlySet<int> model)
        {
            int value;
            if (literal == _literals.First)
            {
                _literals.TakeFirst(l => !model.Contains(-l));
                value = _literals.First;
            }
            else if (literal == _literals.Second)
            {
                _literals.TakeSecond(l => !model.Contains(-l));
                value = _literals.Second;
            }
            else
            {
                throw new ArgumentException("Cannot set false unwatched literal");
            }

            return value;
        }

        public void Reset(int literal)
        {
            if (_literals.First == 0)
            {
                _literals.TakeFirst(l => l == literal);
            }
            else if (_literals.Second == 0)
            {
                _literals.TakeSecond(l => l == literal);
            }
            else
            {
                throw new ArgumentException("Do not reset when there is no zero.");
            }
        }
    }
}
