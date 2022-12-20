using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEarth_Data_Editor.Json
{
    public struct JsonSerializationSettings
    {
        public static JsonSerializationSettings Default = new JsonSerializationSettings(true, true);

        public bool EnterNewLines;
        public bool AddTab;

        public JsonSerializationSettings(bool _enterNewLines, bool _addTab)
        {
            EnterNewLines = _enterNewLines;
            AddTab = _addTab;
        }
    }
}
