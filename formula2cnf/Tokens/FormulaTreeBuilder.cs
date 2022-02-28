using formula2cnf.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Tokens
{
    internal sealed class FormulaTreeBuilder
    {
        private Node? _root;
        private Node? _current;

        public FormulaTreeBuilder()
        {
            _root = null;
            _current = null;
        }

        public bool TryParse(Token token)
        {
            return token.Type switch
            {
                Token.TokenType.LeftBracket => TryOpen(token),
                Token.TokenType.RightBracket => TryClose(token),
                Token.TokenType.And => TrySet(Node.NodeType.And),
                Token.TokenType.Or => TrySet(Node.NodeType.Or),
                Token.TokenType.Not => TrySet(Node.NodeType.Not),
                Token.TokenType.Variable => TryVariable(token),
                Token.TokenType.Whitespace => true,
                _ => throw new NotImplementedException($"Token type {token.Type} is not implemented."),
            };
        }

        private bool TryVariable(Token token)
        {
            if (_current != null)
            {
                _current.SetVariable(token.Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TrySet(Node.NodeType type)
        {
            if (_current != null)
            {
                return _current.TrySetType(type);
            }
            else
            {
                return false;
            }
        }

        private bool TryClose(Token token)
        {
            if (_current != null)
            {
                _current = _current.Parent;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TryOpen(Token token)
        {
            var result = true;

            if (_root == null)
            {
                _root = new Node();
                _current = _root;
            }
            else if (_current != null)
            {
                _current = _current.AddChild();
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}
