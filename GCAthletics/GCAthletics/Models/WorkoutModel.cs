using System;
using GCAthletics.Models;

namespace GCAthletics
{
    public class WorkoutModel : BaseItemModel
    {
        // ID provided by BaseItemModel
        public DateTime Date { get; set; }
        public bool Completed { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }
        public int TeamID { get; set; }
        public int WorkoutID { get; set; }

        public override string ToString()
        {
            return string.Format("[Workout: WorkoutID={0}, Date={1}, Completed={2}, UserID={3}, TeamID={4}]", ID, Date, Completed, UserID, TeamID);
        }
    }
}
