using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Tokens
{
    public enum TokenType : byte { LeftBracket, RightBracket, And, Or, Not, Variable, Whitespace }
}
