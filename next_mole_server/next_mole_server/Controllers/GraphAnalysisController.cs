using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using next_mole_server.Models;

namespace next_mole_server.Controllers
{
    public class GraphAnalysisController : ApiController
    {
        // GET: api/GraphAnalysis
        //public IEnumerable<string> Get()
        //{
        //    //return GraphAnalysis.doPython();
        //    return new string[] { "value1", "value2" };
        //}

        public void Get()
        {
            //string a = "d";
            //int b = 7;
            Analyze.Main();
            //GraphAnalysis.doPython();
            //return new string[] { "value1", "value2" };
        }

        // GET: api/GraphAnalysis/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/GraphAnalysis
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/GraphAnalysis/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/GraphAnalysis/5
        public void Delete(int id)
        {
        }
    }
}
