using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Parsers
{
    internal sealed class WhitespaceParser<T> : IParser<T>
    {
        private readonly T _type;

        public WhitespaceParser(T type)
        {
            _type = type;
        }

        public bool TryParse(string text, int position, out IEnumerable<Token<T>> occurence)
        {
            var current = position;
            while((current < text.Length) && char.IsWhiteSpace(text[current]))
            {
                current++;
            }

            if (current != position)
            {
                occurence = Utils.MakeToken(text, _type, new TextPosition(position, current - position));
                return true;
            }
            else
            {
                occurence = Utils.MakeNone<T>();
                return false;
            }
        }
    }
}
