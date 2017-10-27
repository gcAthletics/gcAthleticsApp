using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gcAthleticsAPI.Models;

namespace gcAthleticsAPI.Controllers
{
    public class AnnoucementsController : ApiController
    {
        // create entity object which is the connection to the db
        gcDBentity db = new gcDBentity();

        // GET: api/Annoucements
        [HttpGet]
        public IEnumerable<string> GetAllAnnouncements()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Annoucements/5
        [HttpGet]
        public string GetAnnouncement(int id)
        {
            return "value";
        }

        // POST: api/Annoucements
        [HttpPost]
        public void InsertAnnouncement([FromBody]string value)
        {
        }

        // PUT: api/Annoucements/5
        [HttpPut]
        public void UpdateAnnouncement(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Annoucements/5
        [HttpDelete]
        public void DeleteAnnouncement(int id)
        {
        }
    }
}
