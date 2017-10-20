// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace gcAthletics.iOS
{
    [Register ("TaskDetailViewController")]
    partial class TaskDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        gcAthletics.iOS.WeightController dataSource { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        gcAthletics.iOS.WeightController @delegate { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (dataSource != null) {
                dataSource.Dispose ();
                dataSource = null;
            }

            if (@delegate != null) {
                @delegate.Dispose ();
                @delegate = null;
            }
        }
    }
}