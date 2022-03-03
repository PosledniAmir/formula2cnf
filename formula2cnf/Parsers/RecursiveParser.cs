using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Parsers
{
    internal sealed class RecursiveParser<T> : IParser<T>
    {
        private IParser<T>? _parser;

        public bool TryParse(string text, int position, out IEnumerable<Token<T>> occurence)
        {
            if (_parser != null)
            {
                _parser.TryParse(text, position, out occurence);
                return true;
            }
            else
            {
                throw new ArgumentException("Unbound recursive parser found");
            }
        }

        public void Bind(IParser<T> parser)
        {
            Interlocked.CompareExchange(ref _parser, parser, null);
        }
    }
}
