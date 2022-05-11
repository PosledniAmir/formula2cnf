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
        private readonly List<int> _toRemove;

        public LearnedClauseCache(IFormulaPruner formula)
        {
            _formula = formula;
            _toRemove = new List<int>();
        }

        public void Learned(int clause)
        {
            var list = _formula.Literals(clause).ToList();
            if (list.Count == 0)
            {
                throw new ArgumentException("Learned clause cannot be empty.");
            }
            for(int test = 0; test < _formula.Clauses; test++)
            {
                var literals = _formula.Literals(test);
                if (list.All(l => literals.Contains(l)))
                {
                    _toRemove.Add(test);
                }
            }
        }

        public IEnumerable<int> Reset()
        {
            foreach (var item in _toRemove)
            {
                yield return item;
            }
            _toRemove.Clear();
        }
    }
}
