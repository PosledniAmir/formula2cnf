using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public sealed class DecisionSet
    {
        private readonly Queue<List<int>> _queue;
        public int Count => _queue.Count;

        public DecisionSet()
        {
            _queue = new Queue<List<int>>();
        }

        public DecisionSet(Stack<int> literals)
        {
            _queue = new Queue<List<int>>();
            var list = new List<int>();
            while (literals.Count > 0)
            {
                list.Add(literals.Pop());
                var i = list.Count - 2;
                if (i >= 0)
                {
                    list[i] = -list[i];
                }
                _queue.Enqueue(new List<int>(list));
            }
        }

        public List<int> Pop()
        {
            return _queue.Dequeue();
        }
    }
}
