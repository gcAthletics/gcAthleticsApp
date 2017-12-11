/*
 * This is the class that controls the functionality of the Home Page on the app. It uses the components from Resource.Layout.HomeScreen.axml 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GCAthletics.Droid
{
    [Activity(Label = "Home", MainLauncher = false)]
    public class HomeActivity : Activity
    {
        //this holds the current user data
        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //get the current user data from the previous screen
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            var email = usrModel.Email;
            int teamID = usrModel.TeamID;

            //set the view to be from HomeScreen.axml
            SetContentView(Resource.Layout.HomeScreen);

            //get text view objects from the layout resource
            TextView nameTxt = FindViewById<TextView>(Resource.Id.textName);
            TextView teamTxt = FindViewById<TextView>(Resource.Id.textTeam);

            // Get buttons from the layout resource
            ImageButton calendarButton = FindViewById<ImageButton>(Resource.Id.calendarImgBtn);
            ImageButton workoutsButton = FindViewById<ImageButton>(Resource.Id.workoutImgBtn);
            ImageButton alertsButton = FindViewById<ImageButton>(Resource.Id.alertsImgBtn);
            ImageButton rosterButton = FindViewById<ImageButton>(Resource.Id.rosterImgBtn);

            //get team information to display on the home screen
            try
            {
                //connect to the database
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                //get team information
                TeamModel teamModel = dbu.getTeamById(usrModel.TeamID);

                //display team information
                nameTxt.Text = usrModel.Name;
                teamTxt.Text = teamModel.Name;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }


            //when nameTxt is clicked (the current user's name dispayed on the home page)
            //open up an options menu to logout or change password
            nameTxt.Click += (sender, e) =>
            {
                //build options menu
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Options");
                alert.SetMessage("What would you like to do?");
                //option 1, backout of options menu
                alert.SetButton("Back", (c, ev) =>
                {
                    alert.Hide();
                });
                //option 2, logout and return to login screen
                alert.SetButton2("Logout", (c, ev) =>
                {
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                });
                //option 3, change current user's password
                alert.SetButton3("Change Password", (c, ev) => 
                {
                    var intent = new Intent(this, typeof(PasswordActivity));
                    //send current user data to next screen
                    intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                    StartActivity(intent);
                });
                //shop options menu
                alert.Show();
            };

            // when calendarButton is clicked, open up CalendarScreen.axml
            // also start activity CalendarActivity.cs (activity controlling actions for the calendar screen)
            calendarButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CalendarActivity));
                //send current user data to next screen
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };

            // when alertsButton is clicked, open up AlertScreen.axml
            // also start activity AlertsActivity.cs (activity controlling actions for the calendar screen)
            alertsButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AlertsActivity));
                //send current user data to next screen
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };

            // when rosterButton is clicked, open up RosterScreen.axml
            // also start activity RosterActivity.cs (activity controlling actions for the roster screen)
            rosterButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(RosterActivity));
                //send current user data to next screen
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };

            // when rosterButton is clicked, open up WorkoutsScreen.axml
            // also start activity WorkoutActivity.cs (activity controlling actions for the workout screen)
            workoutsButton.Click += (sender, e) =>
            {
                var intent = new Intent();

                if (usrModel.Role.Equals("coach", StringComparison.InvariantCultureIgnoreCase))
                {
                    intent = new Intent(this, typeof(AddWorkoutActivity));
                }
                else
                {
                    intent = new Intent(this, typeof(WorkoutActivity));
                }

                //send current user data to next screen
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };
        }
    }
}