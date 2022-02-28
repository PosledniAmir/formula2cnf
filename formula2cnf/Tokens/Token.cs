using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Tokens
{
    internal sealed class Token<T>
    {
        public readonly T Type;
        public readonly TextPosition Position;
        public readonly string Value;

        public Token(T type, TextPosition position, string value)
        {
            Type = type;
            Position = position;
            Value = value;
        }
    }
}
