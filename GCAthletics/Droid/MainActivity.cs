using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace GCAthletics.Droid
{
    [Activity(Label = "GC Athletics", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var imageView = FindViewById<ImageView>(Resource.Id.thunderImage);
            imageView.SetImageResource(Resource.Mipmap.thunder);

            // Get login button from the layout resource,
            // and attach an event to it
            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);

            // when login button is clicked, open up HomeScreen.axml (the app home screen)
            // also start activity HomeActivity.cs (activity controlling actions for the app home screen)
            loginButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(HomeActivity));
                StartActivity(intent);
            };
        }
    }
}

