using System;
using System.Data.SQLite;
using System.Data;

namespace GCAthletics.Database
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            SQLiteConnection db = new SQLiteConnection("Data Source = /Users/awetstone56/Documents/GitHub/gcAthleticsApp/GCAthletics/GCAthletics.Database/GCAthletics; Version = 3;");
            db.Open();
            Console.WriteLine("Successfully connected to database");
            string sql = "insert into Team (Name, Wins, Losses, Sport) values ('TestTeam', 0, 0, 'Softball')";
            SQLiteCommand command = new SQLiteCommand(sql, db);
            command.ExecuteNonQuery();
            Console.WriteLine("Successfuly inserted row into database");
            sql = "select * from Team";
            command = new SQLiteCommand(sql, db);
            SQLiteDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                Console.WriteLine("Name: " + reader["Name"] + "Wins: " + reader["Wins"] +
                                  "Losses: " + reader["Losses"] + "Sport: " + reader["Sport"]);
            }
            db.Close();
            Console.WriteLine("Database Connection closed");
        }
    }
}
