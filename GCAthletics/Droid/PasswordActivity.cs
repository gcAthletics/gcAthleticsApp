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
    [Activity(Label = "Change Password")]
    public class PasswordActivity : Activity
    {
        string email;
        int teamID = -1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PasswordLayout);

            email = Intent.Extras.GetString("email");
            teamID = Intent.Extras.GetInt("teamID");

            EditText changePwdText = FindViewById<EditText>(Resource.Id.changePwdText);
            EditText confirmPwdText = FindViewById<EditText>(Resource.Id.confirmPwdText);
            Button changePwdBtn = FindViewById<Button>(Resource.Id.updatePwdBtn);

            changePwdBtn.Click += (sender, e) =>
            {
                try
                {
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();

                    if (changePwdText.Text.Equals(confirmPwdText.Text))
                    {
                        dbu.changePassword(confirmPwdText.Text, email);
                        var intent = new Intent(this, typeof(HomeActivity));
                        intent.PutExtra("email", email);
                        intent.PutExtra("teamID", teamID);
                        StartActivity(intent);
                    }
                    else
                    {
                        Toast.MakeText(this, "Passwords do not match", ToastLength.Long).Show();
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