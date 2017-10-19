// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace gcAthletics.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UIButton Button { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AlertBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AlertButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AthleteName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CalendarBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ContactImg { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Name { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Position { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PositionLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Team { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TeamLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TeamListBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton WeightsBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Welcome { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel WelcomeLabel { get; set; }

        [Action ("AlertBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AlertBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AlertBtn != null) {
                AlertBtn.Dispose ();
                AlertBtn = null;
            }

            if (AlertButton != null) {
                AlertButton.Dispose ();
                AlertButton = null;
            }

            if (AthleteName != null) {
                AthleteName.Dispose ();
                AthleteName = null;
            }

            if (CalendarBtn != null) {
                CalendarBtn.Dispose ();
                CalendarBtn = null;
            }

            if (ContactImg != null) {
                ContactImg.Dispose ();
                ContactImg = null;
            }

            if (Name != null) {
                Name.Dispose ();
                Name = null;
            }

            if (Position != null) {
                Position.Dispose ();
                Position = null;
            }

            if (PositionLbl != null) {
                PositionLbl.Dispose ();
                PositionLbl = null;
            }

            if (Team != null) {
                Team.Dispose ();
                Team = null;
            }

            if (TeamLbl != null) {
                TeamLbl.Dispose ();
                TeamLbl = null;
            }

            if (TeamListBtn != null) {
                TeamListBtn.Dispose ();
                TeamListBtn = null;
            }

            if (WeightsBtn != null) {
                WeightsBtn.Dispose ();
                WeightsBtn = null;
            }

            if (Welcome != null) {
                Welcome.Dispose ();
                Welcome = null;
            }

            if (WelcomeLabel != null) {
                WelcomeLabel.Dispose ();
                WelcomeLabel = null;
            }
        }
    }
}