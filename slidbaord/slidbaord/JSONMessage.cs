using System;
using System.Collections;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace slidboard
{

    //Message wrapper. Includes the origination of this message.
    public class JSONMessageWrapper
    {
        private const String CONNECTION_FROM = "PixelSense";

        //Let the server know where the connection is coming from
        public String from = CONNECTION_FROM;
        public String action = "";
        public String extraMsg = "";

        public JSONMessageWrapper(String action, String extraMsg)
        {
            this.action = action;
            this.extraMsg = extraMsg;
        }

        //Serialize current object to JSON then return
        public String getMessage()
        {
            string json = JsonConvert.SerializeObject(this);
            return json;
        }
    }

    //KV for requesting the index of a device
    public class JSONRequestIndex 
    {
        public String requestingDevice = "";
        public String path = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Device Id</param>
        /// <param name="dir">Requesting directory listing</param>
        /// <returns></returns>
        public JSONRequestIndex(String id, String dir)
        {
            this.requestingDevice = id;
            this.path = dir;
        }
        /// <summary>
        /// Returns the object in json format
        /// </summary>
        /// <returns>JSON Object in string format</returns>
        public String request() 
        {
            String json = JsonConvert.SerializeObject(this);
            return json;
        }
    }

    public class Parser
    {
        public static ArrayList parseIndexes(String list)
        {
            list = list.Trim();
            String[] items = list.Split('\n');
            ArrayList alItems = new ArrayList();

            foreach(String i in items)
            {
                IndexObject _temp = JsonConvert.DeserializeObject<IndexObject>(i);
                alItems.Add(_temp);
            }
            return alItems;
        }
    }


}
