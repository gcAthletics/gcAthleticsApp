using System;
namespace GCAthletics
{
    public class Workout
    {
        private string Name { get; set; }
        private Exercise[] exercises;
        private DateTime Date { get; set; }
        private bool Completed { get; set; }
        private Player[] players;

        public Workout()
        {
            
        }

        public Workout(string n, Exercise[] e, DateTime dt, Player[] p)
        {
            
        }
    }
}
