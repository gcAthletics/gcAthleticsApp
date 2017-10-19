using System;
namespace GCAthletics
{
    public class Event : BaseItem
    {
        public int EventID { get; set; }
        public int UserID { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public int SendsAlerts { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("[Event: EventID={0}, UserID={1}, Date={2}, Description={3}, SendsAlerts={4}, Name={5}]", EventID, UserID, Date, Description, SendsAlerts, Name);
        }
    }
}
