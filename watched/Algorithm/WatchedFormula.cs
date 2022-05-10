using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watched.Algorithm
{
    public sealed class WatchedFormula
    {
        private readonly Dictionary<int, LinkedList<WatchedClause>> _map;
        private readonly int _variables;
        private readonly List<WatchedClause> _formula;
        private readonly Stack<List<LinkedListNode<WatchedClause>>> _stack;
        public int Clauses => _formula.Count;
        public int Variables => _variables;
        public IReadOnlyList<WatchedClause> Formula => _formula;

        public WatchedFormula(CnfFormula formula)
        {
            _variables = formula.Variables;
            var list = new List<WatchedClause>();
            var i = 0;
            foreach(var item in formula)
            {
                list.Add(new WatchedClause(i++, item));
            }
            var dict = new Dictionary<int, LinkedList<WatchedClause>>();
            dict[0] = new LinkedList<WatchedClause>();
            for (var variable = 1; variable <= _variables; variable++)
            {
                dict[variable] = new LinkedList<WatchedClause>();
                dict[-variable] = new LinkedList<WatchedClause>();
            }

            foreach(var item in list)
            {
                var (first, second) = item.Exposed;
                dict[first].AddFirst(item);
                if (second != 0)
                {
                    dict[second].AddFirst(item);
                }
            }

            _formula = list;
            _map = dict;
            _stack = new Stack<List<LinkedListNode<WatchedClause>>>();
        }

        public IEnumerable<WatchedClause> SetFalseOn(int literal, IReadOnlySet<int> model)
        {
            var list = _map[literal];
            var node = list.First;
            var moved = new List<LinkedListNode<WatchedClause>>();
            while (node != null)
            {
                var watched = node.Value.SetFalse(literal, model);
                var next = node.Next;
                list.Remove(node);
                moved.Add(node);
                _map[watched].AddFirst(node);
                node = next;
            }
            _stack.Push(moved);
            return moved.Select(m => m.Value);
        }

        public IEnumerable<int> GetExposedOn(int literal)
        {
            return _map[literal].Select(c => c.ClauseId);
        }

        public void Backtrack()
        {
            var moved = _stack.Pop();
            foreach (var item in moved)
            {
                var before = item.Value.Exposed;
                item.Value.Backtrack();
                var after = item.Value.Exposed;
                var (first, second) = DetermineMove(before, after);
                _map[first].Remove(item);
                _map[second].AddFirst(item);
            }
        }

        private Tuple<int, int> DetermineMove(Tuple<int, int> before, Tuple<int, int> after)
        {
            if (before.Item1 != after.Item1)
            {
                return Tuple.Create(before.Item1, after.Item1);
            }
            if (before.Item2 != after.Item2)
            {
                return new Tuple<int, int>(before.Item2, after.Item2);
            }

            throw new ArgumentException("This should not happen.");
        }

        public int AddClause(IEnumerable<int> literals)
        {
            var id = _formula.Count;
            var clause = new WatchedClause(id, literals);
            _formula.Add(clause);
            var (first, second) = clause.Exposed;
            _map[first].AddFirst(clause);
            if (second != 0)
            {
                _map[second].AddFirst(clause);
            }
            return id;
        }
    }
}
