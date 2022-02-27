using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf
{
    internal sealed class Token
    {
        public enum TokenType : byte { LeftBracket, RightBracket, And, Or, Not, Variable, Whitespace }
        public readonly TokenType Type;
        public readonly TextPosition Position;
        public readonly string Value;

        public Token(TokenType type, TextPosition position, string value)
        {
            Type = type;
            Position = position;
            Value = value;
        }
    }
}
