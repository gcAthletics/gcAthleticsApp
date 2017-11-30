
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
        List<TableItem> tableItems = new List<TableItem>();
        ListView listView;

        string email = null;
        int teamID = -1;

        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));

            email = usrModel.Email;
            teamID = usrModel.TeamID;

            SetContentView(Resource.Layout.WorkoutsScreen);

            listView = FindViewById<ListView>(Resource.Id.workoutListView);

            try
            {
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                List<WorkoutModel> sqlList = dbu.GetAllWorkoutsByTeamID(teamID).ToList();

                foreach(var workout in sqlList){
                    tableItems.Add(new TableItem() { Heading = "Workout", SubHeading = workout.Description, DateHeading = workout.Date.ToString() });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

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
