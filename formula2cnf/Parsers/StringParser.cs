using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Parsers
{
    internal sealed class StringParser<T> : IParser<T>
    {
        public readonly string Value;
        private readonly T _type;

        public StringParser(string value, T type)
        {
            Value = value;
            _type = type;
        }

        public bool TryParse(string text, int position, out IEnumerable<Token<T>> occurence)
        {
            bool notFound = false;
            int i = 0;
            for (i = 0; i < Value.Length; i++)
            {
                if (position + i >= text.Length)
                {
                    notFound = true;
                    break;
                }
                if (text[position + i] != Value[i])
                {
                    notFound = true;
                    break;
                }
            }

            if (notFound)
            {
                occurence = Utils.MakeNone<T>();
                return false;
            }
            else
            {
                occurence = Utils.MakeToken(text, _type, new TextPosition(position, i));
                return true;
            }
        }
    }
}
