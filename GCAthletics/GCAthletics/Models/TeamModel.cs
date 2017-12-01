using System;
using GCAthletics.Models;

namespace GCAthletics
{
    public class TeamModel : BaseItemModel
    {
        // ID provided BaseItemModel
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string Sport { get; set; }
        public string Coach { get; set; }

        public override string ToString()
        {
            return $"{ID}, {Name}, {Wins}, {Losses}, {Sport}";
        }
    }
}
