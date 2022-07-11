using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpll.Reader
{
    public static class CnfFileReader
    {
        public static FormulaType DetermineType(string file)
        {
            var type = FormulaType.Error;

            var info = new FileInfo(file);
            if (info.Extension == ".cnf")
            {
                type = FormulaType.Dimacs;
            }
            else if (info.Extension == ".sat")
            {
                type = FormulaType.Smt;
            }

            return type;
        }

        public static bool TryParse(string file, out CnfFormula? formula, out VariableDescriptor? description)
        {
            var type = DetermineType(file);

            try
            {
                var input = File.Open(file, FileMode.Open, FileAccess.Read);
                return CnfStreamReader.TryParse(input, type, out formula, out description);
            } 
            catch
            {
                formula = null;
                description = null;
                return false;
            }
        }
    }
}
