using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gcAthleticsAPI.Models;

namespace gcAthleticsAPI.Controllers
{
    public class ExerciseController : ApiController
    {
        // create entity object which is the connection to the db
        gcDBentity db = new gcDBentity();
        // GET: api/Exercise/5
        [HttpGet]
        public string GetExercise(int id)
        {
            return "value";
        }

        // POST: api/Exercise
        [HttpPost]
        public void InsertExercise([FromBody]string value)
        {
        }

        // PUT: api/Exercise/5
        [HttpPut]
        public void UpdateExercise(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Exercise/5
        [HttpDelete]
        public void DeleteExercise(int id)
        {
        }
    }
}
