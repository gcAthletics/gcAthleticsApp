using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Security.Cryptography;

namespace GCAthletics.Droid
{
    class DButility
    {
        public DButility() { }

        private SqlConnectionStringBuilder sqlConBuilder = new SqlConnectionStringBuilder();

        static SqlConnection connection = null;
        private SqlCommand command = null;

        public SqlConnection createConnection()
        {
            sqlConBuilder.DataSource = "den1.mssql2.gear.host";
            sqlConBuilder.UserID = "gcathletics";
            sqlConBuilder.Password = "Bobcats1!";
            sqlConBuilder.InitialCatalog = "gcathletics";

            try
            {
                connection = new SqlConnection(sqlConBuilder.ConnectionString);
                connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return connection;
        }

        public bool appLogin(string email, string password)
        {
            bool authenticated = false;

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT Email, PasswordHash FROM Users ");
            queryBuilder.Append("WHERE Email = '");
            queryBuilder.Append(email);
            queryBuilder.Append("';");
            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            if (reader.GetString(1).Equals(passwordHash(password), StringComparison.InvariantCultureIgnoreCase))
                            {
                                authenticated = true;
                            }
                        }
                    }
                    else
                    {
                        authenticated = false;
                    }
                }
            }

            return authenticated;
        }

        private string passwordHash(string password)
        {
            StringBuilder builder = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(password));

                foreach (Byte b in result)
                    builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        public void insertAnnouncement(AnnouncementsModel announcement)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Announcements (Name, Description, DateTime, EventID, TeamID)" +
                                "VALUES ('");
            queryBuilder.Append(announcement.Name + "','");
            queryBuilder.Append(announcement.Description + "','");
            queryBuilder.Append(announcement.DateTime.ToString() + "', ");
            queryBuilder.Append(announcement.EventID + ", ");
            queryBuilder.Append(announcement.TeamID + ");");

            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<AnnouncementsModel> getAllAnnouncements(int teamID)
        {
            DateTime sqlDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT AnnouncementID, Name, Description, DateTime, EventID FROM Announcements ");
            queryBuilder.Append("WHERE TeamID = '" + teamID + "' ");
            queryBuilder.Append("ORDER BY DateTime DESC;");

            string query = queryBuilder.ToString();
            List<AnnouncementsModel> rc = new List<AnnouncementsModel>();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AnnouncementsModel announcement = new AnnouncementsModel();
                            announcement.ID = reader.GetInt32(0);
                            announcement.Name = reader.GetString(1);
                            announcement.Description = reader.GetString(2);
                            announcement.DateTime = reader.GetDateTime(3);
                            announcement.EventID = reader.GetInt32(4);
                            rc.Add(announcement);
                        }
                    }
                }
            }

            return rc;
        }

        public void insertTeam(TeamModel team)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Teams (Name, Wins, Losses, Sport) " +
                                "VALUES ('");
            queryBuilder.Append(team.Name + "', ");
            queryBuilder.Append(team.Wins.ToString() + ", ");
            queryBuilder.Append(team.Losses.ToString() + " , '");
            queryBuilder.Append(team.Sport + "');");

            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public TeamModel getTeamById(int id)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT TeamID, Name, Wins, Losses, Sport FROM Team ");
            queryBuilder.Append("WHERE TeamID = " + id.ToString() + ";");

            string query = queryBuilder.ToString();
            TeamModel rc = new TeamModel();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            rc.ID = reader.GetInt32(0);
                            rc.Name = reader.GetString(1);
                            rc.Wins = reader.GetInt32(2);
                            rc.Losses = reader.GetInt32(3);
                            rc.Sport = reader.GetString(4);
                        }
                    }
                }
            }

            return rc;
        }

        public void updateTeam(TeamModel team)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE Team SET ");
            queryBuilder.Append("Name = '" + team.Name + "', ");
            queryBuilder.Append("Wins = " + team.Wins.ToString() + ", ");
            queryBuilder.Append("Losses = " + team.Losses.ToString() + ", ");
            queryBuilder.Append("Sport = '" + team.Sport + "' ");
            queryBuilder.Append("WHERE TeamID = " + team.ID + ";");

            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<UserModel> getAllUsersByTeamID(int id)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT UserId, Phone, Email, Role, Name, TeamID, IsInitial FROM Users ");
            queryBuilder.Append("WHERE TeamID = " + id + ";");

            string query = queryBuilder.ToString();
            List<UserModel> rc = new List<UserModel>();
            UserModel user = new UserModel();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new UserModel();
                                user.ID = reader.GetInt32(0);
                                user.Phone = reader.GetString(1);
                                user.Email = reader.GetString(2);
                                user.Role = reader.GetString(3);
                                user.Name = reader.GetString(4);
                                user.TeamID = reader.GetInt32(5);
                                rc.Add(user);
                            }
                        }
                    }
                }
            }

            return rc;
        }

        public UserModel getUserByEmail(string email)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT UserId, Phone, Email, Role, Name, TeamID, IsInitial FROM Users ");
            queryBuilder.Append("WHERE Email = '" + email + "';");

            string query = queryBuilder.ToString();
            UserModel rc = new UserModel();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using(command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();

                    if(result != null)
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            rc.ID = reader.GetInt32(0);
                            rc.Phone = reader.GetString(1);
                            rc.Email = reader.GetString(2);
                            rc.Role = reader.GetString(3);
                            rc.Name = reader.GetString(4);
                            rc.TeamID = reader.GetInt32(5);
                            rc.IsInitial = reader.GetBoolean(6);
                        }
                    }
                }
            }

            return rc;
        }

        public UserModel getUserById(int id)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT UserID, Phone, Email, Role, Name, TeamID, IsInitial FROM Users ");
            queryBuilder.Append("WHERE UserID = " + id + ";");

            string query = queryBuilder.ToString();
            UserModel rc = null;

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            rc.ID = reader.GetInt32(0);
                            rc.Phone = reader.GetString(1);
                            rc.Email = reader.GetString(2);
                            rc.Role = reader.GetString(3);
                            rc.Name = reader.GetString(4);
                            rc.TeamID = reader.GetInt32(5);
                            rc.IsInitial = reader.GetBoolean(6);
                        }
                    }
                }
            }

            return rc;
        }

        public String[] getUsersByTeamId(int teamID)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT Name ");
            queryBuilder.Append("FROM Users ");
            queryBuilder.Append("WHERE TeamID = " + teamID);

            string query = queryBuilder.ToString();

            String[] users = new String[50];

            if(connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if(connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();

                    if(result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Append(reader.GetString(0));
                            } 
                        }
                    }
                }
            }

            return users;
        }

        public int RegisterTeam(TeamModel teamModel)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Team (Name, Wins, Losses, Coach, Sport) ");
            queryBuilder.Append("VALUES ('");
            queryBuilder.Append(teamModel.Name + "', ");
            queryBuilder.Append(teamModel.Wins + ", ");
            queryBuilder.Append(teamModel.Losses + ", '");
            queryBuilder.Append(teamModel.Coach + "', '");
            queryBuilder.Append(teamModel.Sport + "');");
            string query = queryBuilder.ToString();

            StringBuilder queryBuilder2 = new StringBuilder();
            queryBuilder2.Append("SELECT TeamID FROM Team ");
            queryBuilder2.Append("WHERE Name = '" + teamModel.Name + "' ");
            queryBuilder2.Append("AND Wins = " + teamModel.Wins + " ");
            queryBuilder2.Append("AND Losses = " + teamModel.Losses + " ");
            queryBuilder2.Append("AND Coach = '" + teamModel.Coach + "' ");
            queryBuilder2.Append("AND Sport = '" + teamModel.Sport + "';");
            string query2 = queryBuilder2.ToString();


            int teamID = 0;

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query2, connection))
                {
                    Object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                teamID = reader.GetInt32(0);
                            }
                        }
                    }
                }
            }

            return teamID;
        }

        public void insertUser(UserModel user)
        {
            StringBuilder queryBuilder = new StringBuilder();
            string pwd = "gobobcats1";
            int initial = -1;
            if (user.IsInitial)
                initial = 1;
            else
                initial = 0;

            queryBuilder.Append("INSERT INTO Users (Name, PasswordHash, Phone, Email, Role, TeamID, IsInitial) " +
                                "VALUES ('");
            queryBuilder.Append(user.Name + "', '");
            queryBuilder.Append(passwordHash(pwd) + "', '");
            queryBuilder.Append(user.Phone + "', '");
            queryBuilder.Append(user.Email + "', '");
            queryBuilder.Append(user.Role + "', ");
            queryBuilder.Append(user.TeamID + ", ");
            queryBuilder.Append(initial + ");");

            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void removeUser(int userID)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("DELETE FROM Users ");
            queryBuilder.Append("WHERE UserID = ");
            queryBuilder.Append(userID);
            queryBuilder.Append(";");

            string query = queryBuilder.ToString();

            if(connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if(connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void changePassword(string pwd, string email)
        {
            String hashPassword = passwordHash(pwd);

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE Users ");
            queryBuilder.Append("SET PasswordHash = '");
            queryBuilder.Append(hashPassword + "', ");
            queryBuilder.Append("IsInitial = 0 ");
            queryBuilder.Append("WHERE Email = '");
            queryBuilder.Append(email + "'");

            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // waiting on DB change to include TeamId
        public IEnumerable<AnnouncementsModel> getAnnouncementsByTeamId(int teamId)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT AnnouncementID, Name, Description, DateTime, TeamID, EventID FROM Announcements ");
            queryBuilder.Append("WHERE TeamID = " + teamId + " ORDER BY DateTime DESC;");
            string query = queryBuilder.ToString();
            List<AnnouncementsModel> rc = new List<AnnouncementsModel>();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AnnouncementsModel announcement = new AnnouncementsModel();
                            announcement.ID = reader.GetInt32(0);
                            announcement.Name = reader.GetString(1);
                            announcement.Description = reader.GetString(2);
                            announcement.DateTime = reader.GetDateTime(3);
                            announcement.TeamID = reader.GetInt32(4);
                            announcement.EventID = reader.GetInt32(5);
                            rc.Add(announcement);
                        }
                    }
                }
            }

            return rc;
        }

        public IEnumerable<EventModel> getAllEventsByUserAndDate(int userId, DateTime dt)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT EventID, Name, Description, DateTime, SendAlert FROM Events ");
            queryBuilder.Append("WHERE UserID = " + userId);
            queryBuilder.Append(" AND DateTime > '" + dt + "'");
            queryBuilder.Append(" AND DateTime < '" + dt.AddDays(1));
            queryBuilder.Append("' ORDER BY DateTime ASC;");
            string query = queryBuilder.ToString();
            List<EventModel> rc = new List<EventModel>();
            EventModel e = null;

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            e = new EventModel();
                            e.ID = reader.GetInt32(0);
                            e.Name = reader.GetString(1);
                            e.Description = reader.GetString(2);
                            e.DateTime = reader.GetDateTime(3);
                            e.SendAlert = reader.GetBoolean(4);
                            rc.Add(e);
                        }
                    }
                }
            }

            return rc;
        }

        public IEnumerable<EventModel> getAllEventsByTeamIDAndDate(int TeamID, DateTime dt)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT EventID, Name, Description, DateTime, SendAlert FROM Events ");
            queryBuilder.Append("WHERE TeamID = " + TeamID);
            queryBuilder.Append(" AND DateTime > '" + dt + "'");
            queryBuilder.Append(" AND DateTime < '" + dt.AddDays(1));
            queryBuilder.Append("' ORDER BY DateTime ASC;");
            string query = queryBuilder.ToString();
            List<EventModel> rc = new List<EventModel>();
            EventModel e = null;

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            e = new EventModel();
                            e.ID = reader.GetInt32(0);
                            e.Name = reader.GetString(1);
                            e.Description = reader.GetString(2);
                            e.DateTime = reader.GetDateTime(3);
                            e.SendAlert = reader.GetBoolean(4);
                            rc.Add(e);
                        }
                    }
                }
            }

            return rc;
        }
        // Edit database to accept a teamId or a userId (cannot have both at the same time to reduce repetitive data)
        public void insertEventForUser(EventModel e, int userId)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Events (Name, Description, DateTime, UserID, TeamID, SendAlert) " +
                                "VALUES ('");
            queryBuilder.Append(e.Name + "', '");
            queryBuilder.Append(e.Description + "', '");
            queryBuilder.Append(e.DateTime + "', ");
            queryBuilder.Append(e.UserID + ", ");
            queryBuilder.Append("0, ");
            if(e.SendAlert == true)
            {
                queryBuilder.Append(1 + "); ");
            }
            else
            {
                queryBuilder.Append(0 + ");");
            }

            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Edit database to accept a teamId or a userId (cannot have both at the same time to reduce repetitive data)
        public void insertEventForTeam(EventModel e, int teamId)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Events (Name, Description, DateTime, TeamID, SendAlert) " +
                                "VALUES ('");
            queryBuilder.Append(e.Name + "', '");
            queryBuilder.Append(e.Description + "', '");
            queryBuilder.Append(e.DateTime + "', ");
            queryBuilder.Append(e.TeamID + ", ");
            if (e.SendAlert == true)
            {
                queryBuilder.Append(1 + "); ");
                // logic here to insert in a new announcment for the team
            }
            else
            {
                queryBuilder.Append(0 + ");");
            }

            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void insertWorkoutForTeam(WorkoutModel e, int teamId)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Workouts (Date, Completed, TeamId, Description) ");
            queryBuilder.Append("VALUES ('");
            queryBuilder.Append(e.Date + "', ");
            queryBuilder.Append("0, ");
            queryBuilder.Append(teamId + ", '");
            queryBuilder.Append(e.Description + "')");
            
            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void insertWorkoutForUser(WorkoutModel e, int userId)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Workouts (Date, Completed, UserId) ");
            queryBuilder.Append("VALUES ('");
            queryBuilder.Append(e.Date + "', '");
            queryBuilder.Append("0, ");
            queryBuilder.Append(userId + ");");

            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void updateEvent(EventModel e)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE Events SET ");
            queryBuilder.Append("Name = '" + e.Name + "', ");
            queryBuilder.Append("Description = '" + e.Description + "', ");
            queryBuilder.Append("DateTime = " + e.DateTime + ", ");

            if(e.SendAlert == true)
            {
                queryBuilder.Append("SendAlert = " + 1 + ", ");
            }
            else
            {
                queryBuilder.Append("SendAlert = " + 0 + " ");
            }
            queryBuilder.Append("WHERE EventID = " + e.ID + ";");

            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void deleteEvent(EventModel e)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("DELETE FROM Events WHERE EventID = " + e.ID);
            if(e.SendAlert == true)
            {
                // logic for creating a new announcment that this event got deleted
            }
            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<WorkoutModel> GetAllCurrentWorkoutsByTeamID(int teamID)
        {
            DateTime sqlDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT Date, Completed, Description, WorkoutID ");
            queryBuilder.Append("FROM Workouts ");
            queryBuilder.Append("WHERE TeamID = " + teamID);
            queryBuilder.Append("AND Date > '" + sqlDate + "';");
            List<WorkoutModel> rc = new List<WorkoutModel>();
            WorkoutModel e = null;

            string query = queryBuilder.ToString();

            if (connection.State == System.Data.ConnectionState.Closed){
                try{
                    connection.Open();
                }
                catch (SqlException ex){
                    Console.WriteLine(ex);
                }
            }
            else if (connection.State == System.Data.ConnectionState.Open){
                using (command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            e = new WorkoutModel();
                            e.Date = reader.GetDateTime(0);
                            if (reader.GetBoolean(1))
                                e.Completed = true;
                            else
                                e.Completed = false;
                            e.Description = reader.GetString(2);
                            e.WorkoutID = reader.GetInt32(3);
                            rc.Add(e);
                        }
                    }
                }
            }

            return rc;
        }
    }
}