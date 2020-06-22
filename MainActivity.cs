using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Gms.Maps;
using Android;
using System;
using Android.Gms.Maps.Model;
using Android.Gms.Location;
using Android.Content.PM;
using ActivityCompat = Android.Support.V4.App.ActivityCompat;
using Android.Views;
using Android.Support.V7.Widget;
using Slot.Helpers;

namespace Slot
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {
        readonly string[] permissionGroup = { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation };

        Android.Widget.TextView placeTextView;

        GoogleMap map;
        FusedLocationProviderClient LocationProviderClient;
        Android.Locations.Location myLastLocation;
        private LatLng myposition;


        public bool activityCompat { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set the view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Slot Toolbar";

            RequestPermissions(permissionGroup, 0);
          
            SupportMapFragment mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

            placeTextView = (Android.Widget.TextView)FindViewById(Resource.Id.placeTextView);

 

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if(grantResults.Length < 1)
            {
                return;
            }

            if(grantResults[0] == (int)Android.Content.PM.Permission.Granted)
            {
                DisplayLocation();
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            var mapStyle = MapStyleOptions.LoadRawResourceStyle(this, Resource.Raw.mapstyle);
            googleMap.SetMapStyle(mapStyle);
            map = googleMap;

            map.UiSettings.ZoomControlsEnabled = true;
            map.CameraMoveStarted += Map_CameraMoveStarted;
            map.CameraIdle += Map_CameraIdle;

            if (CheckPermission())
            {
                DisplayLocation();
            }
        }

        private async void Map_CameraIdle(object sender, EventArgs e)
        {
            var position = map.CameraPosition.Target;
            string key = Resources.GetString(Resource.String.mapkey);
            string address =  await MapHelpers.FindCordinateAddress(position, key);
            if (!string.IsNullOrEmpty(address))
            {
                placeTextView.Text = address;
            }
            else
            {
                placeTextView.Text = "Incotro?";
            }
            
            placeTextView.Text = address;
        
        }

        private void Map_CameraMoveStarted(object sender, GoogleMap.CameraMoveStartedEventArgs e)
        {
            placeTextView.Text = "Setting new location";
        }

        bool  CheckPermission()
        {
            bool permissionGranted = false;
          if(ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Android.Content.PM.Permission.Granted && 
                ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted)
            {
                permissionGranted = false;
            }
            else
            {
                permissionGranted = true;
            }
            return permissionGranted;
        }

        async void DisplayLocation()
        {
            if(LocationProviderClient  == null)
            {
                LocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);      
            }
            myLastLocation = await LocationProviderClient.GetLastLocationAsync();
            if(myLastLocation != null)
            {
                myposition = new LatLng(myLastLocation.Latitude, myLastLocation.Longitude);
                map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(myposition, 15));
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            string textToShow;
            if (item.ItemId == Resource.Id.menu_info)
                textToShow = "Informations";
            else
                textToShow = "Overflow";

            Android.Widget.Toast.MakeText(this, item.TitleFormatted + ": " + textToShow,
                Android.Widget.ToastLength.Long).Show();

            return base.OnOptionsItemSelected(item);
        }


    }
}

 