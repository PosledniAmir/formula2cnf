﻿using formula2cnf.Formulas;
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

        private static CnfFormula GenerateDimacs(string formula)
        {
            var generator = new ClauseGenerator();
            var node = GenerateTree("(and a1 (not a1))");
            var result = generator.Generate(node);
            var cnf = new CnfFormula(result);
            return cnf;
        }

        [Fact]
        public void BasicTest01()
        {
            var result = GenerateDimacs("(and (a1 (not a1))");
            Assert.Equal(2, result.Variables);
            Assert.Equal(3, result.Clauses);
            var formula = result.Formula;

            Assert.Equal(1, formula[0].Count);
            Assert.Contains(1, formula[0]);

            Assert.Equal(2, formula[1].Count);
            Assert.Contains(-1, formula[1]);
            Assert.Contains(2, formula[1]);

            Assert.Equal(2, formula[2].Count);
            Assert.Contains(-1, formula[2]);
            Assert.Contains(-2, formula[2]);
        }
    }
}
