using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ufinix.Helpers
{


    public class GeocodingParser
    {
        public long PlaceId { get; set; }
        public string Licence { get; set; }
        public string OsmType { get; set; }
        public long OsmId { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string display_name { get; set; }
        public Address Address { get; set; }
        public List<string> Boundingbox { get; set; }
    }

    public class Address
    {
        public string Road { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public long Postcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }
}