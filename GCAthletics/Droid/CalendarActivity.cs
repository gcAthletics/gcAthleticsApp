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
        CalendarView calendar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CalendarScreen);

            calendar = FindViewById<CalendarView>(Resource.Id.calendarView);
            Button addEventBtn = FindViewById<Button>(Resource.Id.newEventBtn);
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));

            email = usrModel.Email;
            teamID = usrModel.TeamID;

            listView = FindViewById<ListView>(Resource.Id.eventListView);

            addEventBtn.Visibility = ViewStates.Gone;

            try
            {
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                if (usrModel.Role.Equals("coach", StringComparison.InvariantCultureIgnoreCase))
                {
                    addEventBtn.Visibility = ViewStates.Visible;
                }

                var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
                var time = posixTime.AddMilliseconds(calendar.Date);
                time = time.AddHours(-5);
         
                List<EventModel> sqlList = dbu.getAllEventsByUserAndDate(usrModel.ID, time.Date).ToList();
                List<EventModel> sqlList2 = dbu.getAllEventsByTeamIDAndDate(usrModel.TeamID, time.Date).ToList();

                foreach (var announcement in sqlList)
                {
                    DateTime dt;
                    if (DateTime.TryParse(announcement.DateTime.ToString(), CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out dt))
                        Console.WriteLine("successfully converted date");
                    tableItems.Add(new TableItem() { Heading = announcement.Name, SubHeading = announcement.Description, DateHeading = dt.ToString() });
                }
                foreach (var ev in sqlList2)
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

            CalendarActivityAdapter listAdapter = new CalendarActivityAdapter(this, tableItems);

#pragma warning disable CS0618 // Type or member is obsolete
            listView.SetAdapter(listAdapter);
#pragma warning restore CS0618 // Type or member is obsolete

            listView.ItemClick += OnListItemClick;

            calendar.DateChange += (sender, e) =>
            {
                try
                {
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();
                    //var milliSeconds = calendar.Date;
                    var oldDateTime = new DateTime(e.Year, (e.Month + 1), e.DayOfMonth);
                    //var newDate = oldDateTime.AddMilliseconds(milliSeconds);
                    //newDate = newDate.AddHours(-5);

                    List<EventModel> sqlList = dbu.getAllEventsByUserAndDate(usrModel.ID, oldDateTime).ToList();
                    List<EventModel> sqlList2 = dbu.getAllEventsByTeamIDAndDate(usrModel.TeamID, oldDateTime).ToList();
                    tableItems.Clear();
                    foreach (var ev in sqlList)
                    {
                        DateTime dt;
                        if (DateTime.TryParse(ev.DateTime.ToString(), CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out dt))
                            Console.WriteLine("successfully converted date");
                        tableItems.Add(new TableItem() { Heading = ev.Name, SubHeading = ev.Description, DateHeading = dt.ToString() });
                        listAdapter.NotifyDataSetChanged();
                    }
                    foreach (var ev in sqlList2)
                    {
                        DateTime dt;
                        if (DateTime.TryParse(ev.DateTime.ToString(), CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out dt))
                            Console.WriteLine("successfully converted date");
                        tableItems.Add(new TableItem() { Heading = ev.Name, SubHeading = ev.Description, DateHeading = dt.ToString() });
                        listAdapter.NotifyDataSetChanged();
                    }
                    listAdapter.NotifyDataSetChanged();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            };

            addEventBtn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AddEventActivity));
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
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
            //Android.Widget.Toast.MakeText(this, item.Heading, Android.Widget.ToastLength.Short).Show();
            //Console.WriteLine("Clicked on " + item.Heading);
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