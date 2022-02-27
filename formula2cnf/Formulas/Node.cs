using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace formula2cnf.Formulas
{
    internal sealed class Node
    {
        public enum NodeType : byte { And, Or, Not, Variable }
        public readonly NodeType Type;
        public readonly IReadOnlyList<Node> Children;

        public Node(NodeType type, IReadOnlyList<Node> children)
        {
            Type = type;
            Children = children;
        }
    }
}
