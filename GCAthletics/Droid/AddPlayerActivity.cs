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
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace GCAthletics.Droid
{
    [Activity(Label = "Add Player", MainLauncher = false)]
    public class AddPlayerActivity : Activity
    {
        string role;
        string email = null;
        int teamID = -1;

        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));

            email = usrModel.Email;
            teamID = usrModel.TeamID;

            SetContentView(Resource.Layout.AddPlayerLayout);

            EditText InsNameText = FindViewById<EditText>(Resource.Id.InsNameText);
            EditText InsEmailText = FindViewById<EditText>(Resource.Id.InsEmailText);
            EditText InsPhoneText = FindViewById<EditText>(Resource.Id.InsPhoneText);
            Spinner InsSpinner = FindViewById<Spinner>(Resource.Id.InsRoleSpinner);
            Button InsPlayerBtn = FindViewById<Button>(Resource.Id.InsPlayerBtn);

            InsSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.role_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            InsSpinner.Adapter = adapter;

            InsPlayerBtn.Click += (sender, e) =>
            {
                try
                {
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();

                    RegexUtilities RegUtil = new RegexUtilities();

                    UserModel NewUsrModel = dbu.getUserByEmail(email.ToString());

                    NewUsrModel.Name = InsNameText.Text;
                    NewUsrModel.Phone = InsPhoneText.Text;
                    NewUsrModel.Email = InsEmailText.Text;
                    NewUsrModel.Role = role;
                    NewUsrModel.TeamID = teamID;
                    NewUsrModel.IsInitial = true;

                    if (InsNameText.Text.Equals("") || InsPhoneText.Text.Equals("") || InsEmailText.Text.Equals(""))
                    {
                        string toast = "One or more fields blank";
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                    }
                    else
                    {
                        if (RegUtil.IsValidEmail(NewUsrModel.Email))
                        {
                            if (RegUtil.IsValidPhone(NewUsrModel.Phone))
                            {
                                dbu.insertUser(NewUsrModel);

                                string toast = string.Format("Added user {0} to team", NewUsrModel.Name);
                                Toast.MakeText(this, toast, ToastLength.Long).Show();

                                var intent = new Intent(this, typeof(RosterActivity));
                                usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
                                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                                StartActivity(intent);
                            }
                            else
                            {
                                string toast = "Invalid Phone Number";
                                Toast.MakeText(this, toast, ToastLength.Long).Show();
                            }
                        }
                        else
                        {
                            string toast = "Invalid Email";
                            Toast.MakeText(this, toast, ToastLength.Long).Show();
                        }
                    }
                }
                catch(SqlException ex)
                {
                    Console.WriteLine(ex);
                    string toast = "Unable to add player, please wait and try again";
                    Toast.MakeText(this, toast, ToastLength.Long).Show();
                }
            };
        }

        private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            role = (string) spinner.GetItemAtPosition(e.Position);
        }

        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(RosterActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }
    }
}