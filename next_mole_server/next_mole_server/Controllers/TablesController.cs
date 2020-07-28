using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using next_mole_server.Models;

namespace next_mole_server.Controllers
{
    public class TablesController : ApiController
    {
        // GET: api/Tables
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Tables/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Tables
        public void Post([FromBody]string tableName)
        {
            Table.createTable(tableName);
        }

        // PUT: api/Tables/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Tables/5
        public void Delete(int id)
        {
        }
    }
}
