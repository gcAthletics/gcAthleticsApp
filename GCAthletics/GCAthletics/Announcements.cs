using System;
namespace GCAthletics
{
    public class Announcements : BaseItem
    {
        public int AnnouncementID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public int EventID { get; set; }

        public override string ToString()
        {
            return string.Format("[Announcements: AnnouncementID={0}, Name={1}, Description={2}, Date={3}, EventID={4}]", AnnouncementID, Name, Description, Date, EventID);
        }
    }
}
