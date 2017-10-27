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
        // GET: api/Annoucements
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Annoucements/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Annoucements
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Annoucements/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Annoucements/5
        public void Delete(int id)
        {
        }
    }
}
