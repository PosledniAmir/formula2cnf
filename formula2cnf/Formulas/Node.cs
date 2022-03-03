using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal sealed class Node
    {
        public enum NodeType : byte { And, Or, Not, Variable, Invalid }
        private string? _value;
        private readonly List<Node> _children;
        private NodeType _type;
        private Node? _parent;
        public NodeType Type => _type;
        public IReadOnlyList<Node> Children => _children;
        public Node? Parent => _parent;
        public string? Value => _value;

        public Node()
        {
            _parent = null;
            _value = null;
            _children = new List<Node>();
            _type = NodeType.Invalid;
        }

        private Node(Node parent) : this()
        {
            _parent = parent;
        }

        public bool TrySetType(NodeType type)
        {
            if (_type == NodeType.Invalid)
            {
                _type = type;
                return true;
            }
            return false;
        }

        public void SetVariable(string variable)
        {
            var child = AddChild();
            child._type = NodeType.Variable;
            child._value = variable;
        }

        public Node AddChild()
        {
            var child = new Node(this);
            _children.Add(child);
            return child;
        }

        public override string ToString()
        {
            
            var first = NodeType.Invalid.ToString();
            var second = NodeType.Invalid.ToString();

            if (_children.Count > 0)
            {
                first = _children[0].ToString();
            }

            if (_children.Count > 1)
            {
                second = _children[1].ToString();
            }

            return _type switch
            {

                NodeType.And => $"(and {first} {second})",
                NodeType.Or => $"(or {first} {second})",
                NodeType.Not => $"(not {first}",
                NodeType.Variable => _value ?? NodeType.Invalid.ToString(),
                NodeType.Invalid => NodeType.Invalid.ToString(),
                _ => _type.ToString(),
            };
        }
    }
}
