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

            public Step(bool first, int taken, IEnumerable<int> unviable)
            {
                First = first; Taken = taken; Unviable = unviable.ToList(); 
            }
        }
        private readonly int _clauseId;
        private readonly IReadOnlyList<int> _fullLiterals;
        private readonly Queue<int> _literals;
        private readonly Stack<Step> _stack;
        private Tuple<int, int> _exposed;

        public int ClauseId => _clauseId;
        public Tuple<int, int> Exposed => _exposed;
        public bool Unsatisfiable => _exposed.Item1 == 0 && _exposed.Item2 == 0;
        public bool Unit => _exposed.Item1 == 0 || _exposed.Item2 == 0;

        public IReadOnlyList<int> Literals => _fullLiterals;

        public WatchedClause(int clauseId, IEnumerable<int> literals)
        {
            _clauseId = clauseId;
            _literals = new Queue<int>(literals);
            _fullLiterals = new List<int>(literals);
            _stack = new Stack<Step>();
            if (_literals.Count == 0)
            {
                throw new ArgumentException("Clause cannot be empty");
            }
            else if (_literals.Count == 1)
            {
                _exposed = Tuple.Create(_literals.Dequeue(), 0);
            }
            else
            {
                _exposed = Tuple.Create(_literals.Dequeue(), _literals.Dequeue());
            }
        }

        private IEnumerable<int> FilterUnviableLiterals(IReadOnlySet<int> model)
        {
            var literal = _literals.Peek();
            while (_literals.Count > 1)
            {
                if (model.Contains(literal))
                {
                    yield break;
                }
                else if (model.Contains(-literal))
                {
                    yield return literal;
                }
                _literals.Dequeue();
                literal = _literals.Peek();
            }

            if (model.Contains(literal))
            {
                yield break;
            }
            else if (model.Contains(-literal))
            {
                _literals.Dequeue();
                yield return literal;
            }
        }

        public int SetFalse(int literal, IReadOnlySet<int> model)
        {
            var unviable = FilterUnviableLiterals(model);
            var (first, second) = _exposed;
            var wasFirst = false;
            var value = 0;
            if (_literals.Count > 0)
            {
                value = _literals.Dequeue();
            }

            int old;
            if (first == literal)
            {
                old = first;
                first = value;
                wasFirst = true;
            }
            else if (second == literal)
            {
                old = second;
                second = value;
            }
            else
            {
                throw new ArgumentException("Cannot set false unwatched literal");
            }

            _stack.Push(new Step(wasFirst, old, unviable));
            _exposed = Tuple.Create(first, second);
            return value;
        }

        public void Backtrack()
        {
            var step = _stack.Pop();
            var (first, second) = _exposed;
            int toQueue;
            if (step.First)
            {
                toQueue = first;
                first = step.Taken;
            }
            else
            {
                toQueue = second;
                second = step.Taken;
            }

            foreach (var item in step.Unviable)
            {
                _literals.Enqueue(item);
            }

            _literals.Enqueue(toQueue);
            _exposed = Tuple.Create(first, second);
        }
    }
}
