using System;
namespace GCAthletics
{
    public class Event
    {
        private string Name { get; set; }
        private DateTime Date { get; set; }
        private string Description { get; set; }
        private bool Alerts { get; set; }
        private bool IsPrivate { get; set; }
        private Announcement announcement;

        public Event()
        {
            
        }

        public Event(string n, DateTime dt, string d)
        {
            
        }
    }
}
