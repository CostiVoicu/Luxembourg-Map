using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxembourg_Map
{
    internal class Node
    {
        private int idNode;
        private Point screenPos;
        private List<int[]> childrenCost;

        public Node()
        {
            IdNode = -1;
            ScreenPos = new Point(-1, -1);
            ChildrenCost = new List<int[]>();
        }
        public Node(int idNode, Point screenPos, List<int[]> childrenCost)
        {
            IdNode = idNode;
            ScreenPos = screenPos;
            ChildrenCost = childrenCost;
        }

        public int IdNode
        {
            get { return idNode; }
            set { idNode = value; }
        }
        public Point ScreenPos
        {
            get { return screenPos; }
            set { screenPos = value; }
        }
        public List<int[]> ChildrenCost
        {
            get { return childrenCost; }
            set { childrenCost = value; }
        }
    }
}
