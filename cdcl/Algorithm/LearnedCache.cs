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
        private readonly Dictionary<int, HashSet<int>> _learned;

        public LearnedClauseCache(IFormulaPruner formula)
        {
            _formula = formula;
            _learned = new Dictionary<int, HashSet<int>>();
        }

        public void Learned(int clause)
        {
            _learned.Add(clause, _formula.Literals(clause).ToHashSet());
        }

        public List<int> Reset()
        {
            var toRemove = new List<int>(_learned.Count);
            for (var i = 0; i < _formula.Clauses; i++)
            {
                if (_learned.ContainsKey(i))
                {
                    continue;
                }
                else
                {
                    var literals = _formula.Literals(i).ToHashSet();
                    foreach(var (_, set) in _learned)
                    {
                        if (set.All(l => literals.Contains(l)))
                        {
                            toRemove.Add(i);
                        }
                    }
                    
                }
            }
            _learned.Clear();
            return toRemove;
        }
    }
}
