using formula2cnf.Formulas;
using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace formula2cnf.test.Formulas
{
    public class TseitinGeneratorTester
    {
        private static Node GenerateTree(string formula)
        {
            var grammar = new FormulaGrammar();
            Assert.True(grammar.TryParse(formula, out var tokens));
            var builder = new FormulaTreeBuilder();
            foreach (var item in tokens)
            {
                Assert.True(builder.TryParse(item));
            }
            Assert.NotNull(builder.Root);
#pragma warning disable CS8603 // Possible null reference return.
            return builder.Root;
#pragma warning restore CS8603 // Possible null reference return.
        }

        [Fact]
        public void BasicTest01()
        {
            var generator = new TseitinGenerator();
            var node = GenerateTree("(and a1 (not a1))");
            var result = generator.Generate(node);
            var list = result.ToList();
            Assert.Single(list);
            var encoded = list[0].Generate();
        }
    }
}
