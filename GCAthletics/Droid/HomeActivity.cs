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

namespace GCAthletics.Droid
{
    [Activity(Label = "HomeActivity", MainLauncher = false)]
    public class HomeActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.HomeScreen);
            // Create your application here

            // Get buttons from the layout resource,
            // and attach an event to it
            ImageButton calendarButton = FindViewById<ImageButton>(Resource.Id.calendarImgBtn);
            ImageButton workoutsButton = FindViewById<ImageButton>(Resource.Id.workoutImgBtn);
            ImageButton alertsButton = FindViewById<ImageButton>(Resource.Id.alertsImgBtn);
            ImageButton rosterButton = FindViewById<ImageButton>(Resource.Id.rosterImgBtn);


            // when calendarButton is clicked, open up CalendarScreen.axml
            // also start activity CalendarActivity.cs (activity controlling actions for the calendar screen)
            calendarButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CalendarActivity));
                StartActivity(intent);
            };

            // when alertsButton is clicked, open up AlertScreen.axml
            // also start activity AlertsActivity.cs (activity controlling actions for the calendar screen)
            alertsButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AlertsActivity));
                StartActivity(intent);
            };
        }
    }
}