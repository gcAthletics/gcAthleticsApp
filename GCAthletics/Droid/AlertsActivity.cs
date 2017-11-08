/*
 *  Check this out
 *  https://developer.xamarin.com/guides/android/user_interface/layouts/list-view/part-3-customizing-list-view-appearance/
 * 
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
using BuiltInViews;
using System.Data.SqlClient;

namespace GCAthletics.Droid
{
    [Activity(Label = "Alerts", MainLauncher = false)]
    public class AlertsActivity : Activity
    {
        List<TableItem> tableItems = new List<TableItem>();
        ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AlertsScreen);

            listView = FindViewById<ListView>(Resource.Id.alertListView);

            try
            {
                DButility dbu = new DButility();
                SqlConnection connection = dbu.createConnection();

                List<AnnouncementsModel> sqlList = dbu.getAllAnnouncements().ToList();

                foreach(var announcement in sqlList){
                    tableItems.Add(new TableItem() { Heading = announcement.Name, SubHeading = announcement.Description });
                }
            }
            catch (SqlException ex){
                Console.WriteLine(ex.ToString());
            }

            listView.Adapter = new AlertsActivityAdapter(this, tableItems);

            listView.ItemClick += OnListItemClick;

        }

        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var item = tableItems[e.Position];
            Android.Widget.Toast.MakeText(this, item.Heading, Android.Widget.ToastLength.Short).Show();
            Console.WriteLine("Clicked on " + item.Heading);
        }

        //when back button is pressed, go to home screen
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(HomeActivity));
            StartActivity(intent);
        }
    }
}