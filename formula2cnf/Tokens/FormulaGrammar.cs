using formula2cnf.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Tokens
{
    internal sealed class FormulaGrammar
    {
        private readonly IParser<TokenType> _grammar;

        public FormulaGrammar()
        {
            var recursive = new RecursiveParser<TokenType>();

            var open = new StringParser<TokenType>("(", TokenType.LeftBracket);
            var close = new StringParser<TokenType>(")", TokenType.RightBracket);
            var whitespace = new WhitespaceParser<TokenType>(TokenType.Whitespace);
            var variable = new NonWhitespaceParser<TokenType>(TokenType.Variable, "()");

            var not = new SequenceParser<TokenType>(new StringParser<TokenType>("not", TokenType.Not), whitespace, recursive);
            var or = new SequenceParser<TokenType>(new StringParser<TokenType>("or", TokenType.Or), whitespace, recursive, whitespace, recursive);
            var and = new SequenceParser<TokenType>(new StringParser<TokenType>("and", TokenType.And), whitespace, recursive, whitespace, recursive);

            var formula = new SequenceParser<TokenType>(open, new OneOfParser<TokenType>(and, or, not), close);

            var top = new OneOfParser<TokenType>(formula, variable);
            recursive.Bind(top);

            _grammar = top;
        }

        public bool TryParse(string text, out IEnumerable<Token<TokenType>> tokens)
        {
            return _grammar.TryParse(text, 0, out tokens);
        }
    }
}
