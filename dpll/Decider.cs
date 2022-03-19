using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll
{
    internal class Decider
    {
        private readonly Stack<Tuple<int, int>> _decisions;
        private readonly Dictionary<int, HashSet<int>> _possibilities;
        private readonly HashSet<int> _decided;
        private readonly CnfFormula _formula;
        private int _locked;

        public Decider(CnfFormula formula)
        {
            _formula = formula;
            _locked = 1;
            _decisions = new Stack<Tuple<int, int>>();
            _possibilities = new Dictionary<int, HashSet<int>>();
            for (var i = 1; i <= formula.Clauses; i++)
            {
                var set = new HashSet<int>();

                foreach (var item in formula.Formula[i])
                {
                    set.Add(item);
                }

                _possibilities[i] = set;

            }

            _decided = new HashSet<int>();
        }

        public bool TryDecide(int clause)
        {
            var result = false;

            foreach(var item in _possibilities[clause])
            {
                if (!_decided.Contains(item) && !_decided.Contains(-item))
                {
                    _decisions.Push(Tuple.Create(clause, item));
                    _decided.Add(item);
                    _possibilities[clause].Remove(item);
                    result = true;
                    break;
                }
            }

            return result;
        }

        private void FillAgain(int clause)
        {
            var set = new HashSet<int>();

            foreach (var item in _formula.Formula[clause])
            {
                set.Add(item);
            }

            _possibilities[clause] = set;
        }

        private int BacktrackAboveLocked()
        {
            while (_decisions.Count > _locked)
            {
                var (clause, item) = _decisions.Pop();
                _decided.Remove(item);
                if (_possibilities[clause].Count > 0)
                {
                    return clause;
                }
                else
                {
                    FillAgain(clause);
                }
            }

            return -1;
        }

        public int Backtrack()
        {
            var clause = BacktrackAboveLocked();
            if (clause >= 0)
            {
                return clause;
            }

            (clause, var item) = _decisions.Peek();

            if (_possibilities[clause].Count == 1)
            {
                _locked++;
            }
            if (_possibilities[clause].Count == 0)
            {
                return 0;
            }

            _decisions.Pop();
            _decided.Remove(item);
            return clause;
        }

        public IEnumerable<int> GetModel()
        {
            return _decided.OrderBy(i => Math.Abs(i));
        }
    }
}
