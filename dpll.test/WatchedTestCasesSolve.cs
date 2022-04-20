using dpll.Algorithm;
using dpll.Reader;
using System.IO;
using watched.Algorithm;
using Xunit;
using formula2cnf;

namespace dpll.test
{
    public sealed class WatchedTestCasesSolve
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
                var sat = new DpllSat(new WatchedChecker(new WatchedFormula(cnf)));
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
                var sat = new DpllSat(new WatchedChecker(new WatchedFormula(cnf)));
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
                var sat = new DpllSat(new WatchedChecker(new WatchedFormula(cnf)));
                Assert.False(sat.IsSatisfiable());
            }
        }
    }
}
