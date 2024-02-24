using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Luxembourg_Map
{
    internal class Graph
    {
        private List<Node> body;
        private List<Arc> arcList;

        public Graph()
        {
            Body = new List<Node>();
            ArcList = new List<Arc>();
        }

        public Graph(XmlDocument doc)
        {
            Body = new List<Node>();
            ArcList = new List<Arc>();

            doc.Load("LuxembourgMap.xml");

            double maxLon = 5018275;
            double minLon = 4945029;
            double maxLat = 652685;
            double minLat = 573929;

            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            double deltaLon = maxLon - minLon;
            double lonScale = (screenWidth - 1) / deltaLon;
            double x0 = -minLon * lonScale;
            double deltaLat = maxLat - minLat;
            double latScale = (screenHeight - 1) / deltaLat;
            double y0 = maxLat * latScale;

            foreach (XmlNode node in doc.SelectSingleNode("/map/nodes"))
            {
                int id = int.Parse(node.Attributes[0].Value);
                double longitude = int.Parse(node.Attributes[1].Value);
                double latitude = int.Parse(node.Attributes[2].Value);

                double screenX = x0 + longitude * lonScale;
                double screenY = y0 - latitude * latScale; ;

                Point screenPoint = new Point((int)screenX, (int)screenY);
                Node newNode = new Node(id, screenPoint, new List<int[]>());
                body.Add(newNode);
            }

            foreach (XmlNode arc in doc.SelectSingleNode("/map/arcs"))
            {
                int startNodeId = int.Parse(arc.Attributes[0].Value);
                int endNodeId = int.Parse(arc.Attributes[1].Value);
                int length = int.Parse(arc.Attributes[2].Value);

                Node startNode = body.Find(nodeG => nodeG.IdNode == startNodeId);
                Node endNode = body.Find(nodeG => nodeG.IdNode == endNodeId);

                startNode.ChildrenCost.Add(new int[] { endNode.IdNode, length });
                endNode.ChildrenCost.Add(new int[] { startNode.IdNode, length });

                Arc newArc = new Arc(startNode, endNode, length);

                arcList.Add(newArc);
            }

        }

        public List<Node> Body
        {
            get { return body;}
            set { body = value; }
        }

        public List<Arc> ArcList
        {
            get { return arcList; }
            set { arcList = value; }
        }
    }

}
