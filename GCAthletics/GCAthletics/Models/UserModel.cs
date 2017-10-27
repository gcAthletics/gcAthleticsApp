using System;
using GCAthletics.Models;

namespace GCAthletics
{
    public class UserModel : BaseItemModel
    {
        // ID provided by BaseItemModel
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public int TeamID { get; set; }

        public override string ToString()
        {
            return string.Format("[User: UserID={0}, Phone={1}, Email={2}, Role={3}, Name={4}, TeamID={5}]", ID, Phone, Email, Role, Name, TeamID);
        }
    }
}
