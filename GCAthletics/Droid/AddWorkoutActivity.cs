﻿using System;
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
        string email = null;
        int teamID = -1;
        int selectedUser;
        UserModel usrModel = new UserModel();
       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            WorkoutModel workModel = new WorkoutModel();
            email = usrModel.Email;
            teamID = usrModel.TeamID;
       
            SetContentView(Resource.Layout.AddWorkoutScreen);

            EditText detailsText = FindViewById<EditText>(Resource.Id.InsWorkText);
            Button dateBtn = FindViewById<Button>(Resource.Id.InsWorkDateBtn);
            Button assignBtn = FindViewById<Button>(Resource.Id.InsWorkBtn);

            dateBtn.Click += (sender, e) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    workModel.Date = time;
                    dateBtn.Text = time.ToString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            assignBtn.Click += (sender, e) =>
            {
                try
                {
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();

                    workModel.Completed = false;
                    workModel.TeamID = usrModel.TeamID;

                    dbu.insertWorkoutForTeam(workModel, usrModel.TeamID);

                    string toast = "Assigned workout to teaam";
                    Toast.MakeText(this, toast, ToastLength.Long).Show();

                    var intent = new Intent(this, typeof(HomeActivity));
                    usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
                    intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                    StartActivity(intent);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            };
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