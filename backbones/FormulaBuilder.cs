using formula2cnf.Formulas;
using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backbones
{
    internal sealed class FormulaBuilder
    {
        private readonly Context _context;


        public FormulaBuilder(Context context)
        {
            _context = context;
        }

        public Dictionary<int, BoolExpr> GetVariables(CnfFormula formula)
        {
            var variables = new Dictionary<int, BoolExpr>();

            foreach (var variable in Enumerable.Range(1, formula.Variables))
            {
                variables[variable] = _context.MkBoolConst(variable.ToString());
            }

            return variables;
        }

        public BoolExpr GetFormula(CnfFormula formula, IReadOnlyDictionary<int, BoolExpr> variables)
        {
            var clauses = new List<BoolExpr>(formula.Clauses);
            foreach (var clause in formula.Formula)
            {
                var clauseExpr = new List<BoolExpr>(clauses.Count);
                foreach (var literal in clause)
                {
                    if (literal > 0)
                    {
                        clauseExpr.Add(variables[literal]);
                    }
                    else
                    {
                        clauseExpr.Add(_context.MkNot(variables[-literal]));
                    }
                }
                clauses.Add(_context.MkOr(clauseExpr.ToArray()));
            }

            return _context.MkAnd(clauses.ToArray());
        }

        public BoolExpr Forbid(int literal, BoolExpr variable)
        {
            if (literal > 0)
            {
                return _context.MkNot(variable);
            }
            else
            {
                return variable;
            }
        }
    }
}
