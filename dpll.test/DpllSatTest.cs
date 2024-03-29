﻿using dpll.Algorithm;
using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dpll.test
{
    public sealed class DpllSatTest
    {
        public bool Solve(CnfFormula formula)
        {
            var sat = new DpllSat(new BasicFormulaPruner(formula));
            Assert.True(sat.IsSatisfiable());
            var model = sat.GetModels().First();
            var checker = new ClauseChecker(new BasicFormulaPruner(formula), new VariableDecider(formula.Variables));

            foreach (var item in model)
            {
                checker.Satisfy(item, 0);
            }

            return checker.Satisfied;
        }

        [Fact]
        public void BasicTest01()
        {
            var formula = new CnfFormula(new []
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
    }
}
