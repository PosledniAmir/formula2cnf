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

        private int AddSubOccurence(List<Token<T>> tokens, IEnumerable<Token<T>> occurence)
        {
            var position = 0;

            foreach (var item in occurence)
            {
                position += item.Position.Length;
                tokens.Add(item);
            }

            return position;
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
                    position += AddSubOccurence(tokens, subOccurence);
                }
            }

            occurence = tokens;
            return true;
        }
    }
}
