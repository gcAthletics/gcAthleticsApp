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
using Android.Accounts;

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
            loginButton.Click += (sender, e) =>
            {
                bool isCorrectLogin = false;

                var email = emailField.Text;
                var password = passwordField.Text;

                try
                {
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();

                    isCorrectLogin = dbu.appLogin(email, password);

                    connection.Close();
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
                else
                {
                    Toast.MakeText(this, "Incorrect Email/Password Combination", ToastLength.Long).Show();
                }
            };
        }
    }
}

