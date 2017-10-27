using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gcAthleticsAPI.Models;

namespace gcAthleticsAPI.Controllers
{
    public class TeamController : ApiController
    {
        // create entity object which is the connection to the db
        gcDBentity db = new gcDBentity();

        // GET: api/Team
        public IEnumerable<string> GetAllTeams()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Team/5
        [HttpPost]
        public string GetTeam(int id)
        {
            return "value";
        }

        // POST: api/Team
        [HttpPost]
        public void InsertTeam(string name, string sport, int losses, int wins)
        {
            Team team = new Team();
            team.Name = name;
            team.Sport = sport;
            team.Wins = wins;
            team.Losses = losses;
        }

        // PUT: api/Team/5
        [HttpPut]
        public void UpdateTeam(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Team/5
        [HttpDelete]
        public void DeleteTeam(int id)
        {
        }
    }
}
