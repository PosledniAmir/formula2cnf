using formula2cnf.Parsers;
using System.Linq;
using Xunit;

namespace formula2cnf.test.Parsers
{
    public class NonWhitespaceParserTest
    {
        [Fact]
        public void BasicTest01()
        {
            var parser = new NonWhitespaceParser<bool>(true);
            Assert.True(parser.TryParse("hello world!", 0, out var tokens));
            Assert.Single(tokens);
            var first = tokens.First();
            Assert.Equal("hello", first.Value);
            Assert.Equal(0, first.Position.Position);
            Assert.Equal(5, first.Position.Length);
        }

        [Fact]
        public void BasicTest02()
        {
            var parser = new NonWhitespaceParser<bool>(true);
            Assert.False(parser.TryParse("\r\n\t world!", 0, out var tokens));
            Assert.Empty(tokens);
        }
    }
}
