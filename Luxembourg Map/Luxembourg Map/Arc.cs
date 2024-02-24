using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxembourg_Map
{
    internal class Arc
    {
        private Node startNode;
        private Node endNode;
        private int length;

        public Arc(Node startNode, Node endNode, int length)
        {
            StartNode = startNode;
            EndNode = endNode;
            Length = length;
        }

        public Node StartNode
        {
            get { return startNode; }
            set { startNode = value; }
        }

        public Node EndNode
        {
            get { return endNode; }
            set { endNode = value; }
        }

        public int Length
        {
            get { return length; }
            set { length = value; }
        }
    }
}
