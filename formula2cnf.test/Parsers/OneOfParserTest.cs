using formula2cnf.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace formula2cnf.test.Parsers
{
    public class OneOfParserTest
    {
        [Fact]
        public void BasicTest01()
        {
            var left = new StringParser<bool>("ahoj", true);
            var right = new StringParser<bool>("hello", false);
            var parser = new OneOfParser<bool>(left, right);
            Assert.True(parser.TryParse("hello world!", 0, out var tokens));
            Assert.Single(tokens);
            var first = tokens.First();
            Assert.Equal("hello", first.Value);
            Assert.Equal(0, first.Position.Position);
            Assert.Equal(5, first.Position.Length);

            Assert.True(parser.TryParse("ahoj světe!", 0, out tokens));
            Assert.Single(tokens);
            first = tokens.First();
            Assert.Equal("ahoj", first.Value);
            Assert.Equal(0, first.Position.Position);
            Assert.Equal(4, first.Position.Length);
        }

        [Fact]
        public void BasicTest02()
        {
            var left = new StringParser<bool>("ahoj", true);
            var right = new StringParser<bool>("hello", false);
            var parser = new OneOfParser<bool>(left, right);
            Assert.False(parser.TryParse("not hello world!", 0, out var tokens));
            Assert.Empty(tokens);
        }
    }
}
