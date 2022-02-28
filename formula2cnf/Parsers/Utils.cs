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
        public static IEnumerable<Token<T>> MakeTokens<T>(string text, T type, IEnumerable<TextPosition> positions)
        {
            foreach(var position in positions)
            {
                yield return new Token<T>(type, position, text[position.Position..position.Length]);
            }
        }

        public static IEnumerable<Token<T>> MakeToken<T>(string text, T type, TextPosition position)
        {
            yield return new Token<T>(type, position, text[position.Position..position.Length]);
        }

        public static IEnumerable<Token<T>> MakeNone<T>()
        {
            return Enumerable.Empty<Token<T>>();
        }
    }
}
