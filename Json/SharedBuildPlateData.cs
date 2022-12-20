using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEarth_Data_Editor.Json
{
    [Serializable]
    public class SharedBuildPlateData
    {
        public string type { get; set; }
        public string model { get; set; }
        public BuildPlateOffset offset { get; set; }
        public BuildPlateDimensions dimension { get; set; }
        public double blocksPerMeter { get; set; }
        public string surfaceOrientation { get; set; }
        public int order { get; set; }
    }
}
