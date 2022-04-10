using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal sealed class ClauseGenerator
    {
        private readonly NodeDictionary _nodeDictionary;
        private readonly bool _implication;

        public int First => _nodeDictionary.First;
        public int Count => _nodeDictionary.Count;
        public IEnumerable<Tuple<int, string>> NamedVariables => _nodeDictionary.GetNamedVariables();

        public ClauseGenerator(bool implication = false)
        {
            _nodeDictionary = new NodeDictionary();
            _implication = implication;
        }

        public IEnumerable<IClauseGenerator> Generate(Node root)
        {
            _nodeDictionary.Clear();
            _nodeDictionary.AddVariables(root);

            var stack = new Stack<Node>();
            stack.Push(root);
            yield return new First(_nodeDictionary.First);

            while (stack.Count > 0)
            {
                var node = stack.Pop();

                foreach (var item in node.Children)
                {
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
            var value = _nodeDictionary.GetVariable(node);
            var first = 0;
            var second = 0;

            if (node.Children.Count > 0)
            {
                first = _nodeDictionary.GetVariable(node.Children[0]);
            }

            if (node.Children.Count > 1)
            {
                second = _nodeDictionary.GetVariable(node.Children[1]);
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
    }
}
