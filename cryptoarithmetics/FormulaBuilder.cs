using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics
{
    internal sealed class FormulaBuilder
    {
        public enum Operation { Plus, Minus };
        public enum ClauseOperation { And, Or };
        private readonly Context _context;
        private readonly int _base;

        public FormulaBuilder(Context context, int k)
        {
            _context = context;
            _base = k;
        }

        public IntExpr GetConstant(string constant)
        {
            return _context.MkIntConst(_context.MkSymbol(constant));
        }

        public BoolExpr ConditionRange(IntExpr constant, bool first)
        {
            var start = first ? 1 : 0;
            return _context.MkAnd(
                _context.MkGe(constant, _context.MkInt(start)),
                _context.MkLt(constant, _context.MkInt(_base)));
        }

        public BoolExpr ConditionUniqueness(IEnumerable<KeyValuePair<string, IntExpr>> constants)
        {
            var stack = new Stack<IntExpr>(constants.Select(pair => pair.Value));
            var pairs = new List<BoolExpr>();

            while (stack.Count > 1)
            {
                var top = stack.Pop();
                foreach (var item in stack)
                {
                    pairs.Add(_context.MkNot(_context.MkEq(top, item)));
                }
            }

            return _context.MkAnd(pairs);
        }

        public ArithExpr ConditionWord(string word, IReadOnlyDictionary<string, IntExpr> constants)
        {
            var reversed = word.Reverse().Select(c => c.ToString()).ToList();
            var multiplication = 1;
            var values = new List<ArithExpr>(reversed.Count);

            foreach (var item in reversed)
            {
                var constant = constants[item];
                values.Add(_context.MkMul(constant, _context.MkInt(multiplication)));
                multiplication *= _base;
            }

            return _context.MkAdd(values);
        }

        public ArithExpr GetWordOperation(Operation operation, params ArithExpr[] exprs)
        {
            return operation switch
            {
                Operation.Plus => _context.MkAdd(exprs),
                Operation.Minus => _context.MkSub(exprs),
                _ => throw new NotImplementedException(),
            };
        }

        public BoolExpr GetClauseOperation(ClauseOperation operation, params BoolExpr[] exprs)
        {
            return operation switch
            {
                ClauseOperation.And => _context.MkAnd(exprs),
                ClauseOperation.Or => _context.MkOr(exprs),
                _ => throw new NotImplementedException(),
            };
        }

        public BoolExpr ConditionEquality(ArithExpr left, ArithExpr right)
        {
            return _context.MkEq(left, right);
        }
    }
}
