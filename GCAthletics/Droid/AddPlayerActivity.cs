/*
 * This is the class that controls the functionality of the Add Player Page on the app. It uses the components from Resource.Layout.AddPlayerLayout.axml 
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

namespace GCAthletics.Droid
{
    [Activity(Label = "Add Player", MainLauncher = false)]
    public class AddPlayerActivity : Activity
    {
        //holds value for new account's role
        string role;

        //holds current user's data
        string email = null;
        int teamID = -1;
        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //get current user's data from previous screen
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            email = usrModel.Email;
            teamID = usrModel.TeamID;

            //set view to AddPlayerLayout.axml
            SetContentView(Resource.Layout.AddPlayerLayout);

            //get contents from layout resource
            EditText InsNameText = FindViewById<EditText>(Resource.Id.InsNameText);
            EditText InsEmailText = FindViewById<EditText>(Resource.Id.InsEmailText);
            EditText InsPhoneText = FindViewById<EditText>(Resource.Id.InsPhoneText);
            Spinner InsSpinner = FindViewById<Spinner>(Resource.Id.InsRoleSpinner);
            Button InsPlayerBtn = FindViewById<Button>(Resource.Id.InsPlayerBtn);

            //add event handler to the role selector spinner
            InsSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            //get choices for role selector spinner
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.role_array, Android.Resource.Layout.SimpleSpinnerItem);
            //set choices for role selector spinner
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            InsSpinner.Adapter = adapter;

            //when InsPlayerBtn is clicked, check to see if any fields are blank or incorrectly formatted
            //add new player/coach to team
            InsPlayerBtn.Click += (sender, e) =>
            {
                try
                {
                    //connect to the database
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();

                    //used to see if input fields are formatted correctly
                    RegexUtilities RegUtil = new RegexUtilities();


                    //UserModel NewUsrModel = dbu.getUserByEmail(email.ToString());

                    //create new user from input fields
                    UserModel NewUsrModel = new UserModel();
                    NewUsrModel.Name = InsNameText.Text;
                    NewUsrModel.Phone = InsPhoneText.Text;
                    NewUsrModel.Email = InsEmailText.Text;
                    NewUsrModel.Role = role;
                    NewUsrModel.TeamID = teamID;
                    NewUsrModel.IsInitial = true;

                    //check for blank fields and alert user
                    if (InsNameText.Text.Equals("") || InsPhoneText.Text.Equals("") || InsEmailText.Text.Equals(""))
                    {
                        string toast = "One or more fields blank";
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                    }
                    else
                    {
                        //check for invalid email format
                        if (RegUtil.IsValidEmail(NewUsrModel.Email))
                        {
                            //check for invalid phone number format
                            if (RegUtil.IsValidPhone(NewUsrModel.Phone))
                            {
                                //insert new account to team and database
                                dbu.insertUser(NewUsrModel);

                                //alert current user that the new user was added to the team
                                string toast = string.Format("Added user {0} to team", NewUsrModel.Name);
                                Toast.MakeText(this, toast, ToastLength.Long).Show();

                                //go back to the Roster Page and send current user's data
                                var intent = new Intent(this, typeof(RosterActivity));
                                usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
                                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                                StartActivity(intent);
                            }
                            else
                            {
                                //alert user of invalid phone number
                                string toast = "Invalid Phone Number";
                                Toast.MakeText(this, toast, ToastLength.Long).Show();
                            }
                        }
                        else
                        {
                            //alert user of invalid email
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

        //when role selector spinner is used, set role to be the role chosen
        private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            role = (string) spinner.GetItemAtPosition(e.Position);
        }

        //when the back button is pressed, go to the Roster Page and send the current user's data
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(RosterActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }
    }
}