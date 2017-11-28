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
using Newtonsoft.Json;

namespace GCAthletics.Droid
{
    [Activity(Label = "Roster", MainLauncher = false)]
    public class RosterActivity : Activity
    {
        List<TableItem> tableItems = new List<TableItem>();
        ListView listView;

        string email = null;
        int teamID = -1;

        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));

            email = usrModel.Email;
            teamID = usrModel.TeamID;

            SetContentView(Resource.Layout.RosterScreen);

            listView = FindViewById<ListView>(Resource.Id.rosterListView);

            Button addPlayerBtn = FindViewById<Button>(Resource.Id.addPlayerBtn);
            addPlayerBtn.Visibility = ViewStates.Gone;

            try
            {
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                UserModel usrModel = dbu.getUserByEmail(email);

                if(usrModel.Role.Equals("coach", StringComparison.InvariantCultureIgnoreCase))
                {
                    addPlayerBtn.Visibility = ViewStates.Visible;
                }

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

            listView.ItemClick += OnListItemClick;

            addPlayerBtn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AddPlayerActivity));
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };
        }

        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }

        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var item = tableItems[e.Position];

            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));

            if (usrModel.Role.Equals("coach", StringComparison.InvariantCultureIgnoreCase))
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Remove Player");
                alert.SetMessage("Would you like to remove " + item.Heading + " from the team?");
                alert.SetButton("No", (c, ev) =>
                {
                    alert.Hide();
                });
                alert.SetButton2("Yes", (c, ev) =>
                {
                    try
                    {
                        DButility dbu = new DButility();
                        UserModel rmvModel = dbu.getUserByEmail(item.SubHeading);
                        dbu.removeUser(rmvModel.ID);

                        string toast = "Successfully removed user " + item.Heading + " from the team.";
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
                alert.Show();
            }
        }
    }
}