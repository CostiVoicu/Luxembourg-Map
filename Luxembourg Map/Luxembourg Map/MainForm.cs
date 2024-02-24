using System;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;

namespace Luxembourg_Map
{
    public partial class MainForm : Form
    {
        Graph graph;
        private Bitmap buffer;
        Node startNodePos = new Node(), endNodePos = new Node();
        List<int> prevList;

        public MainForm()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("LuxembourgMap.xml");

            graph = new Graph(doc);
            PrintMap();

            InitializeComponent();
        }

        List<int> dijkstra(Node source)
        {
            List<int> distance = new List<int>(graph.Body.Count());
            List<int> prev = new List<int>(graph.Body.Count());
            SortedSet<int[]> pq = new SortedSet<int[]>(new DistanceComparer());

            for (int index = 0; index < graph.Body.Count(); ++index)
            {
                distance.Add(Int32.MaxValue);
                prev.Add(-1);
            }

            distance[source.IdNode] = 0;
            pq.Add(new int[] { source.IdNode, 0 });

            while (pq.Count > 0)
            {
                int[] minDistVertex = pq.Min;
                if (minDistVertex[0] == endNodePos.IdNode)
                {
                    break;
                }
                pq.Remove(minDistVertex);
                int u = minDistVertex[0];
                Node nodeU = graph.Body.Find(node => node.IdNode == minDistVertex[0]);
                foreach (int[] pairNodeCost in nodeU.ChildrenCost)
                {
                    int v = pairNodeCost[0];
                    int weight = pairNodeCost[1];

                    if (distance[v] > distance[u] + weight)
                    {
                        distance[v] = distance[u] + weight;
                        pq.Add(new int[] { v, distance[v] });
                        prev[v] = u;
                    }
                }
            }

            return prev;
        }
        private class DistanceComparer : IComparer<int[]>
        {
            public int Compare(int[] x, int[] y)
            {
                int result = x[1].CompareTo(y[1]);

                if (result == 0)
                {
                    result = x[0].CompareTo(y[0]);
                }

                return result;
            }
        }

        private void getNode_click(object sender, EventArgs e)
        {
            MouseEventArgs eMouse = (MouseEventArgs)e;
            if (eMouse.Button == MouseButtons.Left)
            {
                if (startNodePos.IdNode == -1 && endNodePos.IdNode == -1)
                {
                    startNodePos = getNearestNodeClick(eMouse.Location);
                }
                else if (startNodePos.IdNode != -1 && endNodePos.IdNode == -1)
                {
                    endNodePos = getNearestNodeClick(eMouse.Location);
                    prevList = dijkstra(startNodePos);
                    Invalidate();
                }
            }
        }

        Node getNearestNodeClick(Point pos)
        {
            int area = 5;
            foreach (Node n in graph.Body)
            {
                if ((pos.X >= n.ScreenPos.X - area && pos.X <= n.ScreenPos.X + area) && (pos.Y >= n.ScreenPos.Y - area && pos.Y <= n.ScreenPos.Y + area))
                    return n;
            }
            return new Node();
        }

        void printPath(Graphics g)
        {
            Pen p = new Pen(Color.Red, 2);
            Brush brush = new SolidBrush(Color.Red);
            int radius = 3;
            g.DrawEllipse(p, startNodePos.ScreenPos.X - radius, startNodePos.ScreenPos.Y - radius, radius + radius, radius + radius);
            g.FillEllipse(brush, startNodePos.ScreenPos.X - radius, startNodePos.ScreenPos.Y - radius, radius + radius, radius + radius);

            g.DrawEllipse(p, endNodePos.ScreenPos.X - radius, endNodePos.ScreenPos.Y - radius, radius + radius, radius + radius);
            g.FillEllipse(brush, endNodePos.ScreenPos.X - radius, endNodePos.ScreenPos.Y - radius, radius + radius, radius + radius);

            int indexNode = endNodePos.IdNode;
            p = new Pen(Color.Blue, 4);
            while (prevList[indexNode] != -1)
            {
                Node currentNode1 = graph.Body.Find(node => node.IdNode == indexNode);
                Node currentNode2 = graph.Body.Find(node => node.IdNode == prevList[indexNode]);
                g.DrawLine(p, currentNode1.ScreenPos, currentNode2.ScreenPos);
                indexNode = prevList[indexNode];
            }
        }

        private void PrintMap()
        {
            buffer = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            Graphics gBuffer = Graphics.FromImage(buffer);
            gBuffer.Clear(Color.White);
            Pen p = new Pen(Color.Black, 1);
            foreach (Arc a in graph.ArcList)
            {
                gBuffer.DrawLine(p, a.StartNode.ScreenPos, a.EndNode.ScreenPos);
            }
            Invalidate();
        }

        private void Map_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(buffer, 0, 0);
            if (startNodePos.IdNode != -1 && endNodePos.IdNode != -1)
            {
                printPath(e.Graphics);
                startNodePos = new Node();
                endNodePos = new Node();
            }
        }
    }
}
