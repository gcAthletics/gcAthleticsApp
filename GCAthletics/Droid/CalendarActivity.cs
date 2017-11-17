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
    [Activity(Label = "Calendar", MainLauncher = false)]
    public class CalendarActivity : Activity
    {
        string email = null;
        int teamID = -1;        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            email = Intent.Extras.GetString("email");
            teamID = Intent.Extras.GetInt("teamID");

            SetContentView(Resource.Layout.CalendarScreen);
            // Create your application here
        }

        //when back button is pressed, go to home screen
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            Intent.PutExtra("email", email);
            Intent.PutExtra("teamID", teamID);
            StartActivity(intent);
        }
    }
}