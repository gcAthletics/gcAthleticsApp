using System;
using SQLite;

namespace GCAthletics
{
    public interface ISqliteService
    {
        SQLiteConnection GetConnection(string dbName);
    }
}
