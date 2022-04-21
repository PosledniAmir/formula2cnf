using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    internal sealed class WatchedClause
    {
        private readonly struct Step
        {
            public readonly bool First;
            public readonly int Taken;
            public readonly IReadOnlyList<int> Unviable;

            public Step(bool first, int taken, List<int> unviable)
            {
                First = first; Taken = taken; Unviable = unviable; 
            }
        }
        private readonly int _clauseId;
        private readonly IReadOnlyList<int> _fullLiterals;
        private readonly WatchQueue<int> _literals;
        private readonly Stack<Step> _stack;

        public int ClauseId => _clauseId;
        public Tuple<int, int> Exposed => Tuple.Create(_literals.First, _literals.Second);
        public bool Unsatisfiable => _literals.First == 0 && _literals.Second == 0;
        public bool Unit => _literals.First == 0 ^ _literals.Second == 0;

        public IReadOnlyList<int> Literals => _fullLiterals;

        public WatchedClause(int clauseId, IEnumerable<int> literals)
        {
            _clauseId = clauseId;
            _literals = new WatchQueue<int>(literals);
            _fullLiterals = new List<int>(literals);
            _stack = new Stack<Step>();
            if (_literals.First == 0 && _literals.Second == 0)
            {
                throw new ArgumentException("Clause cannot be empty");
            }
        }

        public int SetFalse(int literal, IReadOnlySet<int> model)
        {
            int value;
            var unviable = new List<int>();
            var wasFirst = false;
            if (literal == _literals.First)
            {
                wasFirst = true;
                _literals.TakeFirst();
                while (model.Contains(-_literals.First))
                {
                    unviable.Add(_literals.First);
                    _literals.TakeFirst();
                }
                value = _literals.First;
            }
            else if (literal == _literals.Second)
            {
                _literals.TakeSecond();
                while (model.Contains(-_literals.Second))
                {
                    unviable.Add(_literals.Second);
                    _literals.TakeSecond();
                }
                value = _literals.Second;
            }
            else
            {
                throw new ArgumentException("Cannot set false unwatched literal");
            }

            _stack.Push(new Step(wasFirst, literal, unviable));
            return value;
        }

        public void Backtrack()
        {
            var step = _stack.Pop();
            int toQueue;
            _literals.Enqueue(step.Taken);
            if (step.First)
            {
                toQueue = _literals.TakeFirst();
            }
            else
            {
                toQueue = _literals.TakeSecond();
            }

            foreach (var item in step.Unviable)
            {
                _literals.Enqueue(item);
            }

            if (toQueue != 0)
            {
                _literals.Enqueue(toQueue);
            }
        }
    }
}
