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
using Android;

namespace GCAthletics.Droids
{
    [Activity(Label = "Register Team")]
    public class RegisterActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RegisterScreen);

            EditText regNameText = FindViewById<EditText>(Resource.Id.InsRegName);
            EditText regPhoneText = FindViewById<EditText>(Resource.Id.InsRegPhone);
            EditText regEmailText = FindViewById<EditText>(Resource.Id.InsRegEmail);
            EditText regTeamNameText = FindViewById<EditText>(Resource.Id.InsRegTeamName);
            EditText regSportText = FindViewById<EditText>(Resource.Id.InsRegSport);
            Button regButton = FindViewById<Button>(Resource.Id.registerTeamBtn);

            regButton.Click += (sender, e) =>
            {

            };

        }
    }
}