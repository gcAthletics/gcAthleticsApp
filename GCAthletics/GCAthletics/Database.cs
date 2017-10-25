using System;
using System.Collections.Generic;
using System.Linq;
using GCAthletics.Models;
using SQLite;
using Xamarin.Forms;

/*
 * Here is some database info from the azure service
 * username: gcAdmin
 * password: ADMINpassword1!
 * Server=tcp:gcathletics.database.windows.net,1433;Initial Catalog=GCathleticsDB;Persist Security Info=False;User ID={your_username};Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
 * 
 */

namespace GCAthletics
{
    public class Database
    {
        static object locker = new object();
        ISqliteService SQLite
        {
            get
            {
                return DependencyService.Get<ISqliteService>();
            }
        }
        readonly SQLiteConnection connection;
        readonly string DatabaseName;

        public Database(string databaseName)
        {
            DatabaseName = databaseName;
            connection = SQLite.GetConnection(DatabaseName);
        }

        public void CreateTable<T>()
        {
            lock (locker)
            {
                connection.CreateTable<T>();
            }
        }

        // make insert item
        /*public void InsertItem<T>(T item)
        {
            lock (locker)
            {
                var id 
            }
        }*/

        public int SaveItem<T>(T item)
        {
            lock (locker)
            {
                var id = ((BaseItemModel)(object)item).ID;
                if (id != 0)
                {
                    connection.Update(item);
                    return id;
                }
                else
                {
                    return connection.Insert(item);
                }
            }
        }

        public void ExecuteQuery(string query, object[] args)
        {
            lock (locker)
            {
                connection.Execute(query, args);
            }
        }

        public List<T> Query<T>(string query, object[] args) where T : new()
        {
            lock (locker)
            {
                return connection.Query<T>(query, args);
            }
        }

        public IEnumerable<T> GetItems<T>() where T : new()
        {
            lock (locker)
            {
                return (from i in connection.Table<T>() select i).ToList();
            }
        }

        public int DeleteItem<T>(int id)
        {
            lock (locker)
            {
                return connection.Delete<T>(id);
            }
        }

        public int DeleteAll<T>()
        {
            lock (locker)
            {
                return connection.DeleteAll<T>();
            }
        }
    }
}
