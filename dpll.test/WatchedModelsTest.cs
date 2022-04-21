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
    public sealed class WatchedModelsTest
    {
        [Fact]
        public void BasicTest01()
        {
            var formula = new CnfFormula(new[]
            {
                new []{ 1, 2 },
                new []{-1, -2 }
            });

            var dpll = new DpllSat(new WatchedChecker(new WatchedFormula(formula)));
            var list = dpll.GetModels().ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(1, list[0]);
            Assert.Contains(-2, list[0]);
            Assert.Contains(-1, list[1]);
            Assert.Contains(2, list[1]);
        }

        [Fact]
        public void BasicTest02()
        {
            var formula = new CnfFormula(new[]
            {
                new []{ 1, 2, 3 },
                new []{-1, -2, -3},
            });

            var dpll = new DpllSat(new WatchedChecker(new WatchedFormula(formula)));
            var list = dpll.GetModels().ToList();
            Assert.Equal(6, list.Count);
        }

        [Fact]
        public void BasicTest03()
        {
            var formula = new CnfFormula(new[]
            {
                new []{ 1, 2, 3 },
                new []{-1, -2, -3},
                new []{ 1, 2, 3 },
                new []{-1, -2, -3},
            });

            var dpll = new DpllSat(new WatchedChecker(new WatchedFormula(formula)));
            var list = dpll.GetModels().ToList();
            Assert.Equal(6, list.Count);
        }
    }
}
