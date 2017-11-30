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
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace GCAthletics.Droid
{
    [Activity(Label = "Add Event")]
    public class AddEventActivity : Activity
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

            bool privateSwitchisSelected = false;

            DateTime inputdate = DateTime.Now;
            DateTime inputtime = DateTime.Now;

            SetContentView(Resource.Layout.AddEventScreen);

            EditText titleText = FindViewById<EditText>(Resource.Id.InsEventTitleText);
            EditText descriptionText = FindViewById<EditText>(Resource.Id.InsEventDescText);
            Button chooseDateBtn = FindViewById<Button>(Resource.Id.InsEventDateBtn);
            Button chooseTimeBtn = FindViewById<Button>(Resource.Id.InsEventTimeBtn);
            Button createEventBtn = FindViewById<Button>(Resource.Id.InsEventBtn);
            Switch privateSwitch = FindViewById<Switch>(Resource.Id.privateSwitch);
            
            chooseDateBtn.Click += (Sender, e) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    inputdate = time;
                    chooseDateBtn.Text = inputdate.ToString();
                    Console.WriteLine("Date selected is: " + inputdate.ToString());
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            chooseTimeBtn.Click += (Sender, e) =>
            {
                TimePickerFragment frag = TimePickerFragment.NewInstance(
                    delegate (DateTime time2)
                    {
                        inputtime = time2;
                        StringBuilder timeBuilder = new StringBuilder();
                        timeBuilder.Append(inputtime.Hour);
                        timeBuilder.Append(":");
                        timeBuilder.Append(inputtime.Minute);
                        chooseTimeBtn.Text = timeBuilder.ToString();
                    });
                frag.Show(FragmentManager, TimePickerFragment.TAG);
            };

            privateSwitch.CheckedChange += (Sender, e) =>
            {
                if (e.IsChecked)
                    privateSwitchisSelected = true;
                else
                    privateSwitchisSelected = false;
            };

            createEventBtn.Click += (Sender, e) =>
            {
                DateTime insDate = new DateTime(inputdate.Year, inputdate.Month, inputdate.Day, inputtime.Hour, inputtime.Minute, inputtime.Second);

                try
                {
                    DButility dbu = new DButility();
                    SqlConnection connection = dbu.createConnection();

                    EventModel eventModel = new EventModel();
                    eventModel.DateTime = insDate;
                    eventModel.Description = descriptionText.Text;
                    eventModel.Name = titleText.Text;
                    eventModel.TeamID = usrModel.TeamID;
                    eventModel.UserID = usrModel.ID;
                    eventModel.SendAlert = false;

                    if (privateSwitchisSelected)
                    {
                        dbu.insertEventForUser(eventModel, usrModel.ID);

                        string toast = "Added private event to calendar";
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                    }
                    else
                    {
                        eventModel.SendAlert = true;
                        dbu.insertEventForTeam(eventModel, teamID);

                        string toast = "Added team event to calendar";
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                    }
                    
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }

                //string toast = "Added event to calendar";
                //Toast.MakeText(this, toast, ToastLength.Long).Show();

                var intent = new Intent(this, typeof(CalendarActivity));
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };
        }
    }
}