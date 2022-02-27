using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Parsers
{
    internal interface IParser
    {
        bool TryParse(string text, int position, out IEnumerable<Token> occurence);
    }
}
