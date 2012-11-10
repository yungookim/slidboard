using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace slidbaord
{
    class HttpClient
    {

        public HttpClient(String ip)
        {

        }


        WebRequest request = WebRequest.Create("http://69.164.219.86:8081/init?asdf=a");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Console.WriteLine(response.StatusDescription);
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    Console.WriteLine(reader.ReadToEnd());
    }
}
