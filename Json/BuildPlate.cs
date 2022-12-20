using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEarth_Data_Editor.Json
{
    [Serializable]
    public class BuildPlate
    {
        public double blocksPerMeter { get; set; }
        public BuildPlateDimensions dimension { get; set; }
        public string eTag { get; set; }
        public Guid id { get; set; }
        public bool isModified { get; set; }
        public string lastUpdated { get; set; }
        public bool locked { get; set; }
        public string model { get; set; }
        public int numberOfBlocks { get; set; }
        public BuildPlateOffset offset { get; set; }
        public int order { get; set; }
        public int requiredLevel { get; set; }
        public string surfaceOrientation { get; set; }
        public string templateId { get; set; }
        public string type { get; set; }
    }
}
