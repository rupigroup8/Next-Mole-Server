using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using next_mole_server.Models;

namespace next_mole_server.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
     
        public IEnumerable<User> Get()
        {
            User user = new User();
            return user.getAll();

        }

        // GET: api/User/5    
        [Route("api/user/{UserEmail}/{UserPassword}")]
        public bool Get(string UserEmail, string UserPassword)
        {
            User user = new User();
            return user.checkLogin(UserEmail, UserPassword);
        }

        // POST: api/User
        //[Route("api/user")]
        public void Post([FromBody] User user)
        {
            user.InsertUser();
        }
       
        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
