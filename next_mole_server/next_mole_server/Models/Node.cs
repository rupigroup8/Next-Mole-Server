using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using next_mole_server.Models.DAL;

using Newtonsoft.Json;

namespace next_mole_server.Models
{
    public class Node
    {
        int nodeId;
        string nodeNum;
        string nodeImageURL;
        string nodeDescription;

        public int NodeId { get => nodeId; set => nodeId = value; }
        public string NodeNum { get => nodeNum; set => nodeNum = value; }
        public string NodeDescription { get => nodeDescription; set => nodeDescription = value; }
        public string NodeImageURL { get => nodeImageURL; set => nodeImageURL = value; }

        public static object Server { get; private set; }

        public Node()
        {
        }
        public Node(string nodeName)
        {
            this.NodeNum = nodeName;
          
        }

        public Node GetInfo(string nodeName, string categoryName)
        {
            DBservices db = new DBservices();
            Node n = new Node();
            switch (categoryName.ToUpper())
            {
                case "GENERAL KNOWLEDGE":
                    categoryName = "General";
                    break;
                case "FILMS":
                    categoryName = "Movies";
                    break;
                case "CELEBRITY":
                    categoryName = "Celeb";
                    break;
                case "POLITICS":
                    categoryName = "Politicians";
                    break;

                default:
                    break;
            }

            string vertexTableName = categoryName + "Vertecies";
            n = db.GetVertexInfo(vertexTableName, nodeName);
            return n;
        }

        public static int postNodes(List<Node> nodes, string name)
        {
            DBservices dbs = new DBservices();
            int num = dbs.DBinsertNodes(nodes, name);
            return num;
        }
        public static int postAllNodes(List<Node> nodes, string name)
        {

            DBservices dbs = new DBservices();
            int num = dbs.DBinsertAllNodes(nodes, name);
            return num;

        }
       

    }
}