using cryptoarithmetics.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptoarithmetics
{
    internal sealed class ParserResult
    {
        public IReadOnlyList<Tuple<Token, string>> Tokens { get; set; } = new List<Tuple<Token, string>>();
        public IReadOnlyList<string> Words { get; set; } = new List<string>();
        public IReadOnlySet<string> FirstLetters { get; set; } = new HashSet<string>();
    }
}
