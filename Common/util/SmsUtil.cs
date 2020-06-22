using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Common
{
    public class SmsUtil
    {

        private static readonly string API_KEY = "40cfe0b980e1bd897b61910b77e60c0e523af53f";
        private static readonly string API_SECRET = "b5231e685ff3a232479fee8d8e5b5e1acc38ba4230f0500b480df7b11520600920c0a64f27163d7bfb8feffa829190c64d434ceb21de460b818143607612c33d";
        public static void sendSMS(string message, string phoneNumber)
        {
            try
            {
                byte[] result;
                SHA512 shaM = new SHA512Managed();

                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                string apiKey = API_KEY;
                string nonce = unixTimestamp.ToString();
                string method = "POST";
                string url = "/prepaid/message";
                string sender = "";
                string recipient = phoneNumber;
                string visibleMessage = message;
                string scheduleDate = "";
                string validityDate = "";
                string callbackUrl = "";
                string secret = API_SECRET; // value provided

                byte[] byteArray = Encoding.UTF8.GetBytes(apiKey + nonce + method + url + sender + recipient + message + visibleMessage + scheduleDate + validityDate + callbackUrl + secret);

                result = shaM.ComputeHash(byteArray);

                string hashString = string.Empty;
                foreach (byte x in result)
                {
                    hashString += String.Format("{0:x2}", x);
                }


                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://www.web2sms.ro/prepaid/message");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.ProtocolVersion = HttpVersion.Version11;
                httpWebRequest.Method = "POST";

                string _auth = string.Format("{0}:{1}", apiKey, hashString);
                string _enc = Convert.ToBase64String(Encoding.ASCII.GetBytes(_auth));
                string _cred = string.Format("{0} {1}", "Basic", _enc);

                httpWebRequest.Headers[HttpRequestHeader.Authorization] = _cred;

                Encoding encoding = new UTF8Encoding();
                string postData = "{\"apiKey\":\"" + apiKey +
                    "\",\"sender\":\"" + sender +
                    "\",\"recipient\":\"" + recipient +
                    "\",\"message\":\"" + message +
                    "\",\"scheduleDatetime\":\"" + scheduleDate +
                    "\",\"validityDatetime\":\"" + validityDate +
                    "\",\"callbackUrl\":\"" + callbackUrl +
                    "\",\"userData\":\"" + "" +
                    "\",\"visibleMessage\":\"" + visibleMessage +
                    "\",\"nonce\":\"" + nonce + "\"}";
                byte[] data = encoding.GetBytes(postData);


                Stream stream = httpWebRequest.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                string s = response.ToString();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                String jsonresponse = "";
                String temp = null;
                while ((temp = reader.ReadLine()) != null)
                {
                    jsonresponse += temp;
                }

            }
            catch (WebException e)
            {

                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        Console.WriteLine(text);
                    }
                }
            }
        }
    }
}