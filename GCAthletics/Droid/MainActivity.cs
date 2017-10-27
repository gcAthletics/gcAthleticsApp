using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Data.SqlClient;
using System;
using System.Text;
using System.Security.Cryptography;

namespace GCAthletics.Droid
{
    [Activity(Label = "GC Athletics", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var imageView = FindViewById<ImageView>(Resource.Id.thunderImage);
            imageView.SetImageResource(Resource.Mipmap.thunder);

            var emailField = FindViewById<EditText>(Resource.Id.emailField);
            var passwordField = FindViewById<EditText>(Resource.Id.passwordField);

            // Get login button from the layout resource,
            // and attach an event to it
            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);

            // when login button is clicked, open up HomeScreen.axml (the app home screen)
            // also start activity HomeActivity.cs (activity controlling actions for the app home screen)
            loginButton.Click += async (sender, e) =>
            {
                bool isCorrectLogin = false;

                var email = emailField.Text;
                var password = passwordField.Text;

                try
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = "gcathletics.database.windows.net";
                    builder.UserID = "gcAdmin";
                    builder.Password = "ADMINpassword1!";
                    builder.InitialCatalog = "GCathleticsDB";

                    SqlConnection connection = new SqlConnection(builder.ConnectionString);
                    connection.Open();
                    StringBuilder query = new StringBuilder();
                    query.Append("SELECT Email, PasswordHash FROM Users ");
                    query.Append("WHERE Email = '");
                    query.Append(email);
                    query.Append("';");
                    string sql = query.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine(reader.GetString(1));
                                if (reader.GetString(1).Equals(passwordHash(password), StringComparison.InvariantCultureIgnoreCase))
                                {
                                    isCorrectLogin = true;
                                }
                            }
                        }
                    }


                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                if (isCorrectLogin)
                {
                    var intent = new Intent(this, typeof(HomeActivity));
                    StartActivity(intent);
                }
            };
        }

        // Method to hash the password
        // returns a 256 bit hash value as a string
        public static string passwordHash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}

