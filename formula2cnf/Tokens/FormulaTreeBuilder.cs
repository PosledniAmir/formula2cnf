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
        public Node? Root => _root;

        public FormulaTreeBuilder()
        {
            _root = null;
            _current = null;
        }

        public bool TryParse(Token<TokenType> token)
        {
            return token.Type switch
            {
                TokenType.LeftBracket => TryOpen(),
                TokenType.RightBracket => TryClose(),
                TokenType.And => TrySet(Node.NodeType.And),
                TokenType.Or => TrySet(Node.NodeType.Or),
                TokenType.Not => TrySet(Node.NodeType.Not),
                TokenType.Variable => TryVariable(token),
                TokenType.Whitespace => true,
                _ => throw new NotImplementedException($"Token type {token.Type} is not implemented."),
            };
        }

        private bool TryVariable(Token<TokenType> token)
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

        private bool TryClose()
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

        private bool TryOpen()
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
