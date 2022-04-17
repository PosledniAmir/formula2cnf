using dpll.Algorithm;
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
    public sealed class WatchedSatTest
    {
        public bool Solve(CnfFormula formula)
        {
            var sat = new DpllSat(new WatchedClauseChecker(formula));
            Assert.True(sat.IsSatisfiable());
            var model = sat.GetModel().ToList();
            var checker = new ClauseChecker(formula);

            foreach (var item in model)
            {
                checker.Satisfy(item);
            }

            return checker.Satisfied;
        }

        [Fact]
        public void BasicTest01()
        {
            var formula = new CnfFormula(new[]
            {
                new []{ 1},
                new []{ -2 },
                new []{ 3},
            });

            Assert.True(Solve(formula));
        }

        [Fact]
        public void BasicTest02()
        {
            var formula = new CnfFormula(new[]
            {
                new []{ -1, -2},
            });

            Assert.True(Solve(formula));
        }

        [Fact]
        public void BasicTest03()
        {
            var formula = new CnfFormula(new[]
            {
                new []{ -1, -2},
                new []{ 1, 2},
                new []{ 2, 3},
                new []{ -2, -3},
            });

            Assert.True(Solve(formula));
        }

        [Fact]
        public void BasicTest04()
        {
            var formula = new CnfFormula(new[]
            {
                new []{ 1, 2, 3},
                new []{ 1, -2, -3},
                new []{ -1, 2, 3},
                new []{ 1, -2, 3},
                new []{ -1, -2, 3},
                new []{ -3, -2 },
            });

            Assert.True(Solve(formula));
        }
    }
}
