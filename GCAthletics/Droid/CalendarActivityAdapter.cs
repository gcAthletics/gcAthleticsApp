/*
 * This is the class that controls the listView inside of CalendarScreen.axml
 * It works by populating a view made from AlertLayout.axml and an event's information
 * The view is then added to the listView from CalendarScreen.axml
 */

using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using CustomRowView;

namespace GCAthletics.Droid
{
    public class CalendarActivityAdapter : BaseAdapter<TableItem>
    {
        List<TableItem> items;
        Activity context;
        public CalendarActivityAdapter(Activity context, List<TableItem> items): base()
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

        public override int Count 
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.EventLayout, null);
            var text1 = view.FindViewById<TextView>(Resource.Id.headerTxt1);
            text1.Text = item.Heading;
            var text2 = view.FindViewById<TextView>(Resource.Id.bodyTxt1);
            text2.Text = item.SubHeading;
            var text3 = view.FindViewById<TextView>(Resource.Id.dateTxt1);
            text3.Text = item.DateHeading;

            return view;
        }
    }
}
