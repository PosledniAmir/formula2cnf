using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dpll.test
{
    public sealed class ResolutorTest
    {
        [Fact]
        public void BasicTest01()
        {
            var formula = new CnfFormula(new []
            { 
                new [] { 1},
                new [] { -1, 2},
                new [] { -1, -2, 3},
                new [] { -2, -3, 4},
                new [] { -4, 5},
                new [] { -5, 1 },
            });

            var resolutor = new Resolutor(formula);

            Assert.True(resolutor.UnitResolute(0));
            Assert.Single(formula.Formula[1]);
            Assert.Contains(2, formula.Formula[1]);
            Assert.Equal(2, formula.Formula[2].Count);
            Assert.Contains(-2, formula.Formula[2]);
            Assert.Contains(3, formula.Formula[2]);

            Assert.True(resolutor.UnitResolute(1));
            Assert.Single(formula.Formula[2]);
            Assert.Contains(3, formula.Formula[2]);
            Assert.Equal(2, formula.Formula[3].Count);
            Assert.Contains(-3, formula.Formula[3]);
            Assert.Contains(4, formula.Formula[3]);

            Assert.True(resolutor.UnitResolute(2));
            Assert.Single(formula.Formula[3]);
            Assert.Contains(4, formula.Formula[3]);

            Assert.True(resolutor.UnitResolute(3));
            Assert.Single(formula.Formula[4]);
            Assert.Contains(5, formula.Formula[4]);

            Assert.True(resolutor.UnitResolute(4));
            Assert.Single(formula.Formula[5]);
            Assert.Contains(1, formula.Formula[5]);

            resolutor.Backtrack();
            Assert.Equal(2, formula.Formula[5].Count);
            Assert.Contains(-5, formula.Formula[5]);
            Assert.Contains(1, formula.Formula[5]);

            resolutor.Backtrack();
            Assert.Equal(2, formula.Formula[4].Count);
            Assert.Contains(-4, formula.Formula[4]);
            Assert.Contains(5, formula.Formula[4]);

            resolutor.Backtrack();
            Assert.Equal(2, formula.Formula[3].Count);
            Assert.Contains(-3, formula.Formula[3]);
            Assert.Contains(4, formula.Formula[3]);

            resolutor.Backtrack();
            resolutor.Backtrack();
            Assert.Equal(3, formula.Formula[2].Count);
            Assert.Contains(-1, formula.Formula[2]);
            Assert.Contains(-2, formula.Formula[2]);
            Assert.Contains(3, formula.Formula[2]);
        }
    }
}
