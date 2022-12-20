using ProjectEarth_Data_Editor.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEarth_Data_Editor
{
    public static class Converter
    {
        public static JsonObject SharedToNormal(JsonObject jo)
        {
            JsonObject buildplate = (JsonObject)jo["result"];
            JsonObject buildPlateData = (JsonObject)buildplate["buildplateData"];

            JsonObject res = new JsonObject();
            res.Add("blocksPerMeter", buildPlateData["blocksPerMeter"]);
            res.Add("dimension", buildPlateData["dimension"]);
            res.Add("eTag", "dsasdasda");
            res.Add("id", Guid.NewGuid().ToString()); // random
            res.Add("isModified", "false");
            res.Add("lastUpdated", "2022-04-13T12:42:24Z"); // TODO: change to 1970-1-1...
            res.Add("locked", "false");
            res.Add("model", buildPlateData["model"]);
            res.Add("numberOfBlocks", "0"); // Not used?
            res.Add("offset", buildPlateData["offset"]);
            res.Add("order", "0");
            res.Add("requiredLevel", "0");
            res.Add("surfaceOrientation", buildPlateData["surfaceOrientation"]);
            res.Add("templateId", "00000000-0000-0000-0000-000000000000"); // idk
            res.Add("type", buildPlateData["type"]);
            return res;
        }
    }
}
