using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics.Parsing
{
    internal static class Tokenizer
    {
        public static IEnumerable<Tuple<Token, string>> TokenizeInstance(string equation)
        {
            var upper = equation.ToUpper();
            return ParseInstancePrivate(upper).Where(t => t.Item2.Length > 0);
        }

        private static IEnumerable<Tuple<Token, string>> ParseInstancePrivate(string equation)
        {
            string word;
            var token = new StringBuilder();
            foreach (char c in equation)
            {
                if (char.IsWhiteSpace(c))
                {
                    word = token.ToString();
                    yield return Tuple.Create(Determine(word), word);
                    token.Clear();

                }
                else if (c == '+' || c == '-' || c == '=' || c == '(' || c == ')')
                {
                    word = token.ToString();
                    yield return Tuple.Create(Determine(word), word);
                    token.Clear();

                    yield return Tuple.Create(Determine(c), c.ToString());
                }
                else if (char.IsLetter(c) || c == '|' | c == '&')
                {
                    token.Append(c);
                }
                else
                {
                    token.Append(c);
                    yield return Tuple.Create(Token.Error, token.ToString());
                    yield break;
                }
            }

            word = token.ToString();
            yield return Tuple.Create(Determine(word), word);
        }

        private static Token Determine(char c)
        {
            if (c == '-')
            {
                return Token.Minus;
            }
            else if (c == '+')
            {
                return Token.Plus;
            }
            else if (c == '=')
            {
                return Token.Equal;
            }
            else if (c == '(')
            {
                return Token.LeftBracket;
            }
            else if (c == ')')
            {
                return Token.RightBracket;
            }
            else
            {
                return Token.Error;
            }
        }

        private static Token Determine(string word)
        {
            if (word == "&&")
            {
                return Token.And;
            }
            else if (word == "||")
            {
                return Token.Or;
            }
            else
            {
                return Token.Word;
            }
        }
    }
}
