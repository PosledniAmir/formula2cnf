using formula2cnf;
using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Reader
{
    public static class CnfStreamReader
    {
        public static bool TryParse(Stream input, FormulaType type, out CnfFormula? formula, out VariableDescriptor? description)
        {
            if (type == FormulaType.Smt)
            {
                var reader = new Converter(input, false);
                return reader.TryConvert(out formula, out description);
            }
            else if (type == FormulaType.Dimacs)
            {
                var reader = new DimacsReader(input);
                description = null;
                return reader.TryRead(out formula);
            }
            else
            {
                formula = null;
                description = null;
                return false;
            }
        }
    }
}
