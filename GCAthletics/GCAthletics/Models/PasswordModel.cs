using System;
using SQLite;

namespace GCAthletics
{
    public class PasswordModel
    {
        [Unique]
        public string PasswordHash { get; set; }
        [Unique]
        public int UserID { get; set; }
        public int IsInitial { get; set; }

        public override string ToString()
        {
            return string.Format("[Password: PasswordHash={0}, UserID={1}, IsInitial={2}]", PasswordHash, UserID, IsInitial);
        }
    }
}
