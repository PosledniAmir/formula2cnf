using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace formula2cnf.test.Tokens
{
    public class FormulaTreeBuilderTest
    {
        [Fact]
        public void BasicTest01()
        {
            var grammar = new FormulaGrammar();
            Assert.True(grammar.TryParse("(or a1 a2)", out var tokens));
            var list = tokens.ToList();
            Assert.Equal(7, list.Count);
            var builder = new FormulaTreeBuilder();
            foreach (var item in list)
            {
                Assert.True(builder.TryParse(item));
            }
            Assert.Equal("(or a1 a2)", builder.Root?.ToString());
        }

        [Fact]
        public void BasicTest02()
        {
            var grammar = new FormulaGrammar();
            Assert.True(grammar.TryParse("(or a1 (and a2 a3))", out var tokens));
            var list = tokens.ToList();
            Assert.Equal(13, list.Count);
            var builder = new FormulaTreeBuilder();
            foreach (var item in list)
            {
                Assert.True(builder.TryParse(item));
            }
            Assert.Equal("(or a1 (and a2 a3))", builder.Root?.ToString());
        }

        [Fact]
        public void BasicTest03()
        {
            var grammar = new FormulaGrammar();
            Assert.True(grammar.TryParse("(or (and a1 a2) (or a3 a4))", out var tokens));
            var list = tokens.ToList();
            Assert.Equal(19, list.Count);
            var builder = new FormulaTreeBuilder();
            foreach (var item in list)
            {
                Assert.True(builder.TryParse(item));
            }
            Assert.Equal("(or (and a1 a2) (or a3 a4))", builder.Root?.ToString());
        }
    }
}
