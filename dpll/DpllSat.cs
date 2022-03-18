using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll
{
    internal sealed class DpllSat
    {
        private readonly CnfFormula _formula;
        private readonly Resolutor _resolutor;
        private readonly UnitGuard _unitGuard;
        private readonly Decider _decider;

        public DpllSat(CnfFormula formula)
        {
            _formula = formula;
            _resolutor = new Resolutor(_formula);
            _unitGuard = new UnitGuard(_formula);
            _decider = new Decider(_formula.Variables);
        }


    }
}
