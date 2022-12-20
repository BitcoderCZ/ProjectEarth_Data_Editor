using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEarth_Data_Editor.Json
{
    public class JsonObject
    {
        public Dictionary<string, object> Values;

        public bool HasValue => Values.Count > 0;

        public JsonObject() : this(new Dictionary<string, object>())
        { }
        public JsonObject(string key, object value) : this(new Dictionary<string, object>() { { key, value } })
        { }
        public JsonObject(Dictionary<string, object> _values)
        {
            Values = _values;
        }

        public object this[string key] { get => Values[key]; set => Values[key] = value; }

        public void Add(string key, object value)
        {
            if (Values.ContainsKey(key))
                Values[key] = value;
            else
                Values.Add(key, value);
        }

        public override string ToString() => JsonSerializer.Serialize(this, JsonSerializationSettings.Default);
    }
}
