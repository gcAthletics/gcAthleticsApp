using Android.App;
using Android.Widget;
using Android.OS;

namespace GCAthletics.Droid
{
    [Activity(Label = "GC Athletics", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var imageView = FindViewById<ImageView>(Resource.Id.thunderImage);
            imageView.SetImageResource(Resource.Mipmap.thunder);

            // Get our button from the layout resource,
            // and attach an event to it
            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);

            loginButton.Click += (sender, e) =>
            {
                SetContentView(Resource.Layout.HomeScreen);
            };

            //button.Click += delegate { button.Text = $"{count++} clicks!"; };
        }
    }
}

