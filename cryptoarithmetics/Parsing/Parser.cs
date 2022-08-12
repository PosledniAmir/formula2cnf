using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics.Parsing
{
    internal static class Parser
    {
        public static ParserResult Parse(IEnumerable<Tuple<Token, string>> tokens)
        {
            var list = tokens.ToList();
            var words = new List<string>(list.Count);
            var firstLetters = new HashSet<string>(list.Count);

            foreach (var (token, value) in list)
            {
                if (token == Token.Word)
                {
                    words.Add(value);
                    firstLetters.Add(value.First().ToString());
                }
            }

            return new ParserResult()
            {
                Tokens = list,
                Words = words,
                FirstLetters = firstLetters,
            };
        }
    }
}
