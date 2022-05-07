using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public sealed class UnitSet
    {
        private readonly Stack<IReadOnlyList<int>> _stack;
        private readonly HashSet<int> _units;

        public IReadOnlySet<int> Units => _units;

        public UnitSet()
        {
            _stack = new Stack<IReadOnlyList<int>>();
            _units = new HashSet<int>();
        }

        public UnitSet(IEnumerable<int> units) : this()
        {
            foreach (var unit in units)
            {
                _units.Add(unit);
            }
        }

        public void Add(IReadOnlyList<int> clauses)
        {
            foreach (var clause in clauses)
            {
                _units.Add(clause);
            }
            _stack.Push(clauses);
        }

        public void Backtrack(int times)
        {
            for (var i = 0; i < times; i++)
            {
                Backtrack();
            }
        }

        public void Backtrack()
        {
            var clauses = _stack.Pop();
            foreach(var clause in clauses)
            {
                _units.Remove(clause);
            }
        }
    }
}
