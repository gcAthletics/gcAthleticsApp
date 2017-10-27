using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gcAthleticsAPI.Models;
using System.Text;
using System.Security.Cryptography;

namespace gcAthleticsAPI.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ApiController
    {
        // create entity object which is the connection to the db
        gcDBentity db = new gcDBentity();

        // GET: api/Users
        [HttpGet]
        public IEnumerable<string> GetAllUsers()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Users/5
        [HttpGet]
        [ActionName("GetUser")]
        public IQueryable<User> GetUser(int id)
        {
            var user = db.Users.Where(x => x.UserID == id);
            if(user == null)
            {
                return null;
            }
            else
            {
                return user;
            }
        }

        // POST: api/Users
        [HttpPost]
        public void InsertUser([FromBody]string value)
        {

        }

        // PUT: api/Users/5
        [HttpPut]
        public void UpdateUser(int id, [FromBody]string value)
        {
            
        }

        // DELETE: api/Users/5
        [HttpDelete]
        public void DeleteUser(int id)
        {
            
        }

        [HttpGet]
        [ActionName("Login")]
        public HttpResponseMessage Login(string username, string password)
        {
            var user = db.Users.Where(x => (x.Email == username) && (x.PasswordHash == passwordHash(password)));
            if(user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Please enter valid username and password");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, "Success"); 
            }
        }

        // Method to hash the password
        // returns a 256 bit hash value as a string
        public static string passwordHash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
