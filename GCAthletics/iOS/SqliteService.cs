using System;
using System.IO;
using SQLite;
using Xamarin.Forms;

namespace GCAthletics.iOS
{
    public class SqliteService : ISqliteService
    {
        public SqliteService()
        {
        }
        string GetPath(string dbName)
        {
            if (string.IsNullOrWhiteSpace(dbName))
            {
                throw new ArgumentException("Invalid database name", nameof(dbName));
            }
            var sqliteFilename = $"{dbName}.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);
            return path;
        }

        public SQLiteConnection GetConnection(string dbName)
        {
            return new SQLiteConnection(GetPath(dbName));
        }
    }
}
