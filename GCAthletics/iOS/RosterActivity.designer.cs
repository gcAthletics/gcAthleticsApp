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
    [Register ("RosterActivity")]
    partial class RosterActivity
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Phone#Lbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PlayerNameLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PlyNm1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PositionLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView Roster1Pic { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RosterPic { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Phone#Lbl != null) {
                Phone#Lbl.Dispose ();
                Phone#Lbl = null;
            }

            if (PlayerNameLbl != null) {
                PlayerNameLbl.Dispose ();
                PlayerNameLbl = null;
            }

            if (PlyNm1 != null) {
                PlyNm1.Dispose ();
                PlyNm1 = null;
            }

            if (PositionLbl != null) {
                PositionLbl.Dispose ();
                PositionLbl = null;
            }

            if (Roster1Pic != null) {
                Roster1Pic.Dispose ();
                Roster1Pic = null;
            }

            if (RosterPic != null) {
                RosterPic.Dispose ();
                RosterPic = null;
            }
        }
    }
}