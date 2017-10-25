using System;
using SQLite;

namespace GCAthletics.Models
{
    public class BaseItemModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
    }
}
