using System;
namespace GCAthletics
{
    public class Event
    {
        private string name { get; set; }
        private DateTime date { get; set; }
        private string description { get; set; }
        private bool alerts { get; set; }
        private bool isPrivate { get; set; }
        private Announcement announcement { get; set; }

        public Event()
        {
            
        }

        public Event(string n, string dt, string d)
        {
            
        }
    }
}
