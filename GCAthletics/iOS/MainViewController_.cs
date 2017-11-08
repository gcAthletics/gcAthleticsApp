using Foundation;
using System;
using UIKit;

namespace gcAthletics.iOS
{
    public partial class MainViewController : UIViewController
    {
        UIViewController rosterViewController;
        UIButton teamListBtn;

        public MainViewController (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib()
        {
            // Called when loaded from xib or storyboard.

            this.Initialize();
        }

        public void Initialize()
        {

            //Instantiating View Controller with Storyboard ID 'RosterViewController'
            rosterViewController = Storyboard.InstantiateViewController("RosterViewController") as RosterViewController;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //When we push the button, we will push the RosterViewController onto our current Navigation Stack
            teamListBtn.TouchUpInside += delegate
            {
                this.NavigationController.PushViewController(rosterViewController, true);
            };
        }
    }
}