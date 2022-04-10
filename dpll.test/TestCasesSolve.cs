using dpll.Algorithm;
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
    public sealed class TestCasesSolve
    {
        private readonly string TestCases = @"..\..\..\TestCases\";

        [Fact]
        public void BasicTest01()
        {
            var dict = new Dictionary<string, bool>();
            var files = Directory.GetFiles(TestCases);
            foreach (var path in files)
            {
                var input = File.OpenRead(path);
                var converter = new Converter(input, false);
                Assert.True(converter.TryConvert(out var cnf, out var comments));
                var sat = new DpllSat(cnf);
                Assert.True(sat.IsSatisfiable());
            }
        }
    }
}
