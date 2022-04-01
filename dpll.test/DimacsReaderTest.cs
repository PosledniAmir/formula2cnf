using dpll.Reader;
using System.IO;
using System.Text;
using Xunit;

namespace dpll.test
{
    public sealed class DimacsReaderTest
    {
        [Fact]
        public void BasicTest01()
        {
            var input = @"c
c start with comments
c
c 
p cnf 5 3
1 -5 4 0
-1 5 3 4 0
-3 -4 0";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var reader = new DimacsReader(stream);
            Assert.True(reader.TryRead(out var cnf));
            Assert.Equal(@"p cnf 5 3
1 -5 4 0
-1 5 3 4 0
-3 -4 0", cnf.ToString());
        }
    }
}
