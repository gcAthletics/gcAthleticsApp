using System;
using GCAthletics.Models;

namespace GCAthletics
{
    public class EventModel : BaseItemModel
    {
        // ID provided by BaseItemModel
        public int UserID { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public bool SendAlert { get; set; }
        public string Name { get; set; }
        public int TeamID { get; set; }

        public override string ToString()
        {
            return string.Format("[Event: EventID={0}, UserID={1}, DateTime={2}, Description={3}, SendsAlerts={4}, Name={5}]", ID, UserID, DateTime, Description, SendAlert, Name);
        }
    }
}
