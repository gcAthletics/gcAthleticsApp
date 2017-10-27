using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gcAthleticsAPI.Models;

namespace gcAthleticsAPI.Controllers
{
    public class EventsController : ApiController
    {
        // create entity object which is the connection to the db
        gcDBentity db = new gcDBentity();

        // GET: api/Events/5
        [HttpGet]
        public string GetEvent(int id)
        {
            return "value";
        }

        // POST: api/Events
        [HttpPost]
        public void InsertEvent([FromBody]string value)
        {
        }

        // PUT: api/Events/5
        [HttpPut]
        public void UpdateEvent(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Events/5
        [HttpDelete]
        public void DeleteEvent(int id)
        {
        }
    }
}
