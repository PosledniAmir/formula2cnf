using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    internal sealed class ClauseWatcher
    {
        private readonly struct Step
        {
            public readonly int Literal;
            public readonly IReadOnlyList<LinkedListNode<WatchedClause>> Moved;

            public Step(int literal, IReadOnlyList<LinkedListNode<WatchedClause>> moved)
            {
                Literal = literal;
                Moved = moved;
            }
        }

        private readonly IReadOnlyDictionary<int, LinkedList<WatchedClause>> _literals;
        private readonly IReadOnlyList<WatchedClause> _formula;
        private readonly Stack<Step> _stack;

        public IReadOnlyList<WatchedClause> Formula;

        public ClauseWatcher(IEnumerable<HashSet<int>> formula, int variables)
        {
            var literals = new Dictionary<int, LinkedList<WatchedClause>>();
            for (int i = 1; i <= variables; i++)
            {
                literals.Add(i, new LinkedList<WatchedClause>());
                literals.Add(-i, new LinkedList<WatchedClause>());
            }

            var clauseId = 0;
            var temp = new List<WatchedClause>();
            foreach (var clause in formula)
            {
                var watched = new WatchedClause(clauseId++, clause);
                temp.Add(watched);
                var (first, second) = watched.Exposed;

                while (first != 0)
                {
                    literals[first].AddFirst(watched);
                    first = second;
                    second = 0;
                }
            }

            _formula = temp;
            _stack = new Stack<Step>();
            _literals = literals;
        }

        public bool Prune(int literal, HashSet<int> model, out IReadOnlySet<int> unitClauses)
        {
            var units = new HashSet<int>();
            var moved = new List<LinkedListNode<WatchedClause>>();
            var failed = false;
            var list = _literals[-literal];
            var node = list.First;
            while (node != null)
            {
                var watched = node.Value.SetFalse(-literal, model);

                if (watched == 0 && node.Value.Unsatisfiable)
                {
                    failed = true;
                    break;
                }

                list.Remove(node);
                moved.Add(node);

                if (watched != 0)
                {
                    _literals[watched].AddFirst(node);
                }
                else
                {
                    units.Add(node.Value.ClauseId);
                }

                node = list.First;
            }

            unitClauses = units;
            _stack.Push(new Step(literal, moved));
            return !failed;
        }

        public IEnumerable<int> GetSatisfiedClauses(int literal)
        {
            foreach (var clause in _literals[literal])
            {
                yield return clause.ClauseId;
            }
        }

        public void Backtrack()
        {
            var step = _stack.Pop();
            foreach (var item in step.Moved)
            {
                var before = item.Value.Exposed;
                item.Value.Backtrack();
                var after = item.Value.Exposed;
                var (remove, add) = DetermineStep(before, after);
                _literals[remove].Remove(item);
                _literals[add].AddFirst(item);
            }
        }

        private Tuple<int, int> DetermineStep(Tuple<int, int> before, Tuple<int, int> after)
        {
            if (before.Item1 != after.Item1)
            {
                return new Tuple<int, int>(before.Item1, after.Item1);
            }
            else if (before.Item2 != after.Item2)
            {
                return new Tuple<int, int>(before.Item2, after.Item2);
            }
            else
            {
                throw new ArgumentException("This should not happen, clause was not changed.");
            }
        }
    }
}
