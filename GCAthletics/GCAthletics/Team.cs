using System;
namespace GCAthletics
{
    public class Team
    {
        private string name { get; set; }
        private Player[] players { get; set; };
        private int wins { get; set; };
        private int losses { get; set; };
        private string sport { get; set; };
        private Coach coach { get; set; };

        public Team()
        {
            
        }

        public Team(String n)
        {
            name = n;
        }

        public void addPlayer(Player player)
        {
            
        }

        public void removerPlayer(Player player)
        {
            
        }
    }
}
