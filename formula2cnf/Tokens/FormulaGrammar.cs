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
            var open = new StringParser<TokenType>("(", TokenType.LeftBracket);
            var close = new StringParser<TokenType>(")", TokenType.RightBracket);
            var variable = new NonWhitespaceParser<TokenType>(TokenType.Variable);
            var whitespace = new WhitespaceParser<TokenType>(TokenType.Whitespace);
            var formulaRecursion = new RecursiveParser<TokenType>();
            var not = new SequenceParser<TokenType>(new StringParser<TokenType>("not", TokenType.Not), whitespace, variable);
            var or = new SequenceParser<TokenType>(new StringParser<TokenType>("or", TokenType.Or), whitespace, formulaRecursion, whitespace, formulaRecursion);
            var and = new SequenceParser<TokenType>(new StringParser<TokenType>("and", TokenType.And), whitespace, formulaRecursion, whitespace, formulaRecursion);
            var formula = new SequenceParser<TokenType>(open, new OneOfParser<TokenType>(and, or, not, variable), close);
            formulaRecursion.Bind(formula);
            _grammar = formula;
        }

        public bool TryParse(string text, out IEnumerable<Token<TokenType>> tokens)
        {
            return _grammar.TryParse(text, 0, out tokens);
        }
    }
}
