/*
 * This is the class that controls the functionality of the Change Password Page on the app. It uses the components from Resource.Layout.PaasswordLayout.axml 
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
    [Activity(Label = "Change Password")]
    public class PasswordActivity : Activity
    {
        //holds current user's data
        string email;
        int teamID = -1;
        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //set the view to that of PasswordLayout.axml
            SetContentView(Resource.Layout.PasswordLayout);

            //get current user's data
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            email = usrModel.Email;
            teamID = usrModel.TeamID;

            //get all contents from layout resource
            EditText changePwdText = FindViewById<EditText>(Resource.Id.changePwdText);
            EditText confirmPwdText = FindViewById<EditText>(Resource.Id.confirmPwdText);
            Button changePwdBtn = FindViewById<Button>(Resource.Id.updatePwdBtn);

            //when changePwdBtn is clicked
            // 1, check to see if passwords are the same
            // 2, check to see if passwords are not empty
            // 3, if 1 and 2 are true, set the user's password to the hash of the entered password
            changePwdBtn.Click += (sender, e) =>
            {
                try
                {
                    //connect to database
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();

                    //check for the same password in both fields and if either are blank
                    if (changePwdText.Text.Equals(confirmPwdText.Text) && !confirmPwdText.Text.Equals(""))
                    {
                        //change password for currennt user
                        dbu.changePassword(confirmPwdText.Text, email);
                        //go to the Home Page and also send the current user's data
                        var intent = new Intent(this, typeof(HomeActivity));
                        intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                        StartActivity(intent);
                    }
                    //alert user the passwords don't match or are blank
                    else
                    {
                        Toast.MakeText(this, "Passwords do not match or are blank", ToastLength.Long).Show();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            };
        }
    }
}