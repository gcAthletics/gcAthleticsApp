﻿using System;
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
    [Activity(Label = "Announcements", MainLauncher = false)]
    public class AlertsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AlertScreen);

            // Create your application here
        }

        //when back button is pressed, go to log in screen
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            StartActivity(intent);
        }
    }
}