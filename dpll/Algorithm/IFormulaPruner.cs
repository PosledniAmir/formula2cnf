using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public interface IFormulaPruner
    {
        public int Variables { get; }
        public int Clauses { get; }
        public bool IsUnit (int clause);
        public bool IsEmpty (int clause);
        public bool IsSatisfied(int clause, FormulaState model);
        public SatisfyStep Satisfy (int variable, int clause, FormulaState model);
        public IEnumerable<int> Literals(int clause);
        public void Backtrack ();
        public void Backtrack (int times);
        public int AddClause(IEnumerable<int> literals);
    }
}
