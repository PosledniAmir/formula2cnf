using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Parsers
{
    internal sealed class StringParser : IParser
    {
        public readonly string Value;
        private readonly Token.TokenType _type;

        public StringParser(string value, Token.TokenType type)
        {
            Value = value;
            _type = type;
        }

        public bool TryParse(string text, int position, out IEnumerable<Token> occurence)
        {
            bool notFound = false;
            int i = 0; ;
            for (i = 0; i < Value.Length; i++)
            {
                if (position + i >= text.Length)
                {
                    notFound = true;
                    break;
                }
                if (text[position + i] != text[i])
                {
                    notFound = true;
                    break;
                }
            }

            if (notFound)
            {
                occurence = Utils.MakeToken(text, _type, new TextPosition(position + i, i));
                return false;
            }
            else
            {
                occurence = Utils.MakeNone();
                return true;
            }
        }
    }
}
