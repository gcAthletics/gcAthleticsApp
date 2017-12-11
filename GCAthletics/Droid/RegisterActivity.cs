/*
 * This is the class that controls the functionality of the Register Team Page on the app. It uses the components from Resource.Layout.RegisterScreen.axml 
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

namespace GCAthletics.Droid
{
    [Activity(Label = "Register Team")]
    public class RegisterActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //set the view to that of RegisterScreen.axml
            SetContentView(Resource.Layout.RegisterScreen);

            //get all contents from layout resource
            EditText regNameText = FindViewById<EditText>(Resource.Id.InsRegName);
            EditText regPhoneText = FindViewById<EditText>(Resource.Id.InsRegPhone);
            EditText regEmailText = FindViewById<EditText>(Resource.Id.InsRegEmail);
            EditText regTeamNameText = FindViewById<EditText>(Resource.Id.InsRegTeamName);
            EditText regSportText = FindViewById<EditText>(Resource.Id.InsRegSport);
            Button regButton = FindViewById<Button>(Resource.Id.registerTeamBtn);

            //when regButton is clicked, check for any blank fields, or incorrectly formatted data
            //add new team to database, and new coach account to database
            //the new account's temporary password is "gobobcats1" without quotes
            //return to the login page
            regButton.Click += (sender, e) =>
            {
                //this is used to check input fields are correctly formatted 
                RegexUtilities rx = new RegexUtilities();

                //hold the new user and team's data
                UserModel usrModel = new UserModel();
                TeamModel teamModel = new TeamModel();

                //check to see if any fields are blank, if not continue on
                if(regNameText.Text != "" && regPhoneText.Text != "" && regEmailText.Text != ""
                    && regTeamNameText.Text != "" && regSportText.Text != "")
                {
                    //set new user's name to the name that was input
                    usrModel.Name = regNameText.Text;
                    //check to see if input email is correctly formatted
                    if (rx.IsValidEmail(regEmailText.Text))
                    {
                        //set's users email to input email
                        usrModel.Email = regEmailText.Text;
                        //check to see if input phone number is correctly formatted
                        if (rx.IsValidPhone(regPhoneText.Text))
                        {
                            //create new team and user from input data
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
                                //connect to database
                                DButility dbu = new DButility();
                                SqlConnection connection = dbu.createConnection();

                                //register team and get teamID for new user
                                usrModel.TeamID = dbu.RegisterTeam(teamModel);

                                //insert new user to database
                                dbu.insertUser(usrModel);

                                //alert user that the new team was registered
                                string toast = "Registered new team " + teamModel.Name;
                                Toast.MakeText(this, toast, ToastLength.Long).Show();

                                //go back to log in screen
                                var intent = new Intent(this, typeof(MainActivity));
                                StartActivity(intent);
                            }
                            catch(SqlException ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        //alert user of invalid phone number
                        else
                        {
                            string toast = "Invalid Phone number";
                            Toast.MakeText(this, toast, ToastLength.Long).Show();
                        }
                    }
                    //alert user of invalid email
                    else
                    {
                        string toast = "Invalid email";
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                    }
                }
                //alert user of any blank fields
                else
                {
                    string toast = "One or more fields blank";
                    Toast.MakeText(this, toast, ToastLength.Long).Show();
                }
            };
        }
    }
}

