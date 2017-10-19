using System;
namespace GCAthletics
{
    public class Workout : BaseItem
    {
        public int WorkoutID { get; set; }
        public string Date { get; set; }
        public int Completed { get; set; }
        public int UserID { get; set; }
        public int EventID { get; set; }

        public override string ToString()
        {
            return string.Format("[Workout: WorkoutID={0}, Date={1}, Completed={2}, UserID={3}, EventID={4}]", WorkoutID, Date, Completed, UserID, EventID);
        }
    }
}
