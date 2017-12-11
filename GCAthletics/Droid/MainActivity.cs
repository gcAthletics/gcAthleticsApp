/*
 * This is the class that controls the functionality of the Login Page on the app. It uses the components from Resource.Layout.Main.axml 
 */

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
    [Activity(Label = "GC Athletics", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //get all contents from layout resource
            var imageView = FindViewById<ImageView>(Resource.Id.thunderImage);
            var emailField = FindViewById<EditText>(Resource.Id.emailField);
            var passwordField = FindViewById<EditText>(Resource.Id.passwordField);
            TextView registerText = FindViewById<TextView>(Resource.Id.registerText);

            //set imageView's image to thunder.png
            imageView.SetImageResource(Resource.Mipmap.thunder);

            // Get login button from the layout resource,
            // and attach an event to it
            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);

            // when login button is clicked, open up HomeScreen.axml (the app home screen)
            // also start activity HomeActivity.cs (activity controlling actions for the app home screen)
            loginButton.Click += (sender, e) =>
            {

                //get email and password input from user
                var email = emailField.Text;
                var password = passwordField.Text;

                try
                {
                    //connect to database
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();

                    //if login is successful, close db connection and go to home screen
                    if (dbu.appLogin(email, password))
                    {
                        UserModel usrModel = dbu.getUserByEmail(email);
                        connection.Close();
                        var intent = new Intent();
                        if (usrModel.IsInitial)
                        {
                            intent = new Intent(this, typeof(PasswordActivity));
                        }
                        else
                        {
                            intent = new Intent(this, typeof(HomeActivity));
                        }
                        //send data for the user to the next screen
                        intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                        StartActivity(intent);
                    }
                    //if login isn't successful, display toast error message
                    else
                    {
                        Toast.MakeText(this, "Incorrect Email/Password Combination", ToastLength.Long).Show();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }               
            };

            //if registerText is clicked, it it will open up the Register Screen
            registerText.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(RegisterActivity));
                StartActivity(intent);
            };
        }

        //if the back button is pressed, the app will be closed
        public override void OnBackPressed()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}

