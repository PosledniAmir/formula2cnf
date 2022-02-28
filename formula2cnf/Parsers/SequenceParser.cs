using formula2cnf.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Parsers
{
    internal sealed class SequenceParser<T> : IParser<T>
    {
        public readonly IReadOnlyList<IParser<T>> _parsers;

        public SequenceParser(params IParser<T>[] parsers)
        {
            _parsers = parsers;
        }

        public bool TryParse(string text, int position, out IEnumerable<Token<T>> occurence)
        {
            var tokens = new List<Token<T>>();
            foreach (var parser in _parsers)
            {
                if (!parser.TryParse(text, position, out var subOccurence))
                {
                    occurence = Utils.MakeNone<T>();
                    return false;
                }
                else
                {
                    tokens.AddRange(subOccurence);
                }
            }

            occurence = tokens;
            return true;
        }
    }
}
