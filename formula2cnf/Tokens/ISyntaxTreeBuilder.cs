using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Tokens
{
    internal interface ISyntaxTreeBuilder
    {
        public bool IsDone { get; }
        public bool TryConsume(Token token);
        public bool TryBuild(out Node? node);
    }
}
