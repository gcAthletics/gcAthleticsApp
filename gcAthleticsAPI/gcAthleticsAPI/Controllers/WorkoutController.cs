using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gcAthleticsAPI.Models;

namespace gcAthleticsAPI.Controllers
{
    public class WorkoutController : ApiController
    {
        // GET: api/Workout
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Workout/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Workout
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Workout/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Workout/5
        public void Delete(int id)
        {
        }
    }
}
