/*
 * This class can be used to build different layouts in listViews
 * An object of this class is added to a list, and each object in
 * the list is added to a screen's listview. Not all elements of this
 * class need to be instantiated, only the ones that will be used.
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

namespace CustomRowView {
    public class TableItem {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string DateHeading { get; set; }
        public string phoneNumber { get; set; }
        public int ImageResourceId { get; set; }
    }
}