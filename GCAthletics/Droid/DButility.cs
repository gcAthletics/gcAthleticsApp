/*
 * This class contains all need methods to connect to the database and run queries on it.
 * It will need to be instantiated as an object, and then call DButility.createConnection()
 * Then any queries can be run on the database.
 */ 

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

        //connection string builder
        private SqlConnectionStringBuilder sqlConBuilder = new SqlConnectionStringBuilder();

        static SqlConnection connection = null;
        private SqlCommand command = null;

        //add connection string information
        //returnn SqlConnection object tied to database.
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

        //returns true if input email and password match a email and corresponding password hash
        //first parameter is an email string, second is a password string
        public bool appLogin(string email, string password)
        {
            bool authenticated = false;

            //query to select a user and password hash by input email
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT Email, PasswordHash FROM Users ");
            queryBuilder.Append("WHERE Email = '");
            queryBuilder.Append(email);
            queryBuilder.Append("';");
            string query = queryBuilder.ToString();

            //check to see if connected to a database.
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
            //if connected, compare password hashes
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();
                    
                    //check if anything was returned, if not no emails match a user in the database
                    if (result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //compare the returned password hash with the hash of the input password
                            //if true, then the method will return true
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

        //computes the SHA256 hash of an input string
        //returns the hash value
        //takes a string value
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

        //inserts announcement into database
        //takes an AnnouncementModel object
        public void insertAnnouncement(AnnouncementsModel announcement)
        {
            //query to insert announcement into database
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Announcements (Name, Description, DateTime, EventID, TeamID)" +
                                "VALUES ('");
            queryBuilder.Append(announcement.Name + "','");
            queryBuilder.Append(announcement.Description + "','");
            queryBuilder.Append(announcement.DateTime.ToString() + "', ");
            queryBuilder.Append(announcement.EventID + ", ");
            queryBuilder.Append(announcement.TeamID + ");");

            string query = queryBuilder.ToString();

            //check if database connection is closed, if so open it
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
            //if connection is open, execute query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //Gets all announcements by teamID and current date
        //takes an int value that should be a teamID
        //returns list of AnnouncementModels
        public IEnumerable<AnnouncementsModel> getAllAnnouncements(int teamID)
        {
            //get current date
            DateTime sqlDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            //query to get all anouncements that have a date greater than the current date and match the input teamID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT AnnouncementID, Name, Description, DateTime, EventID FROM Announcements ");
            queryBuilder.Append("WHERE TeamID = '" + teamID + "' ");
            queryBuilder.Append("ORDER BY DateTime DESC;");

            string query = queryBuilder.ToString();
            List<AnnouncementsModel> rc = new List<AnnouncementsModel>();

            //if database connection is closed, open it
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
            //if connection state is open, execute query 
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {   
                            //create new AnnouncementModel for each query result
                            //add AnnouncementModel to list
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

        //inserts a team to the database
        //takes a TeamModel object as its only parameter
        public void insertTeam(TeamModel team)
        {   
            //query to insert team
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Teams (Name, Wins, Losses, Sport) " +
                                "VALUES ('");
            queryBuilder.Append(team.Name + "', ");
            queryBuilder.Append(team.Wins.ToString() + ", ");
            queryBuilder.Append(team.Losses.ToString() + " , '");
            queryBuilder.Append(team.Sport + "');");

            string query = queryBuilder.ToString();

            //if database connection is closed, open it
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
            //if connection state is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //this get's a team information by matching a teamID
        //takes an int value as a teamID as its only parameter
        public TeamModel getTeamById(int id)
        {
            //query to get a team by teamID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT TeamID, Name, Wins, Losses, Sport FROM Team ");
            queryBuilder.Append("WHERE TeamID = " + id.ToString() + ";");

            string query = queryBuilder.ToString();
            TeamModel rc = new TeamModel();

            //if database connection is closed, open it
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
            //if database connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {   
                            //populate TeamModel from query result
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

        //this method updates a team's information in the database
        //its only parameter is a TeamModel object
        public void updateTeam(TeamModel team)
        {   
            //query to update team by matching teamID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE Team SET ");
            queryBuilder.Append("Name = '" + team.Name + "', ");
            queryBuilder.Append("Wins = " + team.Wins.ToString() + ", ");
            queryBuilder.Append("Losses = " + team.Losses.ToString() + ", ");
            queryBuilder.Append("Sport = '" + team.Sport + "' ");
            queryBuilder.Append("WHERE TeamID = " + team.ID + ";");

            string query = queryBuilder.ToString();

            //if database connection is closed, open it
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
            //if database connection is open, execute query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //this method gets all users on the same team by using the teamID
        //Takes in an int value as a TeamID
        //returns a list of UserModel objects
        public IEnumerable<UserModel> getAllUsersByTeamID(int id)
        {
            //query to get all users by teamID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT UserId, Phone, Email, Role, Name, TeamID, IsInitial FROM Users ");
            queryBuilder.Append("WHERE TeamID = " + id + ";");

            string query = queryBuilder.ToString();
            List<UserModel> rc = new List<UserModel>();
            UserModel user = new UserModel();

            //if databaseconnection is closed, open it
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
            //if database connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();
                    //check to see if results are empty, if so, there are no users with a matching TeamID
                    if (result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //for each query result, create a new UserModel,
                            //populate it with the query result, and add it to the list
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

        //gets a user by their email
        //takes an email string as its parameter
        //returns a UserModel object
        public UserModel getUserByEmail(string email)
        {
            //query to get user by email
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT UserId, Phone, Email, Role, Name, TeamID, IsInitial FROM Users ");
            queryBuilder.Append("WHERE Email = '" + email + "';");

            string query = queryBuilder.ToString();
            UserModel rc = new UserModel();

            //if database connection is closed, open it
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
            //if connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using(command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();
                    //check if results are empty, if so there is no matching user
                    if(result != null)
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {   
                            //create a nenw UserModel and populate it with the query results
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

        //gets a user by a user ID
        //takes in an int value as a userID
        //returns a UserModel object
        public UserModel getUserById(int id)
        {
            //query to get a user by UserID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT UserID, Phone, Email, Role, Name, TeamID, IsInitial FROM Users ");
            queryBuilder.Append("WHERE UserID = " + id + ";");

            string query = queryBuilder.ToString();
            UserModel rc = null;

            //if database connection is closed, open it
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
            //if databse connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();
                    //check to see if results are empty, if so there is no matching user
                    if (result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //create a new UserModel object and populate it with the query results
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

        //gets all user's names by teamID
        //returns a string list of user's names
        public String[] getUsersByTeamId(int teamID)
        {
            //query to get Names of all users by teamID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT Name ");
            queryBuilder.Append("FROM Users ");
            queryBuilder.Append("WHERE TeamID = " + teamID);

            string query = queryBuilder.ToString();

            String[] users = new String[50];

            //if database connection is closed, open it
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
            //if databse connection is open, run query
            else if(connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();
                    //check to see if results are empty, if so there are no users with matching teamID
                    if(result != null)
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //add each query result to the list to be returned
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

        //adds a new team to the database
        //takes a TeamModel object as a parameter
        //returns an int value which is the newly added team's teamID
        public int RegisterTeam(TeamModel teamModel)
        {
            //query to insert new team
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Team (Name, Wins, Losses, Coach, Sport) ");
            queryBuilder.Append("VALUES ('");
            queryBuilder.Append(teamModel.Name + "', ");
            queryBuilder.Append(teamModel.Wins + ", ");
            queryBuilder.Append(teamModel.Losses + ", '");
            queryBuilder.Append(teamModel.Coach + "', '");
            queryBuilder.Append(teamModel.Sport + "');");
            string query = queryBuilder.ToString();

            //query to get new team's teamID
            StringBuilder queryBuilder2 = new StringBuilder();
            queryBuilder2.Append("SELECT TeamID FROM Team ");
            queryBuilder2.Append("WHERE Name = '" + teamModel.Name + "' ");
            queryBuilder2.Append("AND Wins = " + teamModel.Wins + " ");
            queryBuilder2.Append("AND Losses = " + teamModel.Losses + " ");
            queryBuilder2.Append("AND Coach = '" + teamModel.Coach + "' ");
            queryBuilder2.Append("AND Sport = '" + teamModel.Sport + "';");
            string query2 = queryBuilder2.ToString();


            int teamID = 0;
            //if database connection state is closed, open it
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
            //if database connection is open, run first query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            //if database connection is closed, open it
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
            //if database connection is oopen, run second query
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

        //inserts a new user to the database
        //sets the new user's passwordHash to be the SHA256 hash value of "gobobcats1" without quotes
        //takes a UserModel as its only parameter
        public void insertUser(UserModel user)
        {
            StringBuilder queryBuilder = new StringBuilder();
            string pwd = "gobobcats1";
            int initial = -1;
            if (user.IsInitial)
                initial = 1;
            else
                initial = 0;

            //query to insert user
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

            //if database connection is closed, open it
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
            //if database connection is open, run the query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //removes a user that has a matching userID
        //takes an int value as a userID as its only parameter
        public void removeUser(int userID)
        {
            //query for removing a user by UserID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("DELETE FROM Users ");
            queryBuilder.Append("WHERE UserID = ");
            queryBuilder.Append(userID);
            queryBuilder.Append(";");

            string query = queryBuilder.ToString();

            //if database connection is closed, open it
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
            //if database connection is open, run query
            else if(connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //changes a users password
        //takes in a string to calculate the new passwordHash
        //takes in a string to find the user by email
        public void changePassword(string pwd, string email)
        {
            //calculate SHA256 hash value of input password string
            String hashPassword = passwordHash(pwd);

            //query to update passwordHash by matching email
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE Users ");
            queryBuilder.Append("SET PasswordHash = '");
            queryBuilder.Append(hashPassword + "', ");
            queryBuilder.Append("IsInitial = 0 ");
            queryBuilder.Append("WHERE Email = '");
            queryBuilder.Append(email + "'");

            string query = queryBuilder.ToString();

            //if database connection is closed, open it
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
            //if database connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //get all announcements by teamID
        //takes in an int value as a teamID
        //returns a list of AnnouncementsModel objects
        public IEnumerable<AnnouncementsModel> getAnnouncementsByTeamId(int teamId)
        {
            //query to get all anouncements by matching teamID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT AnnouncementID, Name, Description, DateTime, TeamID, EventID FROM Announcements ");
            queryBuilder.Append("WHERE TeamID = " + teamId + " ORDER BY DateTime DESC;");
            string query = queryBuilder.ToString();
            List<AnnouncementsModel> rc = new List<AnnouncementsModel>();

            //if databse connection is closed, open it
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
            //if database connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //for each query result, create a new AnnouncementsModel object
                        //then add the object to the list to be returned
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

        //gets all events by userID and Date/gets all private events for selected date
        //takes an int value as a UserID and a DateTime object
        //returns a list of EventModel objects
        public IEnumerable<EventModel> getAllEventsByUserAndDate(int userId, DateTime dt)
        {
            //query for getting all events greater than input date and have a matching userID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT EventID, Name, Description, DateTime, SendAlert FROM Events ");
            queryBuilder.Append("WHERE UserID = " + userId);
            queryBuilder.Append(" AND DateTime > '" + dt + "'");
            queryBuilder.Append(" AND DateTime < '" + dt.AddDays(1));
            queryBuilder.Append("' ORDER BY DateTime ASC;");
            string query = queryBuilder.ToString();
            List<EventModel> rc = new List<EventModel>();
            EventModel e = null;

            //if database connection is closed, open it
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

            //if database connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //for each query result, create a new EventModel object
                        //populate the EventModel object with the query results
                        //add the object to the list to be returned
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

        //gets all events by teamID and date/gets all public events for selected date
        //takes an int value as a teamID and a DateTime object
        //returns a list of EventModel objects
        public IEnumerable<EventModel> getAllEventsByTeamIDAndDate(int TeamID, DateTime dt)
        {
            //query to get all events with matching teamID and matching date
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT EventID, Name, Description, DateTime, SendAlert FROM Events ");
            queryBuilder.Append("WHERE TeamID = " + TeamID);
            queryBuilder.Append(" AND DateTime > '" + dt + "'");
            queryBuilder.Append(" AND DateTime < '" + dt.AddDays(1));
            queryBuilder.Append("' ORDER BY DateTime ASC;");
            string query = queryBuilder.ToString();
            List<EventModel> rc = new List<EventModel>();
            EventModel e = null;

            //if database connection is closed, open it
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
            //if databse connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //for each query result, create a new EventModel object
                        //populate the object with the query result
                        //add the object to the list to be returned
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
        
        //inserts private event into database
        //takes an EventModel object and an int value as a userID
        public void insertEventForUser(EventModel e, int userId)
        {
            //query to insert event with a userID into the database
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

            //if database connection is closed, open it
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
            //if database connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //inserts public event into database
        //takes an EventModel object and an int value as a teamID
        public void insertEventForTeam(EventModel e, int teamId)
        {   
            //query to isnert event into database with a teamID
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

            //if database connection is closed, open it
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
            //if database connection is open, execute query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //inserts a workout for a team into the database
        //takes a WorkoutModel object and an int value as a teamID
        public void insertWorkoutForTeam(WorkoutModel e, int teamId)
        {
            //query to insert workout with teamID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO Workouts (Date, Completed, TeamId, Description) ");
            queryBuilder.Append("VALUES ('");
            queryBuilder.Append(e.Date + "', ");
            queryBuilder.Append("0, ");
            queryBuilder.Append(teamId + ", '");
            queryBuilder.Append(e.Description + "')");
            
            string query = queryBuilder.ToString();

            //if database connection is closed, open it
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
            //if database connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        /*
         * Not yet working or implemented, dont use.
         * 
         * Will insert a workout into the database for a specific user
         */ 
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

        //updates an event in the database
        //takes an EventModel object
        public void updateEvent(EventModel e)
        {
            //query to update an event with a matching EventID
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

            //if database connection is closed, open it
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
            //if database connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //deletes an event from the database
        //takes an EventModel object
        public void deleteEvent(EventModel e)
        {
            //Query to remove an event with a matching EventID
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("DELETE FROM Events WHERE EventID = " + e.ID);
            if(e.SendAlert == true)
            {
                // logic for creating a new announcment that this event got deleted
            }
            string query = queryBuilder.ToString();

            //if database connection is closed, open it
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
            //if database connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open)
            {
                using (command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //gets all workouts by teamID
        //takes an int value as a teamID
        //returns a list of WorkoutModel objects
        public IEnumerable<WorkoutModel> GetAllCurrentWorkoutsByTeamID(int teamID)
        {
            //current date but midnight
            DateTime sqlDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            //query to get all workouts by matching teamID and have a date greater than sqlDate
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT Date, Completed, Description, WorkoutID ");
            queryBuilder.Append("FROM Workouts ");
            queryBuilder.Append("WHERE TeamID = " + teamID);
            queryBuilder.Append("AND Date > '" + sqlDate + "';");
            List<WorkoutModel> rc = new List<WorkoutModel>();
            WorkoutModel e = null;

            string query = queryBuilder.ToString();

            //if database connection is closed, open it
            if (connection.State == System.Data.ConnectionState.Closed){
                try{
                    connection.Open();
                }
                catch (SqlException ex){
                    Console.WriteLine(ex);
                }
            }
            //if database connection is open, run query
            else if (connection.State == System.Data.ConnectionState.Open){
                using (command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //for each query result, create a new WorkoutModel object
                        //populate the WorkoutModel object with the query result
                        //add the object to the list to be returned
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