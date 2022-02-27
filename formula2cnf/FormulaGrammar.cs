using formula2cnf.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf
{
    internal sealed class FormulaGrammar
    {
        private readonly IParser _grammar;

        public FormulaGrammar()
        {
            var open = new StringParser("(", Token.TokenType.LeftBracket);
            var close = new StringParser(")", Token.TokenType.RightBracket);
            var variable = new NonWhitespaceParser(Token.TokenType.Variable);
            var whitespace = new WhitespaceParser(Token.TokenType.Whitespace);
            var formulaRecursion = new RecursiveParser();
            var not = new SequenceParser(new StringParser("not", Token.TokenType.Not), whitespace, variable);
            var or = new SequenceParser(new StringParser("or", Token.TokenType.Or), whitespace, formulaRecursion, whitespace, formulaRecursion);
            var and = new SequenceParser(new StringParser("and", Token.TokenType.And), whitespace, formulaRecursion, whitespace, formulaRecursion);
            var formula = new SequenceParser(open, new OneOfParser(and, or, not, variable), close);
            formulaRecursion.Bind(formula);
            _grammar = formula;
        }

        public bool TryParse(string text, out IEnumerable<Token> tokens)
        {
            return _grammar.TryParse(text, 0, out tokens);
        }
    }
}
