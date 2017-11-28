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
    [Activity(Label = "Add Announcement")]
    public class AddAlertActivity : Activity
    {
        string email = null;
        int teamID = -1;

        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));

            email = usrModel.Email;
            teamID = usrModel.TeamID;

            SetContentView(Resource.Layout.AddAlertLayout);

            EditText titleText = FindViewById<EditText>(Resource.Id.newTitleText);
            EditText descriptionText = FindViewById<EditText>(Resource.Id.newDescriptionText);
            Button postBtn = FindViewById<Button>(Resource.Id.postBtn);

            postBtn.Click += (sender, e) =>
            {
                try
                {
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();

                    AnnouncementsModel announcement = new AnnouncementsModel();
                    DateTime now = DateTime.Now;
                    announcement.Name = titleText.Text;
                    announcement.Description = descriptionText.Text;
                    announcement.DateTime = DateTime.SpecifyKind(now, DateTimeKind.Local);
                    announcement.TeamID = teamID;
                    //TODO: change this to reflect an actual ID
                    announcement.EventID = 3;

                    dbu.insertAnnouncement(announcement);

                    Toast.MakeText(this, "Announcement posted", ToastLength.Long).Show();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            };
        }

        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(AlertsActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }
    }
}