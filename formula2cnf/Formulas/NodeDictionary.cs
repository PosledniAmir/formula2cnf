using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal class NodeDictionary
    {
        private readonly Dictionary<Node, int> _implicit;
        private readonly Dictionary<string, int> _explicit;
        private int _count;

        public NodeDictionary()
        {
            _implicit = new Dictionary<Node, int>();
            _explicit = new Dictionary<string, int>();
            _count = 0;
        }

        public void Clear()
        {
            _implicit.Clear();
            _explicit.Clear();
            _count = 0;
        }

        public void AddVariables(Node root)
        {
            Clear();

            var stack = new Stack<Node>();
            AddVariable(root);
            stack.Push(root);

            while (stack.Count > 0)
            {
                var node = stack.Pop();

                foreach (var item in node.Children)
                {
                    AddVariable(item);
                    stack.Push(item);
                }
            }
        }

        public int GetVariable(Node node)
        {
            if (node.Type == Node.NodeType.Variable && node.Value != null)
            {
                return _explicit[node.Value];
            }
            else if (node.Type != Node.NodeType.Variable)
            {
                return _implicit[node];
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public bool TryGetVariable(Node node, out int key)
        {
            if (node.Type == Node.NodeType.Variable && node.Value != null)
            {
                return _explicit.TryGetValue(node.Value, out key);
            }
            else if (node.Type != Node.NodeType.Variable)
            {
                return _implicit.TryGetValue(node, out key);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private void AddVariable(Node node)
        {
            if (node.Type == Node.NodeType.Not)
            {
                return;
            }

            int value;
            if (!TryGetVariable(node, out value))
            {
                value = ++_count;
            }

            if (node.Type == Node.NodeType.Variable && node.Value != null)
            {
                _explicit.TryAdd(node.Value, value);
            }
            else if (node.Type != Node.NodeType.Variable)
            {
                _implicit.TryAdd(node, value);
            }
            else
            {
                throw new ArgumentException();
            }

            while (node.Parent != null && node.Parent.Type == Node.NodeType.Not)
            {
                value = -value;
                _implicit.TryAdd(node.Parent, value);
                node = node.Parent;
            }
        }
    }
}
