using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CustomRowView;
using Newtonsoft.Json;

namespace GCAthletics.Droid
{
    [Activity(Label = "Calendar", MainLauncher = false)]
    public class CalendarActivity : Activity
    {
        List<TableItem> tableItems = new List<TableItem>();
        ListView listView;

        string email = null;
        int teamID = -1;

        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CalendarScreen);

            var calendar = FindViewById<CalendarView>(Resource.Id.calendarView);
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));

            email = usrModel.Email;
            teamID = usrModel.TeamID;

            listView = FindViewById<ListView>(Resource.Id.eventListView);

            try
            {
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
                var time = posixTime.AddMilliseconds(calendar.Date);

                List<EventModel> sqlList = dbu.getAllEventsByUserAndDate(usrModel.ID, time.Date).ToList();

                foreach(var announcement in sqlList)
                {
                    DateTime dt;
                    if (DateTime.TryParse(announcement.DateTime.ToString(), CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out dt))
                        Console.WriteLine("successfully converted date");
                    tableItems.Add(new TableItem() { Heading = announcement.Name, SubHeading = announcement.Description, DateHeading = dt.ToString() });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            listView.Adapter = new CalendarActivityAdapter(this, tableItems);

            listView.ItemClick += OnListItemClick;

            calendar.Click += (sender, e) =>
            {
                try
                {
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();

                    var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
                    var time = posixTime.AddMilliseconds(calendar.Date);

                    List<EventModel> sqlList = dbu.getAllEventsByUserAndDate(usrModel.ID, time.Date).ToList();

                    foreach (var ev in sqlList)
                    {
                        DateTime dt;
                        if (DateTime.TryParse(ev.DateTime.ToString(), CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out dt))
                            Console.WriteLine("successfully converted date");
                        tableItems.Add(new TableItem() { Heading = ev.Name, SubHeading = ev.Description, DateHeading = dt.ToString() });
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            };
        }

        /*private void CalendarOnDateChange(object sender, CalendarView.DateChangeEventArgs args)
        {
            var newdatetime = new DateTime(args.Year, args.Month, args.DayOfMonth);

        }*/

        // when a list item is pressed
        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var item = tableItems[e.Position];
            Android.Widget.Toast.MakeText(this, item.Heading, Android.Widget.ToastLength.Short).Show();
            Console.WriteLine("Clicked on " + item.Heading);
        }

        //when back button is pressed, go to home screen
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }
    }
}