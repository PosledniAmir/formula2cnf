using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Algorithm
{
    public interface IVariableDecider
    {
        public int Decide();
        public void Update(int variable);
        public void Backtrack();
        public void Reset();
    }
}
