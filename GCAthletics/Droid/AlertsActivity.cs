/*
 * This is the class that controls the functionality of the Alerts Page on the app. It uses the components from Resource.Layout.AlertsScreen.axml 
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
using CustomRowView;
using BuiltInViews;
using System.Data.SqlClient;
using System.Globalization;
using Newtonsoft.Json;

namespace GCAthletics.Droid
{
    [Activity(Label = "Announcements", MainLauncher = false)]
    public class AlertsActivity : Activity
    {
        //holds the different announcements to view
        List<TableItem> tableItems = new List<TableItem>();
        ListView listView;

        //holds current user's data
        string email = null;
        int teamID = -1;
        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //get current user's data
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            email = usrModel.Email;
            teamID = usrModel.TeamID;

            //set view to AlertsScreen.axml
            SetContentView(Resource.Layout.AlertsScreen);

            //get contents from resource layout
            Button newAlertBtn = FindViewById<Button>(Resource.Id.newAlertBtn);
            listView = FindViewById<ListView>(Resource.Id.alertListView);

            //hide newAlertBtn, will be enabled if account is has role of coach
            newAlertBtn.Visibility = ViewStates.Gone;

            try
            {
                //connect to database
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                //if current user has role coach, show/unhide newAlertBtn
                if(usrModel.Role.Equals("coach", StringComparison.InvariantCultureIgnoreCase))
                {
                    newAlertBtn.Visibility = ViewStates.Visible;
                }

                //get all announcements for current user's team
                List<AnnouncementsModel> sqlList = dbu.getAllAnnouncements(teamID).ToList();

                //convert time of each announcement to the current user's local date and time format
                //add each announcement to tableItems so they can be viewed properly
                foreach(var announcement in sqlList)
                {
                    DateTime dt;
                    if (DateTime.TryParse(announcement.DateTime.ToString(), CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out dt))
                        Console.WriteLine("successfully converted date");
                    tableItems.Add(new TableItem() { Heading = announcement.Name, SubHeading = announcement.Description, DateHeading = dt.ToString() });
                }
            }
            catch (SqlException ex){
                Console.WriteLine(ex.ToString());
            }

            //attach AlertsActivityAdapter to the listView, this will populate the listView
            listView.Adapter = new AlertsActivityAdapter(this, tableItems);

            //when newAlertBtn is clicked, go to the Add Alert Screen and send the current user's data
            newAlertBtn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AddAlertActivity));
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };

        }

        //when back button is pressed, go to home screen
        //send the current user's data
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }
    }
}