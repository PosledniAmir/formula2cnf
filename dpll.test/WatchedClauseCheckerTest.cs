using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watched.Algorithm;
using Xunit;

namespace dpll.test
{
    public sealed class WatchedClauseCheckerTest
    {
        [Fact]
        public void BasicTest01()
        {
            var formula = new CnfFormula(new[]
            {
                new [] {1, -1, 2, -2, 3, -3, 4, -4},
                new [] {2, -1, -3, -4},
                new [] {3, -1, -2, -4},
                new [] {4, -1, -2, -3}
            });
            var checker = new WatchedChecker(new WatchedFormula(formula));
            Assert.True(checker.Satisfy(1));
            Assert.True(checker.Satisfied);
            checker.Backtrack();
            Assert.True(checker.Satisfy(-1));
            Assert.True(checker.Satisfied);
            checker.Backtrack();
            Assert.True(checker.Satisfy(2));
            Assert.True(checker.Satisfied);
            checker.Backtrack();
            Assert.True(checker.Satisfy(-2));
            Assert.True(checker.Satisfied);
            checker.Backtrack();
            Assert.True(checker.Satisfy(3));
            Assert.True(checker.Satisfied);
            checker.Backtrack();
            Assert.True(checker.Satisfy(-3));
            Assert.True(checker.Satisfied);
            checker.Backtrack();
            Assert.True(checker.Satisfy(4));
            Assert.True(checker.Satisfied);
            checker.Backtrack();
            Assert.True(checker.Satisfy(-4));
            Assert.True(checker.Satisfied);
            checker.Backtrack();
        }
    }
}
