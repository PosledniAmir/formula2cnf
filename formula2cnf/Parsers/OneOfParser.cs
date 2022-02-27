using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Parsers
{
    internal sealed class OneOfParser : IParser
    {
        public readonly IReadOnlyList<IParser> _parsers;

        public OneOfParser(params IParser[] parsers)
        {
            _parsers = parsers;
        }

        public bool TryParse(string text, int position, out IEnumerable<Token> occurence)
        {
            foreach(var parser in _parsers)
            {
                if (parser.TryParse(text, position, out occurence))
                {
                    return true;
                }
            }

            occurence = Utils.MakeNone();
            return false;
        }
    }
}
