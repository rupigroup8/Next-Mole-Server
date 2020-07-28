using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using next_mole_server.Models.DAL;

namespace next_mole_server.Models
{
    public class Network
    {
        List<Node> nodes;
        List<Link> links;

        public List<Node> Nodes { get => nodes; set => nodes = value; }
        public List<Link> Links { get => links; set => links = value; }
       
        public Network GetNet(string categoryName)
        {
            DBservices db = new DBservices();
            Network n = new Network();
            n = db.GetNetInfo(categoryName);
            return n;
        }
    }
}