using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using next_mole_server.Models;


namespace next_mole_server.Controllers
{
    public class LinksController : ApiController
    {
        // GET: api/Links
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Links/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Links
        //public void Post([FromBody]List<Link> links, string name)
        //{
        //    Link.postLinks(links, name);
        //}

        [Route("api/links/{name}")]
        public void Post([FromBody]List<Link> links, string name)
        {
            Link.postLinks(links, name);
        }

        // PUT: api/Links/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Links/5
        public IEnumerable<Link> Delete(string connection)
        {
            Link link = new Link();
            return link.deleteConnection(connection);
        }
       
    }
}
