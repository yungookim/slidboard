using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;

namespace slidbaord
{
    class HttpClient
    {

        public static String GET(String urn, String query)
        {
            WebRequest request = WebRequest.Create("http://69.164.219.86:8081/" + urn + "?msg=" + query);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(response.StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            return reader.ReadToEnd();
        }

        public static ArrayList getIndexObject(String deviceId, String dir)
        {
            JSONRequestIndex reqMsg = new JSONRequestIndex(deviceId, dir);
            JSONMessageWrapper msgWrapper = new JSONMessageWrapper("getIndex", reqMsg.request());
            String response = HttpClient.GET("getIndex", msgWrapper.getMessage());
            return Parser.parseIndexes(response);
        }
    }
}
