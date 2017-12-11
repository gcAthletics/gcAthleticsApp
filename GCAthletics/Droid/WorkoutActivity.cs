/*
 * This is the class that controls the functionality of the Workout Page on the app. It uses the components from Resource.Layout.WorkoutsScreen.axml 
 */

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BuiltInViews;
using CustomRowView;
using Newtonsoft.Json;

namespace GCAthletics.Droid
{
    [Activity(Label = "Workouts")]
    public class WorkoutActivity : Activity
    {
        //holds all workouts
        List<TableItem> tableItems = new List<TableItem>();
        ListView listView;

        //holds current user's data
        string email = null;
        int teamID = -1;
        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //get current user's data
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            email = usrModel.Email;
            teamID = usrModel.TeamID;

            //set view to that of workoutsScreen.axml
            SetContentView(Resource.Layout.WorkoutsScreen);

            //get contents of layout resource
            listView = FindViewById<ListView>(Resource.Id.workoutListView);

            try
            {
                //connect to database
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                //get all workouts from today and future
                List<WorkoutModel> sqlList = dbu.GetAllCurrentWorkoutsByTeamID(teamID).ToList();

                //for each workout create a new tableItem object
                foreach(var workout in sqlList){
                    tableItems.Add(new TableItem() { Heading = "Workout", SubHeading = workout.Description, DateHeading = workout.Date.ToShortDateString() });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

            //populate the listView using AlertsActivityAdapter and the tableItems list of workouts
            listView.Adapter = new AlertsActivityAdapter(this, tableItems);

        }

        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }
    }
}
