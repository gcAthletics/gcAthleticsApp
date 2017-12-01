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

namespace GCAthletics.Droid
{
    [Activity(Label = "Register Team")]
    public class RegisterActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RegisterScreen);

            EditText regNameText = FindViewById<EditText>(Resource.Id.InsRegName);
            EditText regPhoneText = FindViewById<EditText>(Resource.Id.InsRegPhone);
            EditText regEmailText = FindViewById<EditText>(Resource.Id.InsRegEmail);
            EditText regTeamNameText = FindViewById<EditText>(Resource.Id.InsRegTeamName);
            EditText regSportText = FindViewById<EditText>(Resource.Id.InsRegSport);
            Button regButton = FindViewById<Button>(Resource.Id.registerTeamBtn);

            regButton.Click += (sender, e) =>
            {
                RegexUtilities rx = new RegexUtilities();

                UserModel usrModel = new UserModel();
                TeamModel teamModel = new TeamModel();
                if(regNameText.Text != "" && regPhoneText.Text != "" && regEmailText.Text != ""
                    && regTeamNameText.Text != "" && regSportText.Text != "")
                {
                    usrModel.Name = regNameText.Text;
                    if (rx.IsValidEmail(regEmailText.Text))
                    {
                        usrModel.Email = regEmailText.Text;
                        if (rx.IsValidPhone(regPhoneText.Text))
                        {
                            teamModel.Name = regTeamNameText.Text;
                            teamModel.Sport = regSportText.Text;
                            teamModel.Wins = 0;
                            teamModel.Losses = 0;
                            teamModel.Coach = regNameText.Text;
                            usrModel.Phone = regPhoneText.Text;
                            usrModel.IsInitial = true;
                            usrModel.Role = "coach";

                            try
                            {
                                DButility dbu = new DButility();
                                SqlConnection connection = dbu.createConnection();

                                usrModel.TeamID = dbu.RegisterTeam(teamModel);
                                dbu.insertUser(usrModel);

                                string toast = "Registered new team " + teamModel.Name;
                                Toast.MakeText(this, toast, ToastLength.Long).Show();

                                var intent = new Intent(this, typeof(MainActivity));
                                StartActivity(intent);
                            }
                            catch(SqlException ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        else
                        {
                            string toast = "Invalid Phone number";
                            Toast.MakeText(this, toast, ToastLength.Long).Show();
                        }
                    }
                    else
                    {
                        string toast = "Invalid email";
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                    }
                }
                else
                {
                    string toast = "One or more fields blank";
                    Toast.MakeText(this, toast, ToastLength.Long).Show();
                }
            };
        }
    }
}

