using AdminPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using next_mole_server.Models;

namespace AdminPage.Controllers
{
    public class CategoryController : ApiController
    {


        // GET: api/Category
        [HttpGet]
        [Route("api/Category")]
        public IEnumerable<Category> Get()
        {
            //return new string[] { "value1", "value2" };
            try
            {
                List<Category> categoriesList = new List<Category>();
                Category c = new Category();
                categoriesList = c.Read();
                return categoriesList;

            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        // GET: api/Category/5
        [HttpGet]
        [Route("api/CategoryAmount")]
        public IEnumerable<Category> GetAmount()
        {
            try
            {
                List<Category> categoriesList = new List<Category>();
                Category c = new Category();
                categoriesList = c.ReadCat();
                return categoriesList;
                
            }
            catch (Exception ex)
            {

                throw new Exception("problem with get categoreis, the error: " + ex);
            }
        }

        // POST: api/Category
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Category/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Category/5
        public void Delete(int id)
        {
        }
    }
}
