using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Gms.Maps;
using Android;
using System;
using Android.Gms.Maps.Model;
using Android.Gms.Location;
using Android.Support.V4.App;
using Slot.Helpers;


namespace Slot
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {
        readonly string[] permissionGroup = { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation };

        TextView placeTextView;
        Button getDirectionButton;

        GoogleMap map;
        FusedLocationProviderClient locationProviderClient;
        Android.Locations.Location myLastLocation;
        LatLng myposition;
        LatLng destinationPoint;

        MapHelpers mapHelper = new MapHelpers();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            RequestPermissions(permissionGroup, 0);

            SupportMapFragment mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

            placeTextView = (TextView)FindViewById(Resource.Id.placeTextView);
            getDirectionButton = (Button)FindViewById(Resource.Id.getDirectionsButton);
            getDirectionButton.Click += GetDirectionButton_Click;


        }

        private async void GetDirectionButton_Click(object sender, EventArgs e)
        {
            string key = Resources.GetString(Resource.String.mapkey);
            string directionJson = await mapHelper.GetDirectionJsonAsync(myposition, destinationPoint, key);
            Console.WriteLine(directionJson);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (grantResults.Length < 1)
            {
                return;
            }

            if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
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
            map.CameraChange += Map_CameraIdle;


            if (CheckPermission())
            {
                DisplayLocation();
            }
            DisplayLocation();
        }

        private async void Map_CameraIdle(object sender, GoogleMap.CameraChangeEventArgs e)
        {

            destinationPoint = map.CameraPosition.Target;
            string key = Resources.GetString(Resource.String.mapkey);
            string address = await mapHelper.FindCoordinateAddress(destinationPoint, key);

            if (!string.IsNullOrEmpty(address))
            {
                placeTextView.Text = address;
            }
            else
            {

                placeTextView.Text = "Incotro?";
            }
        }

        private void Map_CameraMoveStarted(object sender, GoogleMap.CameraMoveStartedEventArgs e)
        {

            placeTextView.Text = "Se seteaza o noua locatie";
        }

        bool CheckPermission()
        {
            bool permissionGranted = false;
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted &&
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
            if (locationProviderClient == null)
            {
                locationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            }
            myLastLocation = await locationProviderClient.GetLastLocationAsync();
            if (myLastLocation != null)
            {
                myposition = new LatLng(myLastLocation.Latitude, myLastLocation.Longitude);
                map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(myposition, 15));
            }
        }
    }

}