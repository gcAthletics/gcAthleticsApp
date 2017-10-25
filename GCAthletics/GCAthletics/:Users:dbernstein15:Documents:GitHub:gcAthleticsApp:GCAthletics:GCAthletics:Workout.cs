using System;
namespace GCAthletics
{
    public class Workout
    {
        private string name { get; set; }
        private Exercise[] exercises;
        private DateTime date { get; set; }
        private bool completed { get; set; }
        private Player[] players;

        public Workout()
        {
            
        }

        public Workout(string n, Exercise[] e, DateTime date, Player[] p)
        {
            
        }
    }
}
