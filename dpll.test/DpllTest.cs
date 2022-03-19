using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dpll.test
{
    public sealed class DpllTest
    {
        [Fact]
        public void BasicTest01()
        {
            var formula = new CnfFormula(new []
            {
                new []{ 1},
                new []{ -2 },
                new []{ 3},
            });

            var sat = new DpllSat(formula);
            Assert.True(sat.IsSatisfiable());
            var model = sat.GetModel().ToList();
            Assert.Equal(3, model.Count);
            Assert.Contains(1, model);
            Assert.Contains(-2, model);
            Assert.Contains(3, model);
        }
    }
}
