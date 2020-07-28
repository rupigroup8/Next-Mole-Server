using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using next_mole_server.Models.DAL;

namespace next_mole_server.Models
{
    public class Link
    {
        int linkNum;
        string sourceNode;
        string targetNode;
        string connectionType;
        double connectionWeight;

        public int LinkNum { get => linkNum; set => linkNum = value; }
        public string SourceNode { get => sourceNode; set => sourceNode = value; }
        public string TargetNode { get => targetNode; set => targetNode = value; }
        public string ConnectionType { get => connectionType; set => connectionType = value; }
        public double ConnectionWeight { get => connectionWeight; set => connectionWeight = value; }

        public Link()
        {
        }
        public Link(string Snode, string Tnode)
        {
            this.SourceNode = Snode;
            this.TargetNode = Tnode;
        }

        public static int postLinks(List<Link> links, string name)
        {
            DBservices dbs = new DBservices();
            int num = dbs.DBinsertLinks(links, name);
            return num;
        }
        public List<Link> deleteConnection(string connection)
        {
            DBservices dbs = new DBservices();
            return dbs.deleteConnection(connection);
        }

    }
}