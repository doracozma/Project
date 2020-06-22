using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
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
        public static async Task<String> FindCordinateAddress(LatLng position, string mapkey)
        {

            string url = "https://nominatim.openstreetmap.org/reverse?email=doracozma2008@yahoo.com&format=json&lat=" + position.Latitude.ToString() + "&lon=" + position.Longitude.ToString();
            string placeAddress = "";
            Thread.Sleep(500);
            var handler =  new HttpClientHandler();
            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Slot App For Univ1");

            string result =  await httpClient.GetStringAsync(url);
            if (!string.IsNullOrEmpty(result))
            {
                var geoCodeData = JsonConvert.DeserializeObject<GeocodingParser>(result);
                if (geoCodeData != null)
                {
                    placeAddress = geoCodeData.display_name;
                }
            }
            return placeAddress;
        }

        public static string TestLogin(string email, string pass)
        {
            string url = "http://tolorohost-001-site11.btempurl.com/api/authenticate/login";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"email\":\"are mere\"," +
                              "\"password\":\"123\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }

    }
}