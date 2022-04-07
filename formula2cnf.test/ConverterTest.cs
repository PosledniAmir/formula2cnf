using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace formula2cnf.test
{
    public sealed class ConverterTest
    {
        [Fact]
        public void BasicTest01()
        {
            var input = @"(or a1 a2)";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var converter = new Converter(stream, false);
            Assert.True(converter.TryConvert(out var cnf, out var descriptor));
            Assert.Equal("c Root variable: 1\r\nc Original variables:\r\nc a1 = 2\r\nc a2 = 3\r\n", descriptor.ToString());
            Assert.Equal("p cnf 3 3\r\n-1 2 3 0\r\n1 -2 0\r\n1 -3 0", cnf.ToString());
        }
    }
}
