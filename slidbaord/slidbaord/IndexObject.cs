using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace slidbaord
{
    public class IndexObject
    {
        //_id object from MongoDB
        //Using this for faster query
        public String _id = "";
        
        //File's originating device's Id 
        public String deviceId = "";

        //File's full path in the device
        public String fullPath = "";

        //File's UUID
        public String id = "";

        //File's name
        public String name = "";

        //File's parent
        public String parent = "";
    }
}
