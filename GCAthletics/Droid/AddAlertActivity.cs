/*
 * This is the class that controls the functionality of the Add Alert Page on the app. It uses the components from Resource.Layout.AddAlertLayout.axml 
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
    [Activity(Label = "Add Announcement")]
    public class AddAlertActivity : Activity
    {
        //following will hold current user data
        string email = null;
        int teamID = -1;
        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //get current user data from previous page
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            email = usrModel.Email;
            teamID = usrModel.TeamID;

            //set view to be from AddAlertLayout.axml
            SetContentView(Resource.Layout.AddAlertLayout);

            //get objects from layout resource
            EditText titleText = FindViewById<EditText>(Resource.Id.newTitleText);
            EditText descriptionText = FindViewById<EditText>(Resource.Id.newDescriptionText);
            Button postBtn = FindViewById<Button>(Resource.Id.postBtn);

            //when post button is clicked, check for any blank fields and then post the announcement
            //for the current user's team to see
            postBtn.Click += (sender, e) =>
            {
                try
                {
                    //check to see if any fields are blank
                    if (titleText.Text == "" || descriptionText.Text == "")
                    {
                        //make toast message to let user no there are blank fields
                        string toast = "One or more fields blank";
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                    }
                    //post announcement
                    else
                    {
                        //connect to database
                        DButility dbu = new DButility();
                        SqlConnection connection = dbu.createConnection();

                        //create announcement from input fields
                        AnnouncementsModel announcement = new AnnouncementsModel();
                        DateTime now = DateTime.Now;
                        announcement.Name = titleText.Text;
                        announcement.Description = descriptionText.Text;
                        announcement.DateTime = DateTime.SpecifyKind(now, DateTimeKind.Local);
                        announcement.TeamID = teamID;
                        //TODO: change this to reflect an actual ID
                        announcement.EventID = 3;

                        //post announcement
                        dbu.insertAnnouncement(announcement);

                        //make toast message saying post was successful
                        Toast.MakeText(this, "Announcement posted", ToastLength.Long).Show();

                        //go bacak to view announcements page and send current user data
                        var intent = new Intent(this, typeof(AlertsActivity));
                        usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
                        intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                        StartActivity(intent);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
            };
        }

        //when the back button is pressed, go back to the announcements page
        //also send current user data
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(AlertsActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }
    }
}