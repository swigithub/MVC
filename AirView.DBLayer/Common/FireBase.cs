using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WI.Libraries.Common
{
    /*----MoB!----*/
  public  class FireBase
    {

        public dynamic SendNotification(string FirebaseKey, string DevieToken, string JsonData)
        {

            try
            {
                dynamic result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + FirebaseKey);
                httpWebRequest.Method = "POST";
                // VMT,MT,SMSMT
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //  string json = "{\"to\": \"dfk90rb_znE:APA91bHLegH5Ru1zVNj-aDnV6zrm7ewOFmjl9QvM7N9wL-wnInpqBlHWfWyICpcH6lJgRSux2M-EkVXhtRlGdb2REnwftdE-4xgXAOxm54U-c35PIB7k7Q-o-wepxhC0_30QQp3D2jhe\",\"data\": {\"message\": \"This is a Firebase Cloud Messaging Topic Message!\",}}";
                    string json = "{ \"data\": "+ JsonData + ",\"to\" : \"" + DevieToken + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                 return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
