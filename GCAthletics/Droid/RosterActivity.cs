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
using CustomRowView;
using System.Data.SqlClient;

namespace GCAthletics.Droid
{
    [Activity(Label = "Roster", MainLauncher = false)]
    public class RosterActivity : Activity
    {
        List<TableItem> tableItems = new List<TableItem>();
        ListView listView;

        string email = null;
        int teamID = -1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            email = Intent.Extras.GetString("email");
            int teamID = Intent.Extras.GetInt("TeamID");

            SetContentView(Resource.Layout.RosterScreen);

            listView = FindViewById<ListView>(Resource.Id.rosterListView);

            Button addPlayerBtn = FindViewById<Button>(Resource.Id.addPlayerBtn);

            try
            {
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                List<UserModel> sqlList = dbu.getAllUsersByTeamID(teamID).ToList();

                foreach(var member in sqlList)
                {
                    tableItems.Add(new TableItem() { Heading = member.Name, SubHeading = member.Email, phoneNumber = member.Phone });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

            listView.Adapter = new RosterActivityAdapter(this, tableItems);

            addPlayerBtn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AddPlayerActivity));
                intent.PutExtra("email", email);
                intent.PutExtra("teamID", teamID);
                StartActivity(intent);
            };
            
        }

        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            intent.PutExtra("email", email);
            StartActivity(intent);
        }
    }
}