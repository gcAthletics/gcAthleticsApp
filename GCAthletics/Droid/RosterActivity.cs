/*
 * This is the class that controls the functionality of the Roster Page on the app. It uses the components from Resource.Layout.RosterScreen.axml 
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
using CustomRowView;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace GCAthletics.Droid
{
    [Activity(Label = "Roster", MainLauncher = false)]
    public class RosterActivity : Activity
    {
        //holds list of player's data and coach's data on the team
        List<TableItem> tableItems = new List<TableItem>();
        ListView listView;

        //holds current user's data
        string email = null;
        int teamID = -1;
        UserModel usrModel = new UserModel();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //get current user's data from previous screen
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            email = usrModel.Email;
            teamID = usrModel.TeamID;

            //set view to that of RosterScreen.axml
            SetContentView(Resource.Layout.RosterScreen);

            //get listView from layout resource
            listView = FindViewById<ListView>(Resource.Id.rosterListView);

            //get button from layout resource and hide it, it will be unhidden if user has role of coach
            Button addPlayerBtn = FindViewById<Button>(Resource.Id.addPlayerBtn);
            addPlayerBtn.Visibility = ViewStates.Gone;

            try
            {
                //conntect to database
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                //check if current user is a coach, if so, unhide addPlayerBtn
                UserModel usrModel = dbu.getUserByEmail(email);
                if(usrModel.Role.Equals("coach", StringComparison.InvariantCultureIgnoreCase))
                {
                    addPlayerBtn.Visibility = ViewStates.Visible;
                }

                //gget all players and coaches on the current user's team
                List<UserModel> sqlList = dbu.getAllUsersByTeamID(teamID).ToList();
                //for each member of the team, create create a new AlertLayout.axml view for each using tableItems
                foreach(var member in sqlList)
                {
                    tableItems.Add(new TableItem() { Heading = member.Name, SubHeading = member.Email, phoneNumber = member.Phone });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

            //attach rosterActivityAdapter to the listView
            listView.Adapter = new RosterActivityAdapter(this, tableItems);

            listView.ItemClick += OnListItemClick;

            //when addPlayerBtn is clciked, go to Add Player Screen
            //also send the current user's data
            addPlayerBtn.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(AddPlayerActivity));
                intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
                StartActivity(intent);
            };
        }

        //when the back button is pressed, go to the home screen
        //also send the current user's data
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));
            intent.PutExtra("user", JsonConvert.SerializeObject(usrModel));
            StartActivity(intent);
        }

        //when a player item is clicked on in the list, check to see
        // if the current user has the coach role, then ask the current
        // user if they want to remove that player/coach from the team
        // and delete their account
        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            //get item that was clicked on
            var listView = sender as ListView;
            var item = tableItems[e.Position];

            //get current user data from previous screen
            usrModel = JsonConvert.DeserializeObject<UserModel>(Intent.GetStringExtra("user"));

            //check to see if current user has role coach
            if (usrModel.Role.Equals("coach", StringComparison.InvariantCultureIgnoreCase))
            {
                //create option dialog
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("Remove Player");
                alert.SetMessage("Would you like to remove " + item.Heading + " from the team?");
                //option 1, exit dialog
                alert.SetButton("No", (c, ev) =>
                {
                    alert.Hide();
                });
                //option 2, remove player/coach from team
                alert.SetButton2("Yes", (c, ev) =>
                {
                    try
                    {
                        //connect to database
                        DButility dbu = new DButility();
                        UserModel rmvModel = dbu.getUserByEmail(item.SubHeading);

                        //remove user from database
                        dbu.removeUser(rmvModel.ID);

                        //alert current user that the selected user was removed
                        string toast = "Successfully removed user " + item.Heading + " from the team.";
                        Toast.MakeText(this, toast, ToastLength.Long).Show();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
                //show option dialog
                alert.Show();
            }
        }
    }
}