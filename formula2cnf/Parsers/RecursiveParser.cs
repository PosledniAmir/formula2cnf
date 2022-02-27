using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Parsers
{
    internal sealed class RecursiveParser : IParser
    {
        private IParser? _parser;

        public bool TryParse(string text, int position, out IEnumerable<Token> occurence)
        {
            if (_parser != null)
            {
                return _parser.TryParse(text, position, out occurence);
            }
            else
            {
                throw new ArgumentException("Unbound recursive parser found");
            }
        }

        public void Bind(IParser parser)
        {
            Interlocked.CompareExchange(ref _parser, parser, null);
        }
    }
}
