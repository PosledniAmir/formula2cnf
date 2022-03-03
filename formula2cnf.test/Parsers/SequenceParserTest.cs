using formula2cnf.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace formula2cnf.test.Parsers
{
    public class SequenceParserTest
    {
        [Fact]
        public void BasicTest01()
        {
            var hello = new StringParser<bool>("hello", true);
            var whitespace = new WhitespaceParser<bool>(false);
            var world = new StringParser<bool>("world!", true);
            var parser = new SequenceParser<bool>(hello, whitespace, world);
            Assert.True(parser.TryParse("hello world!", 0, out var tokens));
            var list = tokens.ToList();
            Assert.Equal(3, list.Count);
            Assert.Equal("hello", list[0].Value);
            Assert.Equal(0, list[0].Position.Position);
            Assert.Equal(5, list[0].Position.Length);
            Assert.Equal(" ", list[1].Value);
            Assert.Equal(5, list[1].Position.Position);
            Assert.Equal(1, list[1].Position.Length);
            Assert.Equal("world!", list[2].Value);
            Assert.Equal(6, list[2].Position.Position);
            Assert.Equal(6, list[2].Position.Length);
        }

        [Fact]
        public void BasicTest02()
        {
            var hello = new StringParser<bool>("hello", true);
            var whitespace = new WhitespaceParser<bool>(false);
            var world = new StringParser<bool>("world!", true);
            var parser = new SequenceParser<bool>(hello, whitespace, world);
            Assert.False(parser.TryParse("hello not world!", 0, out var tokens));
            Assert.Empty(tokens);
        }
    }
}
