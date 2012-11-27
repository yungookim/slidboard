using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Net.Http;

namespace slidboard
{
    class HttpClient
    {
        private const string TEMP_LOCATION = "C:\\tmp\\";

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

        public static String getFile(String deviceId, String fileFullPath)
        {
            int extLength = fileFullPath.Length - fileFullPath.LastIndexOf(".");
            String fileExt = fileFullPath.Substring(fileFullPath.LastIndexOf("."), extLength);

            JSONRequestIndex reqMsg = new JSONRequestIndex(deviceId, fileFullPath);
            JSONMessageWrapper msgWrapper = new JSONMessageWrapper("getFile", reqMsg.request());
            String response = HttpClient.GET("getFile", msgWrapper.getMessage());

            if (!string.IsNullOrEmpty(response))
            {
                byte[] filebytes = Convert.FromBase64String(response);
                Guid gid = Guid.NewGuid();
                FileStream fs = new FileStream(TEMP_LOCATION + gid + fileExt,
                                               FileMode.CreateNew,
                                               FileAccess.Write,
                                               FileShare.None);
                fs.Write(filebytes, 0, filebytes.Length);
                fs.Close();
                return TEMP_LOCATION + gid.ToString() + fileExt;
            }

            return "";
        }
    }
}
