/*
 * This is the class that controls the listView inside of AlertsScreen.axml
 * It works by populating a view made from AlertLayout.axml and an announcement's information
 * The view is then added to the listView from AlertsScreen.axml
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
using Android.Support.Design.Widget;
using CustomRowView;
using Android;

namespace BuiltInViews {
    public class AlertsActivityAdapter : BaseAdapter<TableItem> {
        List<TableItem> items;
        Activity context;
        public AlertsActivityAdapter(Activity context, List<TableItem> items): base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override TableItem this[int position]
        {   
            get { return items[position]; } 
        }

        public override int Count {
            get { return items.Count; } 
        }

        //set contents of the view to be whatever is passed in
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(GCAthletics.Droid.Resource.Layout.AlertLayout, null);
            var text1 = view.FindViewById<TextView>(GCAthletics.Droid.Resource.Id.headerTxt);
            text1.Text = item.Heading;
            var text2 = view.FindViewById<TextView>(GCAthletics.Droid.Resource.Id.bodyTxt);
            text2.Text = item.SubHeading;
            var text3 = view.FindViewById<TextView>(GCAthletics.Droid.Resource.Id.dateTxt);
            text3.Text = item.DateHeading;
            return view;
        }
    }
}