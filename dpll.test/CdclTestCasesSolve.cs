using cdcl.Algorithm;
using dpll.Algorithm;
using dpll.Reader;
using formula2cnf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dpll.test
{
    public sealed class CdclTestCasesSolve
    {
        private readonly string TestCases = @"..\..\..\TestCases\";
        private readonly string Sat = @"..\..\..\3SAT\";
        private readonly string Unsat = @"..\..\..\UNSAT\";

        [Fact]
        public void BasicTest01()
        {
            var files = Directory.GetFiles(TestCases);
            foreach (var path in files)
            {
                var input = File.OpenRead(path);
                var converter = new Converter(input, false);
                Assert.True(converter.TryConvert(out var cnf, out var comments));
                var sat = new CdclSat(new BasicFormulaPruner(cnf));
                Assert.True(sat.IsSatisfiable());
            }
        }

        [Fact]
        public void BasicTest02()
        {
            var files = Directory.GetFiles(Sat);
            foreach (var path in files)
            {
                var input = File.OpenRead(path);
                var converter = new DimacsReader(input);
                Assert.True(converter.TryRead(out var cnf));
                var sat = new CdclSat(new BasicFormulaPruner(cnf));
                Assert.True(sat.IsSatisfiable());
            }
        }

        [Fact]
        public void BasicTest03()
        {
            var files = Directory.GetFiles(Unsat);
            foreach (var path in files)
            {
                var input = File.OpenRead(path);
                var converter = new DimacsReader(input);
                Assert.True(converter.TryRead(out var cnf));
                var sat = new CdclSat(new BasicFormulaPruner(cnf));
                Assert.False(sat.IsSatisfiable());
            }
        }
    }
}
