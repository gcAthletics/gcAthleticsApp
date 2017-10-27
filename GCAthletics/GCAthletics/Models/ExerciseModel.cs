using System;
using GCAthletics.Models;

namespace GCAthletics
{
    public class ExerciseModel : BaseItemModel
    {
        // ID provided by BaseItemModel
        public string Name { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public int Completed { get; set; }

        public override string ToString()
        {
            return string.Format("[Exercise: ExerciseID={0}, Name={1}, Sets={2}, Reps={3}, Description={4}, Weight={5}, Completed={6}]", ID, Name, Sets, Reps, Description, Weight, Completed);
        }
    }
}
