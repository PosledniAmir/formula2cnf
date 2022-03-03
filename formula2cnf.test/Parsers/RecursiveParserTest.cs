using formula2cnf.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace formula2cnf.test.Parsers
{
    public class RecursiveParserTest
    {
        [Fact]
        public void BasicTest01()
        {
            var left = new StringParser<bool>("(", true);
            var right = new StringParser<bool>(")", true);
            var inside = new RecursiveParser<bool>();
            var parser = new SequenceParser<bool>(left, inside, right);
            inside.Bind(parser);
            Assert.True(parser.TryParse("((()))", 0, out var tokens));
            var list = tokens.ToList();
            Assert.Equal(6, list.Count);
            Assert.Equal("(", list[0].Value);
            Assert.Equal(0,   list[0].Position.Position);
            Assert.Equal(1,   list[0].Position.Length);
            Assert.Equal("(", list[1].Value);
            Assert.Equal(1,   list[1].Position.Position);
            Assert.Equal(1,   list[1].Position.Length);
            Assert.Equal("(", list[2].Value);
            Assert.Equal(2,   list[2].Position.Position);
            Assert.Equal(1,   list[2].Position.Length);
            Assert.Equal(")", list[3].Value);
            Assert.Equal(3,   list[3].Position.Position);
            Assert.Equal(1,   list[3].Position.Length);
            Assert.Equal(")", list[4].Value);
            Assert.Equal(4,   list[4].Position.Position);
            Assert.Equal(1,   list[4].Position.Length);
            Assert.Equal(")", list[5].Value);
            Assert.Equal(5,   list[5].Position.Position);
            Assert.Equal(1,   list[5].Position.Length);
        }

        [Fact]
        public void BasicTest02()
        {
            var left = new StringParser<bool>("(", true);
            var right = new StringParser<bool>(")", true);
            var inside = new RecursiveParser<bool>();
            var parser = new SequenceParser<bool>(left, inside, right);
            inside.Bind(parser);
            Assert.False(parser.TryParse("(((()))(", 0, out var tokens));
            Assert.Empty(tokens);
        }
    }
}
