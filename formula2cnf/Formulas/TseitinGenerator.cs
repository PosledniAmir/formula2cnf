using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal class TseitinGenerator
    {
        private readonly Dictionary<Node, int> _implicit;
        private readonly Dictionary<string, int> _explicit;
        private int _last;
        private bool _implication;

        public TseitinGenerator(bool implication = false)
        {
            _implicit = new Dictionary<Node, int>();
            _explicit = new Dictionary<string, int>();
            _last = 0;
            _implication = implication;
        }

        public IEnumerable<IClauseGenerator> Generate(Node root)
        {
            _implicit.Clear();
            _explicit.Clear();
            _last = 0;

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

                if (node.Type != Node.NodeType.Variable && node.Type != Node.NodeType.Not)
                {
                    yield return BuildClause(node);
                }
            }
        }

        private IClauseGenerator CreateClause(int value, IConsumer consumer)
        {
            if (_implication)
            {
                return new Implication(value, consumer);
            }
            else
            {
                return new Equivalence(value, consumer);
            }
        }

        private IClauseGenerator BuildClause(Node node)
        {
            var value = GetVariable(node);
            var first = 0;
            var second = 0;

            if (node.Children.Count > 0)
            {
                first = GetVariable(node.Children[0]);
            }

            if (node.Children.Count > 1)
            {
                second = GetVariable(node.Children[1]);
            }

            if (node.Type == Node.NodeType.And && node.Children.Count == 2)
            {
                return CreateClause(value, new And(first, second));
            }
            else if (node.Type == Node.NodeType.Or && node.Children.Count == 2)
            {
                return CreateClause(value, new Or(first, second));
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private int GetVariable(Node node)
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

        private void AddVariable(Node node)
        {
            int value;
            if (node.Parent != null && node.Parent.Type == Node.NodeType.Not)
            {
                value = -GetVariable(node.Parent);
            }
            else
            {
                value = ++_last;
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
        }
    }
}
