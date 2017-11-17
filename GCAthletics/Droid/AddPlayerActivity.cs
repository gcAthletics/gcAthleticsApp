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
    [Activity(Label = "Add Player", MainLauncher = false)]
    public class AddPlayerActivity : Activity
    {
        string role;
        string email = null;
        int teamID = -1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            email = Intent.Extras.GetString("email");
            teamID = Intent.Extras.GetInt("teamID");

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

                    UserModel usrModel = dbu.getUserByEmail(email.ToString());

                    usrModel.Name = InsNameText.Text;
                    usrModel.Phone = InsPhoneText.Text;
                    usrModel.Email = InsEmailText.Text;
                    usrModel.Role = role;
                    usrModel.TeamID = teamID;
                    usrModel.IsInitial = true;

                    dbu.insertUser(usrModel);

                    string toast = string.Format("Added user {0} to team", usrModel.Name);
                    Toast.MakeText(this, toast, ToastLength.Long).Show();
                }
                catch(SqlException ex)
                {
                    Console.WriteLine(ex);
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
            var intent = new Intent(this, typeof(HomeActivity));
            intent.PutExtra("email", email);
            intent.PutExtra("teamID", teamID);
            StartActivity(intent);
        }
    }
}