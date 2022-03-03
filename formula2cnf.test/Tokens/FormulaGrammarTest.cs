using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace formula2cnf.test.Tokens
{
    public class FormulaGrammarTest
    {
        [Fact]
        public void BasicTest01()
        {
            var grammar = new FormulaGrammar();
            Assert.True(grammar.TryParse("(or a1 a2)", out var tokens));
            var list = tokens.ToList();
            Assert.Equal(7, list.Count);
            Assert.Equal(TokenType.LeftBracket, list[0].Type);
            Assert.Equal(TokenType.Or, list[1].Type);
            Assert.Equal(TokenType.Whitespace, list[2].Type);
            Assert.Equal(TokenType.Variable, list[3].Type);
            Assert.Equal(TokenType.Whitespace, list[4].Type);
            Assert.Equal(TokenType.Variable, list[5].Type);
            Assert.Equal(TokenType.RightBracket, list[6].Type);
        }

        [Fact]
        public void BasicTest02()
        {
            var grammar = new FormulaGrammar();
            Assert.True(grammar.TryParse("(or a1 (and a2 a3))", out var tokens));
            var list = tokens.ToList();
            Assert.Equal(13, list.Count);
            Assert.Equal(TokenType.LeftBracket,  list[0].Type);
            Assert.Equal(TokenType.Or,           list[1].Type);
            Assert.Equal(TokenType.Whitespace,   list[2].Type);
            Assert.Equal(TokenType.Variable,     list[3].Type);
            Assert.Equal(TokenType.Whitespace,   list[4].Type);
            Assert.Equal(TokenType.LeftBracket,  list[5].Type);
            Assert.Equal(TokenType.And,          list[6].Type);
            Assert.Equal(TokenType.Whitespace,   list[7].Type);
            Assert.Equal(TokenType.Variable,     list[8].Type);
            Assert.Equal(TokenType.Whitespace,   list[9].Type);
            Assert.Equal(TokenType.Variable,     list[10].Type);
            Assert.Equal(TokenType.RightBracket, list[11].Type);
            Assert.Equal(TokenType.RightBracket, list[12].Type);
        }

        [Fact]
        public void BasicTest03()
        {
            var grammar = new FormulaGrammar();
            Assert.True(grammar.TryParse("(or (and a1 a2) (or a3 a4))", out var tokens));
            var list = tokens.ToList();
            Assert.Equal(19, list.Count);
            Assert.Equal(TokenType.LeftBracket,  list[0].Type);
            Assert.Equal(TokenType.Or,           list[1].Type);
            Assert.Equal(TokenType.Whitespace,   list[2].Type);
                                                 
            Assert.Equal(TokenType.LeftBracket,  list[3].Type);
            Assert.Equal(TokenType.And,          list[4].Type);
            Assert.Equal(TokenType.Whitespace,   list[5].Type);
            Assert.Equal(TokenType.Variable,     list[6].Type);
            Assert.Equal(TokenType.Whitespace,   list[7].Type);
            Assert.Equal(TokenType.Variable,     list[8].Type);
            Assert.Equal(TokenType.RightBracket, list[9].Type);

            Assert.Equal(TokenType.Whitespace,   list[10].Type);
            Assert.Equal(TokenType.LeftBracket,  list[11].Type);
            Assert.Equal(TokenType.Or,           list[12].Type);
            Assert.Equal(TokenType.Whitespace,   list[13].Type);
            Assert.Equal(TokenType.Variable,     list[14].Type);
            Assert.Equal(TokenType.Whitespace,   list[15].Type);
            Assert.Equal(TokenType.Variable,     list[16].Type);
            Assert.Equal(TokenType.RightBracket, list[17].Type);
            Assert.Equal(TokenType.RightBracket, list[18].Type);
        }

        [Fact]
        public void BasicTest04()
        {
            var grammar = new FormulaGrammar();
            Assert.False(grammar.TryParse("(or a1) a2)", out var tokens));
            Assert.Empty(tokens);
        }
    }
}
