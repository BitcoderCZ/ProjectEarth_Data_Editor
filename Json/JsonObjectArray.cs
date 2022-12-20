using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEarth_Data_Editor.Json
{
    public class JsonObjectArray
    {
        public List<JsonObject> objects;

        public JsonObjectArray(List<JsonObject> _objects)
        {
            objects = _objects;
        }

        public JsonObjectArray() : this(new List<JsonObject>())
        { }
    }
}
