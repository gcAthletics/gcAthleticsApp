/*
 * This is the class that controls the functionality of the Calendar Page on the app. It uses the components from Resource.Layout.CalendarScreen.axml 
 */

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
        //holds the differentn calendar events to display
        List<TableItem> tableItems = new List<TableItem>();
        ListView listView;

        //holds current user's data
        string email = null;
        int teamID = -1;
        UserModel usrModel = new UserModel();

        //the actual calendar that is displayed on the page
        CalendarView calendar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //set view to that of CalendarScreen.axml
            SetContentView(Resource.Layout.CalendarScreen);

            //get contents from layout resource
            calendar = FindViewById<CalendarView>(Resource.Id.calendarView);
            Button addEventBtn = FindViewById<Button>(Resource.Id.newEventBtn);
            listView = FindViewById<ListView>(Resource.Id.eventListView);

            //get current user's data from previous page
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            email = usrModel.Email;
            teamID = usrModel.TeamID;
            
            //hide addEventBtn, it will be unhidden if the current user's role is a coach
            addEventBtn.Visibility = ViewStates.Gone;

            try
            {
                //connect to database
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                //if current user's role is coach, unhide addEventBtn
                if (usrModel.Role.Equals("coach", StringComparison.InvariantCultureIgnoreCase))
                {
                    addEventBtn.Visibility = ViewStates.Visible;
                }

                //get current selected date from calendar
                var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
                var time = posixTime.AddMilliseconds(calendar.Date);
                time = time.AddHours(-5);
         
                //get all private events
                List<EventModel> sqlList = dbu.getAllEventsByUserAndDate(usrModel.ID, time.Date).ToList();
                //get all public events
                List<EventModel> sqlList2 = dbu.getAllEventsByTeamIDAndDate(usrModel.TeamID, time.Date).ToList();

                //add private events to the listView
                foreach (var announcement in sqlList)
                {
                    DateTime dt;
                    if (DateTime.TryParse(announcement.DateTime.ToString(), CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out dt))
                        Console.WriteLine("successfully converted date");
                    tableItems.Add(new TableItem() { Heading = announcement.Name, SubHeading = announcement.Description, DateHeading = dt.ToString() });
                }
                //add public events to the listView
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

            //create CalendarActivityAdatper
            CalendarActivityAdapter listAdapter = new CalendarActivityAdapter(this, tableItems);

            //set the listView's adapter to a CalendarActivityAdapter
#pragma warning disable CS0618 // Type or member is obsolete
            listView.SetAdapter(listAdapter);
#pragma warning restore CS0618 // Type or member is obsolete

            listView.ItemClick += OnListItemClick;

            //whenever a new date is selected on the calendar
            //show that date is picked
            //display any public/private events below the calendar
            calendar.DateChange += (sender, e) =>
            {
                try
                {
                    //connect to database
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();
                    //var milliSeconds = calendar.Date;
                    var oldDateTime = new DateTime(e.Year, (e.Month + 1), e.DayOfMonth);
                    //var newDate = oldDateTime.AddMilliseconds(milliSeconds);
                    //newDate = newDate.AddHours(-5);

                    /*
                     * the following is mostly the same as above, except it empties and repopulates the listView with the current
                     * selected date's events.
                     * 
                     * the following gets all private and public events for the current selected day
                     */ 
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

            //when the addEventBtn is clicked, go the Add Event Page and send the current user's data
            addEventBtn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AddEventActivity));
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };
        }

        // when a list item is pressed, don't do anything
        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var item = tableItems[e.Position];
            //Android.Widget.Toast.MakeText(this, item.Heading, Android.Widget.ToastLength.Short).Show();
            //Console.WriteLine("Clicked on " + item.Heading);
        }

        //when back button is pressed, go to home screen and send the current user's data
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }
    }
}