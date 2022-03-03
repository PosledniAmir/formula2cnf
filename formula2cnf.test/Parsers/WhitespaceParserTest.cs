using formula2cnf.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace formula2cnf.test.Parsers
{
    public class WhitespaceParserTest
    {
        [Fact]
        public void BasicTest01()
        {
            var parser = new WhitespaceParser<bool>(true);
            Assert.False(parser.TryParse("hello world!", 0, out var tokens));
            Assert.Empty(tokens);
        }

        [Fact]
        public void BasicTest02()
        {
            var parser = new WhitespaceParser<bool>(true);
            Assert.True(parser.TryParse("\r\n\t world!", 0, out var tokens));
            Assert.Single(tokens);
            var first = tokens.First();
            Assert.Equal("\r\n\t ", first.Value);
            Assert.Equal(0, first.Position.Position);
            Assert.Equal(4, first.Position.Length);
        }
    }
}
