﻿using System;
using GCAthletics.Models;

namespace GCAthletics
{
    public class AnnouncementsModel : BaseItemModel
    {
        // ID is provided from BaseItemModel
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public int EventID { get; set; }
        public int TeamID { get; set; }

        public override string ToString()
        {
            return string.Format("[Announcements: AnnouncementID={0}, Name={1}, Description={2}, DateTime={3}, EventID={4}]", ID, Name, Description, DateTime, EventID);
        }
    }
}
