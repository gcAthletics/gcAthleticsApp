/*
 * This is the class that controls the functionality of the Add Event Page on the app. It uses the components from Resource.Layout.AddEventScreen.axml 
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
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace GCAthletics.Droid
{
    [Activity(Label = "Add Event")]
    public class AddEventActivity : Activity
    {
        //holds current user data
        string email = null;
        int teamID = -1;
        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //get current user data from previous screen
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            email = usrModel.Email;
            teamID = usrModel.TeamID;

            //this will determine if the event is public or private
            bool privateSwitchisSelected = false;

            //holds input date and input time for event
            DateTime inputdate = DateTime.Now;
            DateTime inputtime = DateTime.Now;

            //set the view to be AddEventScreen.axml
            SetContentView(Resource.Layout.AddEventScreen);

            //get components from the layout resource
            EditText titleText = FindViewById<EditText>(Resource.Id.InsEventTitleText);
            EditText descriptionText = FindViewById<EditText>(Resource.Id.InsEventDescText);
            Button chooseDateBtn = FindViewById<Button>(Resource.Id.InsEventDateBtn);
            Button chooseTimeBtn = FindViewById<Button>(Resource.Id.InsEventTimeBtn);
            Button createEventBtn = FindViewById<Button>(Resource.Id.InsEventBtn);
            Switch privateSwitch = FindViewById<Switch>(Resource.Id.privateSwitch);
            
            //whenn chooseDateBtn is clicked, pull up a date picker
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

            //when chooseTimeBtn is clicked, pull up a time picker
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

            //change value of privateSwitchisSelected whenever privateSwitch is switched on or off
            privateSwitch.CheckedChange += (Sender, e) =>
            {
                if (e.IsChecked)
                    privateSwitchisSelected = true;
                else
                    privateSwitchisSelected = false;
            };

            //when createEventBtn is clicked, check to see if all input fields are not blank and add event to the calendar
            createEventBtn.Click += (Sender, e) =>
            {
                //format date from both inputdaate and inputtime
                DateTime insDate = new DateTime(inputdate.Year, inputdate.Month, inputdate.Day, inputtime.Hour, inputtime.Minute, inputtime.Second);

                //check to see if any fields are left blank
                if (descriptionText.Text == "" || titleText.Text == "")
                {
                    //create toast message to let user know one or more fields are blank
                    string toast = "One or more fields left blank";
                    Toast.MakeText(this, toast, ToastLength.Long).Show();
                }
                else
                {
                    try
                    {
                        //connect to database
                        DButility dbu = new DButility();
                        SqlConnection connection = dbu.createConnection();

                        //create event from input fields
                        EventModel eventModel = new EventModel();
                        eventModel.DateTime = insDate;
                        eventModel.Description = descriptionText.Text;
                        eventModel.Name = titleText.Text;
                        eventModel.TeamID = usrModel.TeamID;
                        eventModel.UserID = usrModel.ID;
                        eventModel.SendAlert = false;

                        //check to see if event is private
                        //post private event and alert user
                        if (privateSwitchisSelected)
                        {
                            dbu.insertEventForUser(eventModel, usrModel.ID);

                            string toast = "Added private event to calendar";
                            Toast.MakeText(this, toast, ToastLength.Long).Show();
                        }
                        //post public event and alert user
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

                    //go back to Calendar Page and send current user data
                    var intent = new Intent(this, typeof(CalendarActivity));
                    intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                    StartActivity(intent);
                }
            };
        }
    }
}