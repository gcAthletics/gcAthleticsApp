using System;
namespace GCAthletics
{
    public class Team : BaseItem
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string Sport { get; set; }

        public override string ToString()
        {
            return $"{TeamID}, {Name}, {Wins}, {Losses}, {Sport}";
        }
    }
}
