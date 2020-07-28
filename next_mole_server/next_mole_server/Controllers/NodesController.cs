using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using next_mole_server.Models;

namespace next_mole_server.Controllers
{
    public class NodesController : ApiController
    {
        // GET: api/Nodes
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Nodes/5
        [HttpGet]
        [Route("api/VertexGetInfo")]
        public Node Get(string nodeName, string categoryName)
        {
            try
            {
                Node n = new Node();
                return n.GetInfo(nodeName, categoryName);
            }
            catch (Exception ex)
            {
                throw new Exception("problem to get vertex info: ", ex);
            }
        }

       
        // POST: api/Nodes
        //public void Post([FromBody]List<Node> nodes, string name)
        //{
        //    Node.postNodes(nodes, name);
        //}

        [Route("api/nodes/{name}")]
        public void Post([FromBody]List<Node> nodes, string name)
        {
            Node.postNodes(nodes, name);
           
        }

        [HttpPost]
        [Route("api/nodesSaveAll/{name}")]
        public void PostAll([FromBody]List<Node> nodes, string name)
        {
            Node.postAllNodes(nodes, name);

        }

        // PUT: api/Nodes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Nodes/5
        public void Delete(int id)
        {
        }
    }
}
