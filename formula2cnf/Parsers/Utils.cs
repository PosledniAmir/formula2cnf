using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Parsers
{
    internal static class Utils
    {
        public static IEnumerable<Token> MakeTokens(string text, Token.TokenType type, IEnumerable<TextPosition> positions)
        {
            foreach(var position in positions)
            {
                yield return new Token(type, position, text[position.Position..position.Length]);
            }
        }

        public static IEnumerable<Token> MakeToken(string text, Token.TokenType type, TextPosition position)
        {
            yield return new Token(type, position, text[position.Position..position.Length]);
        }

        public static IEnumerable<Token> MakeNone()
        {
            return Enumerable.Empty<Token>();
        }
    }
}
