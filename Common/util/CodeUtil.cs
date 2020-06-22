using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CodeUtil
    {

        private static string activatePagePath = "activate-account";
        private static string lostPasswordPagePath = "lost-password";
        private static string addPartnerPagePath = "add-partner";

        public static int generateCode()
        {
            Random rdm = new Random();
            return rdm.Next(00000, 99999);
        }

        public static string generateFakePassCode()
        {
            Random rdm = new Random();
            return "change" + rdm.Next(00000, 99999) + "me";
        }

        public static string buildActivationUrl(string baseUrl, Guid id)
        {
            Dictionary<string, string> Pairs = new Dictionary<string, string>();
            Pairs.Add("id", id.ToString());
            string result = baseUrl + "/" + activatePagePath + "/" + Base64Encode(JsonConvert.SerializeObject(Pairs)); ;
            return result;
        }

        public static string buildLostPasswordUrl(string baseUrl, Guid id)
        {
            Dictionary<string, string> Pairs = new Dictionary<string, string>();
            Pairs.Add("id", id.ToString());
            string result = baseUrl + "/" + lostPasswordPagePath + "/" + Base64Encode(JsonConvert.SerializeObject(Pairs)); ;
            return result;
        }
        public static string buildAcceptLinkPartnerUrl(string baseUrl, Guid id)
        {
            Dictionary<string, string> Pairs = new Dictionary<string, string>();
            Pairs.Add("id", id.ToString());
            string result = baseUrl + "/" + addPartnerPagePath + "/" + Base64Encode(JsonConvert.SerializeObject(Pairs)); ;
            return result;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
