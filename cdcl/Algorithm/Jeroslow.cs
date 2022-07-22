using dpll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cdcl.Algorithm
{
    internal sealed class Jeroslow : IVariableDecider
    {
        private readonly IFormulaPruner _formula;
        private readonly SortedDictionary<int, int> _variables;
        private readonly Dictionary<int, int> _map;
        private readonly Stack<Tuple<int, int>> _stack;

        private static SortedDictionary<int, int> ComputeOrder(IFormulaPruner formula)
        {
            var max = formula.Clauses;
            var map = new Dictionary<int, decimal>();

            for (var i = 0; i < max; i++)
            {
                var literals = formula.Literals(i).ToList();
                decimal temp = 1;
                for (var j = 0; j < literals.Count; j++)
                {
                    temp /= 2;
                }

                foreach (var literal in literals)
                {
                    var variable = Math.Abs(literal);
                    if (map.TryGetValue(variable, out var score))
                    {
                        map[variable] = score + temp;
                    }
                    else
                    {
                        map[variable] = temp;
                    }
                }
            }

            var sorted = new SortedDictionary<int, int>();
            var index = 0;
            foreach (var variable in map.OrderByDescending(kv => kv.Value).Select(kv => kv.Key))
            {
                sorted.Add(index++, variable);
            }

            return sorted;
        }

        public Jeroslow(IFormulaPruner formula)
        {
            _formula = formula;
            _variables = ComputeOrder(formula);
            _stack = new Stack<Tuple<int, int>>();
            _map = new Dictionary<int, int>();
            foreach(var (score, variable) in _variables)
            {
                _map[variable] = score;
            }
        }

        public int Decide()
        {
            if (_variables.Count == 0)
            {
                return 0;
            }
            else
            {
                var (_, value) = _variables.First();
                return -value;
            }
        }

        public void Update(int value)
        {
            value = Math.Abs(value);
            var score = _map[value];
            _variables.Remove(score);
            _stack.Push(Tuple.Create(score, value));
        }

        public void Backtrack()
        {
            var (score, variable) = _stack.Pop();
            _variables.Add(score, variable);
        }

        public void Reset()
        {
            _stack.Clear();
            _variables.Clear();
            _map.Clear();

            foreach (var (score, item) in ComputeOrder(_formula))
            {
                _variables.Add(score, item);
            }

            foreach (var (score, variable) in _variables)
            {
                _map[variable] = score;
            }
        }

        public void Learn(IEnumerable<int> clause)
        {
            //do nothing
        }
    }
}
