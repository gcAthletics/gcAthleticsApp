/*
 * This is the class that controls the functionality of the Add Workout Page on the app. It uses the components from Resource.Layout.AddWorkoutScreen.axml 
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
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace GCAthletics.Droid
{
    [Activity(Label = "Assign Workout")]
    public class AddWorkoutActivity : Activity
    {
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

            //holds new workout's data
            WorkoutModel workModel = new WorkoutModel();

            //set view to AddWorkoutScreen.axml
            SetContentView(Resource.Layout.AddWorkoutScreen);

            //get contents from layout resource
            EditText detailsText = FindViewById<EditText>(Resource.Id.InsWorkText);
            Button dateBtn = FindViewById<Button>(Resource.Id.InsWorkDateBtn);
            Button assignBtn = FindViewById<Button>(Resource.Id.InsWorkBtn);
            Button viewWorkBtn = FindViewById<Button>(Resource.Id.viewWorkBtn);

            //when dateBtn is clicked show a date picker
            dateBtn.Click += (sender, e) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    workModel.Date = time;
                    dateBtn.Text = time.ToShortDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            //when assignBtn is clicked, check for blank fields and assign workout to team
            assignBtn.Click += (sender, e) =>
            {
                //check for blank fields and alert user
                if (detailsText.Text == "")
                {
                    String toast = "Please enter workout details";
                    Toast.MakeText(this, toast, ToastLength.Long).Show();
                }
                else
                {
                    try
                    {
                        //connect to database
                        DButility dbu = new DButility();
                        SqlConnection connection = dbu.createConnection();

                        //create workout from input fields
                        workModel.Completed = false;
                        workModel.TeamID = usrModel.TeamID;
                        workModel.Description = detailsText.Text;

                        //assign workout to team
                        dbu.insertWorkoutForTeam(workModel, usrModel.TeamID);

                        //alert user workout was assigned
                        string toast = "Assigned workout to teaam";
                        Toast.MakeText(this, toast, ToastLength.Long).Show();

                        //go to view Workouts Page and send current user data
                        var intent = new Intent(this, typeof(WorkoutActivity));
                        usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
                        intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                        StartActivity(intent);
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            };

            //when viewWorkBtn is clicked go to the view Workouts Page
            //also send current user's data
            viewWorkBtn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(WorkoutActivity));
                usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };
        }
        
        //when the back button is pressed, go to the Home Page and send current user's data
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }
    }
}