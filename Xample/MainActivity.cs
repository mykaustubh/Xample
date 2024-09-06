using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Util;
using System.Net;
using System.Linq;
using XampleProxy;
using System.Threading.Tasks;
using System.Threading;

namespace Xample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ProxyHttpClient _proxyHttpClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            // Initialize ProxyHttpClient with the base URL
            _proxyHttpClient = new ProxyHttpClient("https://ipinfo.io");
            // Call the GetAsync method with the endpoint path
            _ = _proxyHttpClient.GetAsync("/");

            PrintIpAddress(); // Call the method to print the IP address.
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void PrintIpAddress() // To fetch an IP address of the network.
        {
            try
            {
                string hostName = Dns.GetHostName();
                var ipEntry = Dns.GetHostEntry(hostName);
                var ipAddress = ipEntry.AddressList.FirstOrDefault(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                if (ipAddress != null)
                {
                    Log.Debug("Xample", $"Device IP Address: {ipAddress.ToString()}");
                }
                else
                {
                    Log.Debug("Xample", "No IP Address found.");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Xample", $"Error fetching IP address: {ex.Message}");
            }
        }
    }
}