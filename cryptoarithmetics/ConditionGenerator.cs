using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics
{
    internal sealed class ConditionGenerator
    {
        private readonly FormulaBuilder _builder;
        private readonly ParserResult _result;
        public readonly IReadOnlyDictionary<string, IntExpr> Constants;

        public ConditionGenerator(FormulaBuilder builder, ParserResult result)
        {
            _builder = builder;
            _result = result;
            Constants = CreateConstants(_builder, _result.Words);
        }

        public BoolExpr CreateUniqueCondition()
        {
            return _builder.CreateUniqueConditon(Constants);
        }

        public IEnumerable<BoolExpr> CreateRangeConditions()
        {
            return Constants.Select(pair => _builder.CreateBaseCondition(pair.Value, _result.FirstLetters.Contains(pair.Key)));
        }

        public BoolExpr CreateInstanceCondition()
        {
            throw new NotImplementedException();
        }

        private static Dictionary<string, IntExpr> CreateConstants(FormulaBuilder builder, IEnumerable<string> words)
        {
            return words
                .SelectMany(word => word.AsEnumerable())
                .ToHashSet()
                .Select(c => c.ToString())
                .ToDictionary(key => key, item => builder.CreateConstant(item));
        }
    }
}
