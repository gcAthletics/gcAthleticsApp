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
        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            var email = usrModel.Email;
            int teamID = usrModel.TeamID;



            SetContentView(Resource.Layout.HomeScreen);
            // Create your application here

            //get text view objects from the layout resource
            TextView nameTxt = FindViewById<TextView>(Resource.Id.textName);
            TextView teamTxt = FindViewById<TextView>(Resource.Id.textTeam);

            // Get buttons from the layout resource,
            // and attach an event to it
            ImageButton calendarButton = FindViewById<ImageButton>(Resource.Id.calendarImgBtn);
            ImageButton workoutsButton = FindViewById<ImageButton>(Resource.Id.workoutImgBtn);
            ImageButton alertsButton = FindViewById<ImageButton>(Resource.Id.alertsImgBtn);
            ImageButton rosterButton = FindViewById<ImageButton>(Resource.Id.rosterImgBtn);

            try
            {
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                TeamModel teamModel = dbu.getTeamById(usrModel.TeamID);

                nameTxt.Text = usrModel.Name;
                teamTxt.Text = teamModel.Name;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }


            nameTxt.Click += (sender, e) =>
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Options");
                alert.SetMessage("What would you like to do?");
                alert.SetButton("Back", (c, ev) =>
                {
                    alert.Hide();
                });
                alert.SetButton2("Logout", (c, ev) =>
                {
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                });
                alert.SetButton3("Change Password", (c, ev) => 
                {
                    var intent = new Intent(this, typeof(PasswordActivity));
                    intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                    StartActivity(intent);
                });
                alert.Show();
            };

            // when calendarButton is clicked, open up CalendarScreen.axml
            // also start activity CalendarActivity.cs (activity controlling actions for the calendar screen)
            calendarButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CalendarActivity));
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };

            // when alertsButton is clicked, open up AlertScreen.axml
            // also start activity AlertsActivity.cs (activity controlling actions for the calendar screen)
            alertsButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AlertsActivity));
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };

            // when rosterButton is clicked, open up RosterScreen.axml
            // also start activity RosterActivity.cs (activity controlling actions for the roster screen)
            rosterButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(RosterActivity));
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };

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
                
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };
        }
    }
}