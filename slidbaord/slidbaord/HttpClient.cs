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
            //Make sure the trailing empty line is removed
            return Parser.parseIndexes(response);
        }

        public static String getFile(String deviceId, String fileFullPath)
        {
            JSONRequestIndex reqMsg = new JSONRequestIndex(deviceId, fileFullPath);
            JSONMessageWrapper msgWrapper = new JSONMessageWrapper("getFile", reqMsg.request());
            String response = HttpClient.GET("getFile", msgWrapper.getMessage());

            Console.WriteLine(response);
            return "";
        }
    }
}
