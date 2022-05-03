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
        private readonly string TestCases = @"..\..\..\TestCases\";

        [Fact]
        public void BasicTest01()
        {
            var input = @"(or a1 a2)";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var converter = new Converter(stream, false);
            Assert.True(converter.TryConvert(out var cnf, out var descriptor));
            Assert.Equal("c Root variable: 1\r\nc Original variables:\r\nc a1 = 2\r\nc a2 = 3\r\n", descriptor.ToString());
            Assert.Equal("p cnf 3 4\r\n1 0\r\n-1 2 3 0\r\n1 -2 0\r\n1 -3 0", cnf.ToString());
        }

        [Fact]
        public void BasicTest02()
        {
            var files = Directory.GetFiles(TestCases);
            foreach (var path in files)
            {
                var input = File.OpenRead(path);
                var converter = new Converter(input, false);
                Assert.True(converter.TryConvert(out var _, out var _));
            }
        }
    }
}
