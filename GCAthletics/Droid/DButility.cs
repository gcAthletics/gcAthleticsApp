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
        public DButility() {}

        private SqlConnectionStringBuilder sqlConBuilder = new SqlConnectionStringBuilder();

        static SqlConnection connection = null;

        public SqlConnection createConnection() {
            sqlConBuilder.DataSource = "gcathletics.database.windows.net";
            sqlConBuilder.UserID = "gcAdmin";
            sqlConBuilder.Password = "ADMINpassword1!";
            sqlConBuilder.InitialCatalog = "GCathleticsDB";

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

            if(connection.State == System.Data.ConnectionState.Closed)
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
            else if(connection.State == System.Data.ConnectionState.Open)
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    Object result = command.ExecuteScalar();

                    if(result != null)
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

    }
}