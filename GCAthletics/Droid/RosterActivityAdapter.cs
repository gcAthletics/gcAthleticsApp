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

namespace GCAthletics.Droid
{
    public class RosterActivityAdapter : BaseAdapter<TableItem>
    {
        List<TableItem> items;
        Activity context;

        public RosterActivityAdapter(Activity context, List<TableItem> items): base()
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
            get { return items.Count;  }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;

            if (view == null)
                view = context.LayoutInflater.Inflate(GCAthletics.Droid.Resource.Layout.RosterLayout, null);

            var nameTxt = view.FindViewById<TextView>(GCAthletics.Droid.Resource.Id.nameTxt);
            nameTxt.Text = item.Heading;
            var emailTxt = view.FindViewById<TextView>(GCAthletics.Droid.Resource.Id.emailTxt);
            emailTxt.Text = item.SubHeading;
            var phoneTxt = view.FindViewById<TextView>(GCAthletics.Droid.Resource.Id.phoneTxt);
            phoneTxt.Text = item.phoneNumber.ToString();

            return view;
        }
    }
}