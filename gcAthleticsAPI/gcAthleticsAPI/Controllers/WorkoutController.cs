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
        // create entity object which is the connection to the db
        gcDBentity db = new gcDBentity();

        // GET: api/Workout
        [HttpGet]
        public IEnumerable<string> GetWorkouts()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Workout/5
        [HttpGet]
        public string GetWorkout(int id)
        {
            return "value";
        }

        // POST: api/Workout
        [HttpPost]
        public void InsertWorkout([FromBody]string value)
        {
        }

        // PUT: api/Workout/5
        [HttpPut]
        public void UpdateWorkout(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Workout/5
        [HttpDelete]
        public void DeleteWorkout(int id)
        {
        }
    }
}
