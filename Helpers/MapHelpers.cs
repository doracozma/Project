using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using ufinix.Helpers;

namespace Slot.Helpers
{
   public class MapHelpers
    {
        public async Task<string> FindCoordinateAddress(LatLng position, string mapkey)
        {
           
            string url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + position.Latitude.ToString() + "," + position.Longitude.ToString() + "&key=" + mapkey;
            string placeAddress = "";
            var handler = new HttpClientHandler();
            HttpClient httpClient = new HttpClient(handler);
            string result =  await httpClient.GetStringAsync(url);

            if (!string.IsNullOrEmpty(result))
            {
                var geoCodeData = JsonConvert.DeserializeObject<GeocodingParser>(result);
                if (geoCodeData.status.Contains("OK"))
                {
                    placeAddress = geoCodeData.results[0].formatted_address;
                }
            }
            return placeAddress;
        }


        public async Task<string> GetDirectionJsonAsync(LatLng location, LatLng destination, string mapkey)
        {
           //https://maps.googleapis.com/maps/api/directions/json?origin=47.6580964,23.5241103&destination=47.661993,23.5523382&mode=driving&key=AIzaSyD5tTtNSJQTE6fdHr26R89VUv-HMu_uHUE
            //origin
            string str_origin = "origin" + location.Latitude.ToString() + "," + location.Longitude.ToString();

            //destination
            string str_destination = "destination" + destination.Latitude.ToString() + "," + destination.Longitude.ToString();

            //Mode
            string mode = "mode=driving";

            string parameters = str_origin + "&" + str_destination + "&" + mode + "&key=" + mapkey;

            string output = "json";

            string url = "https://maps.googleapis.com/maps/api/directions/" + output + "?" + parameters;

            var handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string jsonString = await client.GetStringAsync(url);

            return jsonString;
        }
        
    }
}