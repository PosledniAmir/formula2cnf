using dpll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cdcl.Algorithm
{
    internal sealed class LearnedClauseCache
    {
        private readonly IFormulaPruner _formula;
        private Dictionary<int, LearnedClause> _learned;
        private readonly int _clauses;
        private int _limit;
        private int _current;

        public int Count => _learned.Count;

        public void Set(int limit)
        {
            _limit = limit;
            _current = _limit;
        }

        public LearnedClauseCache(IFormulaPruner formula, int limit)
        {
            _formula = formula;
            _learned = new Dictionary<int, LearnedClause>();
            _limit = limit;
            _current = _limit;
            _clauses = _formula.Clauses;
        }

        public void Learned(int clause, LearnedClause data)
        {
            if (data.Clause.Count == 0)
            {
                throw new ArgumentException("Cannot learn empty clause.");
            }

            _learned.Add(clause, data);
        }

        public void Remap(Dictionary<int, int> map)
        {
            var remaped = new Dictionary<int, LearnedClause>();

            foreach(var (key, target) in map)
            {
                if (target < _clauses)
                {
                    continue;
                }
                else if (_learned.TryGetValue(key, out LearnedClause clause))
                {
                    remaped.Add(target, clause);
                }
            }

            _learned = remaped;
        }

        private IEnumerable<int> GetLowLbd()
        {
            if (_learned.Count < _current)
            {
                return Enumerable.Empty<int>();
            }
            else
            {
                _current += _limit;
            }
            var selected = _learned.OrderBy(c => c.Value.Lbd).Where(c => c.Value.Clause.Count > 1).ToList();
            var half = selected.Count / 2;
            return selected.OrderBy(c => c.Value.Lbd).Take(half).Select(c => c.Key);
        }

        public List<int> Reset()
        {
            var toRemove = new List<int>(_learned.Count);
            for (var i = _clauses; i < _formula.Clauses; i++)
            {
                var literals = _formula.Literals(i).ToHashSet();
                foreach(var (k, set) in _learned)
                {
                    if (k == i)
                    {
                        continue;
                    }
                    else if (set.Clause.All(l => literals.Contains(l)))
                    {
                        toRemove.Add(i);
                    }
                }
            }
            
            foreach (var clause in GetLowLbd())
            {
                toRemove.Add(clause);
            }

            return toRemove;
        }
    }
}
