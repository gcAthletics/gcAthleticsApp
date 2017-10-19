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
        UIKit.UIButton AlertButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Name { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Position { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Team { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Welcome { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AlertButton != null) {
                AlertButton.Dispose ();
                AlertButton = null;
            }

            if (Name != null) {
                Name.Dispose ();
                Name = null;
            }

            if (Position != null) {
                Position.Dispose ();
                Position = null;
            }

            if (Team != null) {
                Team.Dispose ();
                Team = null;
            }

            if (Welcome != null) {
                Welcome.Dispose ();
                Welcome = null;
            }
        }
    }
}