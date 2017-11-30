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

namespace GCAthletics.Droid
{
    [Activity(Label = "Assign Workout")]
    public class AddWorkoutActivity : Activity
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

            SetContentView(Resource.Layout.AddAlertLayout);

            EditText detailsText = FindViewById<EditText>(Resouce.Id.InnsWorkText);
            Switch 

        }
    }
}