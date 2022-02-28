using formula2cnf.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace formula2cnf.test.Parsers
{
    public class StringParserTest
    {
        [Fact]
        public void BasicTest01()
        {
            var parser = new StringParser<bool>("hello ", true);
            Assert.True(parser.TryParse("hello world!", 0, out var tokens));
            Assert.Single(tokens);
            var first = tokens.First();
            Assert.Equal("hello ", first.Value);
            Assert.Equal(0, first.Position.Position);
            Assert.Equal(6, first.Position.Length);
        }

        [Fact]
        public void BasicTest02()
        {
            var parser = new StringParser<bool>("hello ", true);
            Assert.False(parser.TryParse("not so hello world!", 0, out var tokens));
            Assert.Empty(tokens);
        }
    }
}
